using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPatterns : MonoBehaviour {
  private GameData _gameData;

  //
  //
  //
  private Renderer _renderer;
  private Color _spaceSelectedColor = Color.blue;
  private Color _spaceDeSelectedColor = Color.white;

  // Start is called before the first frame update
  void Start () {
    _gameData = GameObject.Find ("Game_Data").GetComponent<GameData> ();
    if (_gameData == null) {
      Debug.LogError ("Game Data object is NULL");

    }

  }

  // Update is called once per frame
  void Update () {

  }

  // fires a ray and returns what it hits in a RayCastHit array. All spaces that are hit also change color
  public RaycastHit[] CalculateMovementPattern (Vector3 corePosition, float coreSpaceMoveDistance, float spaceSize) {
    // Debug.Log ("b");

    float rayDistance = coreSpaceMoveDistance * spaceSize;

    Vector3 rayDirection = transform.TransformDirection (Vector3.forward);

    Ray ray = new Ray (corePosition, rayDirection);

    RaycastHit[] hits;

    // RayCastAll() will let you get info from each collider the ray passes through (each one the ray hits)
    hits = Physics.RaycastAll (ray.origin, ray.direction, rayDistance);

    // Draw the ray (only when it is selected) so you can see where it's hitting in Unity
    Debug.DrawRay (corePosition, rayDirection * rayDistance, Color.red);

    ShowMovementPattern (hits);

    // return the hits you got from where the ray was fired (from the core's (that accesses this script) position)
    return hits;
  }

  // Changes the color of everything that was hit by the RaycastAll() in CalculateMovementPattern() to a "selected" color
  void ShowMovementPattern (RaycastHit[] allHits) {

    for (int i = 0; i < allHits.Length; i++) {
      // get the info from the current hit
      RaycastHit currentHit = allHits[i];

      // Debug.Log("X is : " + currentHit.transform.position.x + " Z is: " + currentHit.transform.position.z);

      _renderer = currentHit.transform.parent.GetComponent<Renderer> ();

      SpaceColorSwitcher (_renderer, _spaceSelectedColor);

    }

  }

  // Changes the color of everything that was hit by the RaycastAll() in CalculateMovementPattern() to a "de-selected" color
  public void HideMovementPattern (RaycastHit[] allHits) {
    for (int i = 0; i < allHits.Length; i++) {
      RaycastHit currentHit = allHits[i];

      _renderer = currentHit.transform.parent.GetComponent<Renderer> ();

      SpaceColorSwitcher (_renderer, _spaceDeSelectedColor);

    }

  }

  // changes color of space by taking in it's Renderer and a Color
  void SpaceColorSwitcher (Renderer spaceRenderer, Color color) {
    MaterialPropertyBlock _propBlock = new MaterialPropertyBlock ();

    // Get the current value of the material properties in the renderer (which should be the Material we gave a custom shader to that we assigned (to the parent in this case) in Unity)
    // Note: When we made the custom shader, we had to open it here and edit the "_Color" key to have "[PerRendererData]" in front of it (see GreenShader_shade in the Materials folder) Then we created a Material and assigned it this shader in the "Shader" field (I had to search for it by name). Then we took that Material and assigned it to the cube we want to change color (The parent of the object that this script is attached to, Ex. Normal_Cube)
    spaceRenderer.GetPropertyBlock (_propBlock);
    // Assign our new value.
    _propBlock.SetColor ("_Color", color);
    // Apply the edited values to the renderer.
    spaceRenderer.SetPropertyBlock (_propBlock);

  }

}