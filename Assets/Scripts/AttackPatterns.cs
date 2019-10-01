using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPatterns : MonoBehaviour
{
  private GameData _gameData;
  private int layerMask = 9;

  private Color _spaceSelectedColor = Color.green;

  private Color _spaceDeSelectedColor = Color.white;

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

    RaycastHit temp;

    List<RaycastHit> sortedHits = new List<RaycastHit>();

    List<RaycastHit> attackableSpaces = new List<RaycastHit>();

    bool isEnemyHitPresent = false;


    // sort the array from least distance to greatest distance away from the ray
    for (int i = 0; i < rayResults.Length; i++)
    {
      if (rayResults[i].transform.CompareTag("Enemy"))
      {
        isEnemyHitPresent = true;

      }
      for (int j = i; j > 0; j--)
      {
        if (rayResults[j].distance < rayResults[j - 1].distance)
        {
          temp = rayResults[j];
          rayResults[j] = rayResults[j - 1];
          rayResults[j - 1] = temp;
        }
      }
    }

    // if at least one enemy hit is in the array, filter out the hits(the spaces) that have greater distance than the enemy hit, else when there isn't an enemy hit add everything in the array to the list
    if (isEnemyHitPresent == true)
    {

      float enemyDistance = 0.0f;

      for (int i = 0; i < rayResults.Length; i++)
      {
        if (rayResults[i].transform.CompareTag("Enemy"))
        {

          enemyDistance = rayResults[i].distance;
          // go through list to filter out all hits with greater distance than enemy's
          foreach (RaycastHit r in rayResults)
          {
            if (r.distance < enemyDistance)
            {
              attackableSpaces.Add(r);
            }
            else if (r.distance < enemyDistance + spaceSize / 4.0f && !r.transform.CompareTag("Enemy"))
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
      foreach (RaycastHit r in rayResults)
      {
        attackableSpaces.Add(r);
      }
    }


    ShowAtkPattern(attackableSpaces);


    return attackableSpaces;

  }

  // Changes the color of everything that was hit by the RaycastAll() in CalculateMovementPattern() to a "selected" color
  void ShowAtkPattern(List<RaycastHit> hitsArray)
  {
    for (int i = 0; i < hitsArray.Count; i++)
    {
      // get the info from the current hit
      RaycastHit currentHit = hitsArray[i];

      Renderer parentRenderer;

      try
      {
        parentRenderer = currentHit.transform.parent.GetComponent<Renderer>();

        SpaceColorSwitcher(parentRenderer, _spaceSelectedColor);
      }
      catch (System.Exception)
      {
        // throw;
      }
    }
  }



  public void HideAtkPattern(List<RaycastHit> hitsArray)
  {
    Renderer parentRenderer;
    try
    {
      for (int i = 0; i < hitsArray.Count; i++)
      {
        // get the info from the current hit
        RaycastHit currentHit = hitsArray[i];

        // do not try to get renderer if we hit an enemy otherwise the catch() will activate and skip the rest of the RaycastHits
        if (!currentHit.transform.CompareTag("Enemy"))
        {
          parentRenderer = currentHit.transform.parent.GetComponent<Renderer>();


          SpaceColorSwitcher(parentRenderer, _spaceDeSelectedColor);
        }
      }
    }
    catch (System.Exception)
    {
      // throw;
    }
  }


  // changes color of space by taking in it's Renderer and a Color
  public void SpaceColorSwitcher(Renderer spaceRenderer, Color color)
  {
    MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();

    // Get the current value of the material properties in the renderer (which should be the Material we gave a custom shader to that we assigned (to the parent in this case) in Unity)
    // Note: When we made the custom shader, we had to open it here and edit the "_Color" key to have "[PerRendererData]" in front of it (see GreenShader_shade in the Materials folder) Then we created a Material and assigned it this shader in the "Shader" field (I had to search for it by name). Then we took that Material and assigned it to the cube we want to change color (The parent of the object that this script is attached to, Ex. Normal_Cube)
    spaceRenderer.GetPropertyBlock(_propBlock);
    // Assign our new value.
    _propBlock.SetColor("_Color", color);
    // Apply the edited values to the renderer.
    spaceRenderer.SetPropertyBlock(_propBlock);
  }


} // End of AttackPatterns class
