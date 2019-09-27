﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPatterns : MonoBehaviour
{
  private GameData _gameData;
  private Renderer _renderer;
  private Color _spaceSelectedColor = Color.blue;
  private Color _spaceDeSelectedColor = Color.white;

  // used to ignore colliders on a certain layer (layer 9 in this case b/c we set it to 9) when casting rays so I don't get unintended hits in the hit arrays and then errors. 
  // The Player is on layer 9 b/c it's collider was getting in the way
  private int layerMask = 9;
  private RaycastHit[] _forwardHits;
  private RaycastHit[] _backwardHits;
  private RaycastHit[] _leftHits;
  private RaycastHit[] _rightHits;

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

  // fires a ray forward and returns what it hits in a RayCastHit array. All spaces that are hit also change color
  public RaycastHit[] ForwardMovementPattern(Vector3 creaturePosition, float creatureSpaceMoveDistance, float spaceSize)
  {
    float rayDistance = creatureSpaceMoveDistance * spaceSize;

    Vector3 rayDirection = transform.TransformDirection(Vector3.forward);

    Ray ray = new Ray(creaturePosition, rayDirection);

    // RayCastAll() will let you get info from each collider the ray passes through (each one the ray hits)
    _forwardHits = Physics.RaycastAll(ray.origin, ray.direction, rayDistance, layerMask);

    // Draw the ray (only when it is selected) so you can see where it's hitting in Unity
    // Debug.DrawRay(creaturePosition, rayDirection * rayDistance, Color.red);

    ShowMovementPattern(_forwardHits);

    // return the hits you got from where the ray was fired (from the creature's (that accesses this script) position)
    return _forwardHits;
  }

  // fires a ray back and returns what it hits in a RayCastHit array. All spaces that are hit also change color
  public RaycastHit[] BackwardMovementPattern(Vector3 creaturePosition, float creatureSpaceMoveDistance, float spaceSize)
  {
    float rayDistance = creatureSpaceMoveDistance * spaceSize;

    Vector3 rayDirection = transform.TransformDirection(Vector3.back);

    Ray ray = new Ray(creaturePosition, rayDirection);

    // RayCastAll() will let you get info from each collider the ray passes through (each one the ray hits)
    _backwardHits = Physics.RaycastAll(ray.origin, ray.direction, rayDistance, layerMask);

    ShowMovementPattern(_backwardHits);

    // return the hits you got from where the ray was fired (from the creature's (that accesses this script) position)
    return _backwardHits;

  }

  // fires a ray left and returns what it hits in a RayCastHit array. All spaces that are hit also change color
  public RaycastHit[] LeftMovementPattern(Vector3 creaturePosition, float creatureSpaceMoveDistance, float spaceSize)
  {
    float rayDistance = creatureSpaceMoveDistance * spaceSize;

    Vector3 rayDirection = transform.TransformDirection(Vector3.left);

    Ray ray = new Ray(creaturePosition, rayDirection);

    // RayCastAll() will let you get info from each collider the ray passes through (each one the ray hits)
    _leftHits = Physics.RaycastAll(ray.origin, ray.direction, rayDistance, layerMask);

    ShowMovementPattern(_leftHits);

    // return the hits you got from where the ray was fired (from the creature's (that accesses this script) position)
    return _leftHits;
  }

  // fires a ray right and returns what it hits in a RayCastHit array. All spaces that are hit also change color
  public RaycastHit[] RightMovementPattern(Vector3 creaturePosition, float creatureSpaceMoveDistance, float spaceSize)
  {
    float rayDistance = creatureSpaceMoveDistance * spaceSize;

    Vector3 rayDirection = transform.TransformDirection(Vector3.right);

    Ray ray = new Ray(creaturePosition, rayDirection);

    // RayCastAll() will let you get info from each collider the ray passes through (each one the ray hits)
    _rightHits = Physics.RaycastAll(ray.origin, ray.direction, rayDistance, layerMask);

    ShowMovementPattern(_rightHits);

    // return the hits you got from where the ray was fired (from the creature's (that accesses this script) position)
    return _rightHits;

  }


  // Changes the color of everything that was hit by the RaycastAll() in CalculateMovementPattern() to a "selected" color
  public void ShowMovementPattern(RaycastHit[] hitsArray)
  {
    for (int i = 0; i < hitsArray.Length; i++)
    {
      // get the info from the current hit
      RaycastHit currentHit = hitsArray[i];

      // Ignore Enemy hits in the array you can't get their parent's renderer b/c they dont have a parent object
      if (!currentHit.transform.CompareTag("Enemy"))
      {
        _renderer = currentHit.transform.parent.GetComponent<Renderer>();

        SpaceColorSwitcher(_renderer, _spaceSelectedColor);
      }
    }
  }

  // Changes the color of everything that was hit by the RaycastAll() in CalculateMovementPattern() to a "de-selected" color
  public void HideMovementPattern(RaycastHit[] hitsArray)
  {
    for (int i = 0; i < hitsArray.Length; i++)
    {
      RaycastHit currentHit = hitsArray[i];

      // Ignore Enemy hits in the array you can't get their parent's renderer b/c they dont have a parent object
      if (!currentHit.transform.CompareTag("Enemy"))
      {
        _renderer = currentHit.transform.parent.GetComponent<Renderer>();

        SpaceColorSwitcher(_renderer, _spaceDeSelectedColor);
      }
    }
  }

  // changes color of space by taking in it's Renderer and a Color
  void SpaceColorSwitcher(Renderer spaceRenderer, Color color)
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



} // End of MovementPatterns class