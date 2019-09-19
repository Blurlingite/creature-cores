using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCube : MonoBehaviour
{

  private GameObject _outerCubeParent;
  [SerializeField]
  private Material _selectedMaterial;

  [SerializeField]
  private Material _deSelectedMaterial;

  private bool _isStillColliding = false;

  // Start is called before the first frame update
  void Start()
  {

    _outerCubeParent = this.transform.parent.gameObject;
    if (_outerCubeParent == null)
    {
      Debug.LogError("Parent block is NULL");

    }


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
      // change top of cube to green

      Renderer colorOfCube = _outerCubeParent.GetComponent<Renderer>();

      if (colorOfCube != null)
      {
        colorOfCube.material = _selectedMaterial;

      }


    }


  }

  void OnTriggerExit(Collider other)
  {

    // if the other object was the Player
    if (other.CompareTag("Player"))
    {
      Renderer colorOfCube = _outerCubeParent.GetComponent<Renderer>();

      if (colorOfCube != null)
      {
        colorOfCube.material = _deSelectedMaterial;

      }
    }
  }
}
