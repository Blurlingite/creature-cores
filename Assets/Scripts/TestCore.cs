using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCore : MonoBehaviour
{

  private Board _board;
  [SerializeField]
  // num of spaces to move * each space's size
  private float _moveDistance = 3.0f;
  // size of each space on the board (x or y value)
  private float _spaceSize;
  private bool _isSelected = false;
  // Start is called before the first frame update
  void Start()
  {
    _board = GameObject.Find("Board").GetComponent<Board>();

    // get the size of each space from the Board component
    if (_board != null)
    {
      _spaceSize = _board.getSpaceSize();
    }
  }

  // Update is called once per frame
  void Update()
  {

    if (transform.position.y >= 3)
    {
      _isSelected = true;
    }

    if (_isSelected == true)
    {

      // Debug.Log("b");

      // RaycastHit hitInfo;
      // // cast a ray using this core's move distance
      // if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward * _moveDistance) * 4, out hitInfo))
      // {
      //   Debug.Log("yes");
      //   Debug.Log(hitInfo.transform.position.z);

      //   Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward * _moveDistance) * 4, Color.red);

      // }
      // else
      // {
      //   Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward * _moveDistance) * 4, Color.red);

      // }

      Debug.Log("b");

      RaycastHit[] hits;

      hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), _moveDistance * _spaceSize);





      for (int i = 0; i < hits.Length; i++)
      {
        RaycastHit currentHit = hits[i];

        Debug.Log(currentHit.transform.position.z);

      }

    }

  }




}
