using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script should not be communicating with any other "Core" script so that we can keep common behaviors of Cores (like knowing when to move) here and not have to copy & paste that code into each "Core" script
public class Core : MonoBehaviour
{
  private Board _board;
  private GameData _gameData; // get move and attack patterns
  private MovementPatterns _movementPatterns;
  private bool _stopFalling = false;
  private bool _isSelected, _isPlacedBackDown = false;
  private float _coreRiseHeight = 3.0f;
  // size of each space on the board (x or y value)
  private float _spaceSize;
  private float _moveDistance;
  // everywhere this core can move
  private RaycastHit[] _allMovementHits;


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

    _movementPatterns = _gameData.GetComponent<MovementPatterns>();

    if (_movementPatterns == null)
    {
      Debug.LogError("Movement Patterns script is NULL and should be on Game Data object");

    }

  }

  // Update is called once per frame
  void Update()
  {

    if (_stopFalling == false)
    {
      transform.Translate(Vector3.down * Time.deltaTime);

    }



    if (_spaceSize <= 0.0f)
    {
      Debug.LogError("Cannot get space size, Ray will not be casted");
    }

    // We know if the core is selected if it is not on the ground (if the y value is above a certain number)
    if (transform.position.y >= 3)
    {
      _isSelected = true;

    }
    else
    {
      _isSelected = false;
    }



    if (_isSelected == true)
    {
      CoreIndivMovementPattern();

      // make this false so we know the core is not down right now
      _isPlacedBackDown = false;

    }
    else if (_isSelected == false && _isPlacedBackDown == false && _allMovementHits != null)
    {

      // use Game Data's method to turn each space back to normal color
      _movementPatterns.HideMovementPattern(_allMovementHits);

      // don't need to set RayCast array to null since it gets reassigned whenever the core is selected, instead use _isPlacedBackDown to know that the core was placed back down
      _isPlacedBackDown = true;

    }


  }

  void OnTriggerStay(Collider other)
  {

    // If player, move this core upwards , then cast 4 rays equal to how far this core can move (ex. 3 spaces forward, backward, left, right)
    if (other.CompareTag("Player") && (Input.GetKeyDown(KeyCode.Space)))
    {

      // Debug.Log("up");
      transform.position = new Vector3(transform.position.x, _coreRiseHeight, transform.position.z);

    }

    // If the B key is pressed, put the core back down where it was
    if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.B))
    {

      // Debug.Log("BBBBBB");

      transform.position = new Vector3(transform.position.x, (_coreRiseHeight - 1.96f), transform.position.z);

    }

  }

  void OnTriggerEnter(Collider other)
  {

    if (other.CompareTag("Board"))
    {
      _stopFalling = true;

    }

  }

  void CoreIndivMovementPattern()
  {
    Vector3 thisCoresPosition = transform.position;

    _allMovementHits = _movementPatterns.CalculateMovementPattern(thisCoresPosition, _moveDistance, _spaceSize);
  }


  public void setMoveDistance(float NumOfSpacesToMove)
  {
    _moveDistance = NumOfSpacesToMove;
  }


}