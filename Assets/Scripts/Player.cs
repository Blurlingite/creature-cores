using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    CalculateMovement();

  }

  // void x()
  // {
  //   float hor = Input.GetAxis("Horizontal");
  //   float vert = Input.GetAxis("Vertical");

  //   Vector3 dir = new Vector3(hor, 0, vert);
  //   transform.Translate(dir * 3.0f * Time.deltaTime);

  // }

  void CalculateMovement()
  {
    // will store info on the cube's stats
    RaycastHit hitInfo;

    // upward movement

    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, Mathf.Infinity) == true && Input.GetKeyDown(KeyCode.UpArrow) == true)
    {
      transform.position = hitInfo.transform.position;
    }


    // downward movement

    else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hitInfo, Mathf.Infinity) == true && Input.GetKeyDown(KeyCode.DownArrow) == true)
    {
      transform.position = hitInfo.transform.position;
    }


    // left movement

    else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hitInfo, Mathf.Infinity) == true && Input.GetKeyDown(KeyCode.LeftArrow) == true)
    {
      transform.position = hitInfo.transform.position;
    }


    // right movement

    else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitInfo, Mathf.Infinity) == true && Input.GetKeyDown(KeyCode.RightArrow) == true)
    {
      transform.position = hitInfo.transform.position;
    }


  }
}
