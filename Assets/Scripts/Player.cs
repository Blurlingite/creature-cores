using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{

  // eventually we will get this ID from the Game_Data object so we know which player this is. We will assign Player 1 a color and when we try to select a core the core will check if the player's ID matches the color on the core. (Ex. Player 1 can move blue cores, Player 2 can move green cores, etc.) If it does, that player can move the core but if it doesn't then they can't
  private int _playerID = 1;

  private int _layerMask = 9;

  private Vector3 _oldPosition;
  private Vector3 _currentPosition;
  private bool _isLocationNew = false;


  private Creature _currentlySelectedCreature;

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
    _currentPosition = transform.position;
    _oldPosition = _currentPosition;
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

    _currentPosition = transform.position;

    if (_currentPosition != _oldPosition)
    {
      _isLocationNew = true;
      // Debug.Log("NOOOO");
      // Debug.Log(_isLocationNew);

    }

  }

  public Creature getCurrentlySelectedCreature()
  {
    return _currentlySelectedCreature;
  }

  public void setCurrentlySelectedCreature(Creature creature)
  {
    _currentlySelectedCreature = creature;
  }

  public int getlayerMask()
  {
    return _layerMask;
  }

  public bool getIsLocationNew()
  {
    return _isLocationNew;
  }


} // End of Player class