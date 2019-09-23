using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPatterns : MonoBehaviour
{
  private GameData _gameData;
  private Renderer _renderer;
  private int layerMask = 9;
  private Color _spaceSelectedColor = new Color(0.2F, 0.3F, 0.4F);
  private Color _spaceDeSelectedColor = Color.white;

  // private RaycastHit[] _forwardAtkHits;
  // private RaycastHit[] _backwardAtkHits;
  // private RaycastHit[] _leftAtkHits;
  // private RaycastHit[] _rightAtkHits;
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
  // public RaycastHit[] ForwardAttackPattern(Vector3 atkSeekerPosition, float creatureSpaceAtkDistance, float spaceSize)
  // {

  //   float rayDistance = creatureSpaceAtkDistance * spaceSize;

  //   Vector3 rayDirection = transform.TransformDirection(Vector3.forward);

  //   Ray ray = new Ray(atkSeekerPosition, rayDirection);

  //   // RayCastAll() will let you get info from each collider the ray passes through (each one the ray hits)
  //   _forwardAtkHits = Physics.RaycastAll(ray.origin, ray.direction, rayDistance, layerMask);

  //   // Draw the ray (only when it is selected) so you can see where it's hitting in Unity
  //   Debug.DrawRay(atkSeekerPosition, rayDirection * rayDistance, Color.red);

  //   ShowAtkPattern(_forwardAtkHits);

  //   // return the hits you got from where the ray was fired (from the creature's (that accesses this script) position)
  //   return _forwardAtkHits;

  // }

  public RaycastHit[] StraightAttackSeekerRay(Vector3 position, Vector3 direction, float maxDistance, float spaceSize, int layerMask, List<Vector3> attackHits)
  {
    Vector3 pos = position;

    Vector3 rayDirection = transform.TransformDirection(direction);

    float rayDistance = maxDistance * spaceSize;


    RaycastHit[] hits = Physics.RaycastAll(pos, rayDirection, rayDistance, layerMask);

    Debug.DrawRay(pos, rayDirection * rayDistance, Color.red);

    ShowAtkPattern(hits);

    for (int i = 0; i < hits.Length; i++)
    {
      // add to list so we can compare each coordinates with player position
      attackHits.Add(hits[i].point);
    }



    return hits;

  }


  // Changes the color of everything that was hit by the RaycastAll() in CalculateMovementPattern() to a "selected" color
  void ShowAtkPattern(RaycastHit[] hitsArray)
  {

    for (int i = 0; i < hitsArray.Length; i++)
    {
      // get the info from the current hit
      RaycastHit currentHit = hitsArray[i];

      _renderer = currentHit.transform.parent.GetComponent<Renderer>();

      SpaceColorSwitcher(_renderer, _spaceSelectedColor);
    }

  }

  public void HideAtkPattern(RaycastHit[] hitsArray)
  {

    for (int i = 0; i < hitsArray.Length; i++)
    {
      // get the info from the current hit
      RaycastHit currentHit = hitsArray[i];

      _renderer = currentHit.transform.parent.GetComponent<Renderer>();

      SpaceColorSwitcher(_renderer, _spaceDeSelectedColor);
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
