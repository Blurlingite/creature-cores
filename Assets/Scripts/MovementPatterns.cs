using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPatterns : MonoBehaviour
{
  private GameData _gameData;

  // Start is called before the first frame update
  void Start()
  {
    _gameData = GameObject.Find("Game_Data").GetComponent<GameData>();
    if (_gameData == null)
    {
      Debug.LogError("Game Data object is NULL");

    }
  }

  // Update is called once per frame
  void Update()
  {

  }


  // fires a ray and returns what it hits in a RayCastHit array. All spaces that are hit also change color
  public RaycastHit[] CalculateMovementPattern(Vector3 corePosition, float coreSpaceMoveDistance, float spaceSize)
  {
    Debug.Log("b");

    float rayDistance = coreSpaceMoveDistance * spaceSize;

    Vector3 rayDirection = transform.TransformDirection(Vector3.forward);


    Ray ray = new Ray(corePosition, rayDirection);

    RaycastHit[] hits;

    // RayCastAll() will let you get info from each collider the ray passes through (each one the ray hits)
    hits = Physics.RaycastAll(ray.origin, ray.direction, rayDistance);


    // Draw the ray (only when it is selected) so you can see where it's hitting in Unity
    Debug.DrawRay(corePosition, rayDirection * rayDistance, Color.red);

    ShowMovementPattern(hits);


    // return the hits you got from where the ray was fired (from the core's (that accesses this script) position)
    return hits;
  }

  // Changes the color of everything that was hit by the RaycastAll() in CalculateMovementPattern() to a "selected" color
  void ShowMovementPattern(RaycastHit[] allHits)
  {

    for (int i = 0; i < allHits.Length; i++)
    {
      // get the info from the current hit
      RaycastHit currentHit = allHits[i];

      Debug.Log("X is : " + currentHit.transform.position.x + " Z is: " + currentHit.transform.position.z);

      // get the parent cube, then get the Renderer so you can change it's color to show that the cube is in the movement path
      Transform parentCube = currentHit.transform.parent;

      Renderer colorOfCube = parentCube.GetComponent<Renderer>();

      colorOfCube.material = _gameData.getMovePatternColor();

    }

  }

  // Changes the color of everything that was hit by the RaycastAll() in CalculateMovementPattern() to a "de-selected" color
  public void HideMovementPattern(RaycastHit[] allHits)
  {
    for (int i = 0; i < allHits.Length; i++)
    {
      RaycastHit currentHit = allHits[i];

      Transform parentCube = currentHit.transform.parent;

      Renderer colorOfCube = parentCube.GetComponent<Renderer>();

      colorOfCube.material = _gameData.getHideMovePatternColor();

    }


  }






}
