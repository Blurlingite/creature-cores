using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCore : MonoBehaviour
{

  private Board _board;
  private GameData _gameData;
  private Ray ray;
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

    _gameData = GameObject.Find("Game_Data").GetComponent<GameData>();
    if (_gameData == null)
    {
      Debug.LogError("Game Data object is NULL");

    }
  }

  // Update is called once per frame
  void Update()
  {

    if (_spaceSize <= 0.0f)
    {
      Debug.LogError("Cannot get space size, Ray will not be casted");
    }

    if (transform.position.y >= 3)
    {
      _isSelected = true;

    }
    else
    {
      _isSelected = false;
    }


    CalculateMovementPattern();


  }


  void CalculateMovementPattern()
  {
    Debug.Log("b");

    float rayDistance = _moveDistance * _spaceSize;

    Vector3 rayDirection = transform.TransformDirection(Vector3.forward);

    ray = new Ray(transform.position, rayDirection);

    RaycastHit[] hits;

    // RayCastAll() will let you get info from each collider the ray passes through (each one the ray hits)
    hits = Physics.RaycastAll(ray.origin, ray.direction, rayDistance);

    if (_isSelected == true)
    {
      // Draw the ray (only when it is selected) so you can see where it's hitting in Unity
      Debug.DrawRay(transform.position, rayDirection * rayDistance, Color.red);

    }

    for (int i = 0; i < hits.Length; i++)
    {
      // get the info from the current hit
      RaycastHit currentHit = hits[i];


      ToggleMovementPattern(currentHit);

      Debug.Log("X is : " + currentHit.transform.position.x + " Z is: " + currentHit.transform.position.z);


    }
  }

  void ToggleMovementPattern(RaycastHit hit)
  {
    // get the parent cube, then get the Renderer so you can change it's color to show that the cube is in the movement path
    Transform parentCube = hit.transform.parent;

    Renderer colorOfCube = parentCube.GetComponent<Renderer>();

    if (_isSelected == true)
    {
      colorOfCube.material = _gameData.getMovePatternColor();
    }
    else
    {
      colorOfCube.material = _gameData.getHideMovePatternColor();
    }

  }






}
