using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

  private float _spaceSize = 4.0f;
  private string _boardSize = "16x16";
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  // void Update()
  // {

  // }

  // changes all spaces on board to white
  public void ClearBoardColor()
  {
    Renderer[] allSpaceRenderers = GetComponentsInChildren<Renderer>();

    foreach (Renderer r in allSpaceRenderers)
    {
      if (r.gameObject.CompareTag("Space"))
      {

        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();

        r.GetPropertyBlock(_propBlock);
        // Assign our new value.
        _propBlock.SetColor("_Color", Color.white);
        // Apply the edited values to the renderer.
        r.SetPropertyBlock(_propBlock);
      }

    }
  }

  // deactivates the Inner_Cube children of all Spaces by switching the Space's bool to false
  public void DeactivateSpaces()
  {
    Transform[] children;

    if (GetComponentsInChildren<Transform>() != null)
    {
      children = GetComponentsInChildren<Transform>();

      foreach (Transform t in children)
      {

        if (t.transform.CompareTag("Space"))
        {
          Space s = t.transform.GetComponent<Space>();
          s.setIsActiveMove(false);
        }
      }

    }
  }

  public float getSpaceSize()
  {
    return _spaceSize;
  }
}
