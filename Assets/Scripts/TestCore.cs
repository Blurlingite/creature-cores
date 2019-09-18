using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCore : MonoBehaviour
{
  [SerializeField]
  // num of spaces to move * each space's size
  private float _moveDistance = 3.0f;
  private bool _isSelected = false;
  // Start is called before the first frame update
  void Start()
  {

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

      Debug.Log("b");

      RaycastHit hitInfo;
      // cast a ray using this core's move distance
      if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward * _moveDistance) * 4, out hitInfo))
      {
        Debug.Log("yes");
        Debug.Log(hitInfo.transform.position.z);

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward * _moveDistance) * 4, Color.red);

      }
      else
      {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward * _moveDistance) * 4, Color.red);

      }

    }

  }




}
