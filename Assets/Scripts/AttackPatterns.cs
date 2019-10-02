using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPatterns : MonoBehaviour
{
  private GameData _gameData;
  private int layerMask = 9;

  private Color _spaceSelectedColor = Color.green;

  private Color _spaceDeSelectedColor = Color.white;

  // the child object index of the child object Inner_Cube
  private int _innerCubeIndex = 1;

  // Extra distance needed for diagonal space calculations
  private float _diagonalOffset;

  // Start is called before the first frame update
  void Start()
  {

    _gameData = GetComponent<GameData>();
    if (_gameData == null)
    {
      Debug.LogError("Game Data object is NULL");
    }
  }

  // Update is called once per frame
  // void Update()
  // {

  // }




  public List<RaycastHit> StraightAttackSeekerRay(Vector3 position, Vector3 direction, float maxDistance, float spaceSize, int layerMask)
  {
    Vector3 pos = position;

    Vector3 rayDirection = transform.TransformDirection(direction);

    float rayDistance = maxDistance * spaceSize;

    RaycastHit[] rayResults = Physics.RaycastAll(pos, rayDirection, rayDistance, layerMask);


    List<RaycastHit> allAttackableSpaces = CalculateAttackableSpaces(rayResults, spaceSize);



    return allAttackableSpaces;

  }


  public List<RaycastHit> DiagonalAttackSeekerRay(Vector3 position, Vector3 direction, float maxDistance, float spaceSize, int layerMask)
  {
    // Shooting diagonally, we need extra distance equal to half the size of 1 space, which is 2.0f in this case since a space is 4x4
    _diagonalOffset = spaceSize / 2;

    spaceSize += _diagonalOffset;

    float rayDistance = maxDistance * spaceSize;

    Vector3 rayDirection = transform.TransformDirection(direction);

    Ray ray = new Ray(position, rayDirection);

    // RayCastAll() will let you get info from each collider the ray passes through (each one the ray hits)
    RaycastHit[] rayResults = Physics.RaycastAll(ray.origin, ray.direction, rayDistance, layerMask);



    List<RaycastHit> allAttackableSpaces = CalculateAttackableSpaces(rayResults, spaceSize);

    // return the hits you got from where the ray was fired (from the creature's (that accesses this script) position)
    return allAttackableSpaces;

  }



  public void HideAtkPattern(List<RaycastHit> hitsArray)
  {

    try
    {
      for (int i = 0; i < hitsArray.Count; i++)
      {
        // get the info from the current hit
        RaycastHit currentHit = hitsArray[i];

        // do not try to get renderer if we hit an enemy otherwise the catch() will activate and skip the rest of the RaycastHits
        if (!currentHit.transform.CompareTag("Enemy"))
        {
          if (currentHit.transform.parent.GetComponent<Space>() != null)
          {
            Space parentSpace = currentHit.transform.parent.GetComponent<Space>();

            parentSpace.setIsActiveMove(false);
          }

        }
      }
    }
    catch (System.Exception)
    {
      // throw;
    }
  }




  List<RaycastHit> CalculateAttackableSpaces(RaycastHit[] attackHits, float theSpaceSize)
  {
    List<RaycastHit> attackableSpaces = new List<RaycastHit>();

    RaycastHit temp;

    bool isEnemyHitPresent = false;


    // sort the array from least distance to greatest distance away from the ray
    for (int i = 0; i < attackHits.Length; i++)
    {
      if (attackHits[i].transform.CompareTag("Enemy"))
      {
        isEnemyHitPresent = true;

      }
      for (int j = i; j > 0; j--)
      {
        if (attackHits[j].distance < attackHits[j - 1].distance)
        {
          temp = attackHits[j];
          attackHits[j] = attackHits[j - 1];
          attackHits[j - 1] = temp;
        }
      }
    }

    // if at least one enemy hit is in the array, filter out the hits(the spaces) that have greater distance than the enemy hit, else when there isn't an enemy hit add everything in the array to the list
    if (isEnemyHitPresent == true)
    {

      float enemyDistance = 0.0f;

      for (int i = 0; i < attackHits.Length; i++)
      {
        if (attackHits[i].transform.CompareTag("Enemy"))
        {

          enemyDistance = attackHits[i].distance;
          // go through list to filter out all hits with greater distance than enemy's
          foreach (RaycastHit r in attackHits)
          {
            if (r.distance < enemyDistance)
            {
              attackableSpaces.Add(r);
            }
            else if (r.distance < enemyDistance + theSpaceSize / 4.0f && !r.transform.CompareTag("Enemy"))
            {
              attackableSpaces.Add(r);
            }
          }
          break;
        }

      }


    }
    else
    {
      foreach (RaycastHit r in attackHits)
      {
        attackableSpaces.Add(r);
      }
    }

    // // go in each space and see if the color of that space is blue, if it is, set the Inner_Cube child active
    foreach (RaycastHit rch in attackableSpaces)
    {
      Transform parentSpace = rch.transform.parent;

      // set the space to active so that it know to turn on the Inner_Cube child object
      if (parentSpace.GetComponent<Space>() != null)
      {
        Space space = parentSpace.GetComponent<Space>();

        space.setIsActiveMove(true);

      }

    }
    return attackableSpaces;
  }


} // End of AttackPatterns class
