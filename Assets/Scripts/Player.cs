using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{

  // eventually we will get this ID from the Game_Data object so we know which player this is. We will assign Player 1 a color and when we try to select a core the core will check if the player's ID matches the color on the core. (Ex. Player 1 can move blue cores, Player 2 can move green cores, etc.) If it does, that player can move the core but if it doesn't then they can't
  private int _playerID = 1;


  // Start is called before the first frame update
  void Start()
  {


  }

  // Update is called once per frame
  void Update()
  {
    CalculateMovement();

  }

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