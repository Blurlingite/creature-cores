using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCube : MonoBehaviour
{

  private bool _isStillColliding = false;

  private Color _spaceSelectedColor = Color.green;
  private Color _spaceDeSelectedColor = Color.white;

  public float Speed = 1, Offset;

  private Renderer _renderer;
  private MaterialPropertyBlock _propBlock;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  // We need to know that the player is still on the space when it selects a core so we need OnTriggerStay and NOT OnTriggerEnter
  void OnTriggerStay(Collider other)
  {

    if (other.CompareTag("Player"))
    {

      _isStillColliding = true;

      _propBlock = new MaterialPropertyBlock();

      _renderer = transform.parent.GetComponent<Renderer>();

      // change top of cube to green

      // Get the current value of the material properties in the renderer (which should be the Material we gave a custom shader to that we assigned (to the parent in this case) in Unity)
      // Note: When we made the custom shader, we had to open it here and edit the "_Color" key to have "[PerRendererData]" in front of it (see GreenShader_shade in the Materials folder) Then we created a Material and assigned it this shader in the "Shader" field (I had to search for it by name). Then we took that Material and assigned it to the cube we want to change color (The parent of the object that this script is attached to, Ex. Normal_Cube)
      _renderer.GetPropertyBlock(_propBlock);

      // Assign our new value.
      _propBlock.SetColor("_Color", _spaceSelectedColor);
      // Apply the edited values to the renderer.
      _renderer.SetPropertyBlock(_propBlock);

    }

  }

  void OnTriggerExit(Collider other)
  {

    // if the other object was the Player
    if (other.CompareTag("Player"))
    {

      // Get the current value of the material properties in the renderer (which should be the Material we gave a custom shader to that we assigned (to the parent in this case) in Unity)
      // Note: When we made the custom shader, we had to open it here and edit the "_Color" key to have "[PerRendererData]" in front of it (see GreenShader_shade in the Materials folder) Then we created a Material and assigned it this shader in the "Shader" field (I had to search for it by name). Then we took that Material and assigned it to the cube we want to change color (The parent of the object that this script is attached to, Ex. Normal_Cube)
      _renderer.GetPropertyBlock(_propBlock);
      // Assign our new value (the de-select color).
      _propBlock.SetColor("_Color", _spaceDeSelectedColor);
      // Apply the edited values to the renderer.
      _renderer.SetPropertyBlock(_propBlock);

    }
  }
}