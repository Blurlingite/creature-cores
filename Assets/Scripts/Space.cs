using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
  // the child object index of the child object Inner_Cube
  private int _innerCubeIndex = 1;

  private GameObject _innerCube;
  // bool to know when this space is active in a movement pattern
  [SerializeField]
  private bool _isActiveMove = false;
  // Start is called before the first frame update
  void Start()
  {
    _innerCube = transform.GetChild(_innerCubeIndex).gameObject;
  }

  // Update is called once per frame
  void Update()
  {

    if (_isActiveMove == true)
    {
      _innerCube.SetActive(true);
      _innerCube.GetComponent<Renderer>().material.color = Color.green;
    }
    else
    {
      _innerCube.SetActive(false);
    }

  }


  public void setIsActiveMove(bool isThisActive)
  {
    _isActiveMove = isThisActive;
  }
}
