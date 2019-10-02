using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementPatterns : MonoBehaviour
{
  private GameData _gameData;
  private Color _spaceSelectedColor = Color.blue;
  private Color _spaceDeSelectedColor = Color.white;

  // the child object index of the child object Inner_Cube
  private int _innerCubeIndex = 1;

  // used to ignore colliders on a certain layer (layer 9 in this case b/c we set it to 9) when casting rays so I don't get unintended hits in the hit arrays and then errors. 
  // The Player is on layer 9 b/c it's collider was getting in the way
  private int layerMask = 9;

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

  // fires a ray (straight line) and returns what it hits in a RayCastHit array. All spaces that are hit also change color
  public List<RaycastHit> StraightMovementPattern(Vector3 creaturePosition, Vector3 direction, float creatureSpaceMoveDistance, float spaceSize)
  {
    float rayDistance = creatureSpaceMoveDistance * spaceSize;

    Vector3 rayDirection = transform.TransformDirection(direction);

    Ray ray = new Ray(creaturePosition, rayDirection);

    // RayCastAll() will let you get info from each collider the ray passes through (each one the ray hits)
    RaycastHit[] rayResults = Physics.RaycastAll(ray.origin, ray.direction, rayDistance, layerMask);

    RaycastHit temp;

    List<RaycastHit> sortedHits = new List<RaycastHit>();

    List<RaycastHit> moveableSpaces = new List<RaycastHit>();


    // sort the array from least distance to greatest distance away from the ray
    for (int i = 0; i < rayResults.Length; i++)
    {
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

    for (int i = 0; i < rayResults.Length; i++)
    {
      if (!rayResults[i].transform.CompareTag("Enemy"))
      {
        moveableSpaces.Add(rayResults[i]);
      }
      else if (rayResults[i].transform.CompareTag("Enemy"))
      {
        break;
      }
    }

    ShowMovementPattern(moveableSpaces);

    // return the hits you got from where the ray was fired (from the creature's (that accesses this script) position)
    return moveableSpaces;
  }



  // fires a ray forward (right diagonal line) and returns what it hits in a RayCastHit array. All spaces that are hit also change color
  public List<RaycastHit> DiagonalMovementPattern(Vector3 creaturePosition, Vector3 direction, float creatureSpaceMoveDistance, float spaceSize)
  {

    // Shooting diagonally, we need extra distance equal to half the size of 1 space, which is 2.0f in this case since a space is 4x4
    _diagonalOffset = spaceSize / 2;

    spaceSize += _diagonalOffset;

    float rayDistance = creatureSpaceMoveDistance * spaceSize;

    Vector3 rayDirection = transform.TransformDirection(direction);

    Ray ray = new Ray(creaturePosition, rayDirection);

    // RayCastAll() will let you get info from each collider the ray passes through (each one the ray hits)
    RaycastHit[] rayResults = Physics.RaycastAll(ray.origin, ray.direction, rayDistance, layerMask);


    RaycastHit temp;

    List<RaycastHit> sortedHits = new List<RaycastHit>();

    List<RaycastHit> moveableSpaces = new List<RaycastHit>();


    // sort the array from least distance to greatest distance away from the ray
    for (int i = 0; i < rayResults.Length; i++)
    {
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

    for (int i = 0; i < rayResults.Length; i++)
    {
      if (!rayResults[i].transform.CompareTag("Enemy"))
      {
        moveableSpaces.Add(rayResults[i]);
      }
      else if (rayResults[i].transform.CompareTag("Enemy"))
      {
        break;
      }
    }

    ShowMovementPattern(moveableSpaces);

    // return the hits you got from where the ray was fired (from the creature's (that accesses this script) position)
    return moveableSpaces;
  }



  // Changes the color of everything that was hit by the RaycastAll() in CalculateMovementPattern() to a "selected" color
  public void ShowMovementPattern(List<RaycastHit> hitsArray)
  {
    for (int i = 0; i < hitsArray.Count; i++)
    {
      // get the info from the current hit
      RaycastHit currentHit = hitsArray[i];

      // Ignore Enemy hits in the array you can't get their parent's renderer b/c they dont have a parent object
      if (!currentHit.transform.CompareTag("Enemy"))
      {
        // _renderer = currentHit.transform.parent.GetComponent<Renderer>();

        Transform parentSpace = currentHit.transform.parent;

        // if (parentSpace.GetComponent<Space>() != null)
        // {
        //   Space space = parentSpace.GetComponent<Space>();

        //   space.setIsActiveMove(true);

        // }

        SpaceColorSwitcher(parentSpace, _spaceSelectedColor);
      }
    }
  }


  // REMOVE REMOVE REMOVE REMOVE REMOVE REMOVE REMOVE 
  // Changes the color of everything that was hit by the RaycastAll() in CalculateMovementPattern() to a "selected" color
  // public void ShowMovementPattern(RaycastHit[] hitsArray)
  // {
  //   for (int i = 0; i < hitsArray.Length; i++)
  //   {
  //     // get the info from the current hit
  //     RaycastHit currentHit = hitsArray[i];

  //     // Ignore Enemy hits in the array you can't get their parent's renderer b/c they dont have a parent object
  //     if (!currentHit.transform.CompareTag("Enemy"))
  //     {

  //       Transform parentSpace = currentHit.transform.parent;

  //       Space space = parentSpace.GetComponent<Space>();

  //       space.setIsActiveMove(true);

  //       // _renderer = currentHit.transform.parent.GetComponent<Renderer>();

  //       SpaceColorSwitcher(parentSpace, _spaceSelectedColor);
  //     }
  //   }
  // }






  // Changes the color of everything that was hit by the RaycastAll() in CalculateMovementPattern() to a "de-selected" color
  public void HideMovementPattern(List<RaycastHit> hitsArray)
  {
    for (int i = 0; i < hitsArray.Count; i++)
    {
      RaycastHit currentHit = hitsArray[i];

      // Ignore Enemy hits in the array you can't get their parent's renderer b/c they dont have a parent object
      if (!currentHit.transform.CompareTag("Enemy"))
      {
        // _renderer = currentHit.transform.parent.GetComponent<Renderer>();

        Transform parentSpace = currentHit.transform.parent;


        SpaceColorSwitcher(parentSpace, _spaceDeSelectedColor);
      }
    }
  }

  // changes color of space by taking in it's Renderer and a Color
  void SpaceColorSwitcher(Transform parentSpace, Color color)
  {
    MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();


    Renderer outerSpace = parentSpace.GetComponent<Renderer>();


    // Get the current value of the material properties in the renderer (which should be the Material we gave a custom shader to that we assigned (to the parent in this case) in Unity)
    // Note: When we made the custom shader, we had to open it here and edit the "_Color" key to have "[PerRendererData]" in front of it (see GreenShader_shade in the Materials folder) Then we created a Material and assigned it this shader in the "Shader" field (I had to search for it by name). Then we took that Material and assigned it to the cube we want to change color (The parent of the object that this script is attached to, Ex. Normal_Cube)
    outerSpace.GetPropertyBlock(_propBlock);
    // Assign our new value.
    _propBlock.SetColor("_Color", color);
    // Apply the edited values to the renderer.
    outerSpace.SetPropertyBlock(_propBlock);

  }





} // End of MovementPatterns class