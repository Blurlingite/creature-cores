using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Creature : MonoBehaviour
{
  private Board _board;
  private GameData _gameData; // get move and attack patterns
  private MovementPatterns _movementPatterns;
  // The player of this creature
  private string _whoseTurnIsIt = "Player_1";
  private bool _stopFalling = false;
  private bool _isSelected, _isDown = true;
  private float _creatureRiseHeight = 3.0f;
  // The Y position the creature needs to be to be on the ground
  private float _creatureGroundY = 1.04f;
  // size of each space on the board (x or y value)
  private float _spaceSize;

  // everywhere this creature can move
  private RaycastHit[] _forwardMovementHits;
  private float _forwardXOffset = 0.0f;
  private float _forwardZOffset = 1.0f;

  private RaycastHit[] _backwardMovementHits;
  private float _backwardZOffset = -1.0f;
  private float _backwardXOffset = 0.0f;


  private RaycastHit[] _leftMovementHits;
  private float _leftXOffset = -0.5f;
  private float _leftZOffset = 0.0f;

  private RaycastHit[] _rightMovementHits;
  private float _rightXOffset = 0.5f;
  private float _rightZOffset = 0.0f;



  // All variables that will be saved to the file go here:

  // number of spaces this creature can move
  private float _moveDistance = 3.0f;

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

    SaveLocally();

  }

  // Update is called once per frame
  void Update()
  {

    if (_spaceSize <= 0.0f)
    {
      Debug.LogError("Cannot get space size, Ray will not be casted");
    }

    // where the creature is currently and movement pattern if it can move
    CreaturePositioning();

  }

  void OnTriggerStay(Collider other)
  {
    // If player, move this creature upwards , then cast 4 rays equal to how far this creature can move (ex. 3 spaces forward, backward, left, right)
    if (other.CompareTag("Player_1") && (Input.GetKeyDown(KeyCode.Space)))
    {
      transform.position = new Vector3(transform.position.x, _creatureRiseHeight, transform.position.z);
    }

    // If the B key is pressed, put the creature back down where it was
    if (other.CompareTag("Player_1") && Input.GetKeyDown(KeyCode.B))
    {
      transform.position = new Vector3(transform.position.x, _creatureGroundY, transform.position.z);

      _isSelected = false;
      _isDown = true;
    }

  }

  void OnTriggerEnter(Collider other)
  {

    if (other.CompareTag("Board"))
    {
      _stopFalling = true;
    }

  }

  // Defines the creature's position currently and where it can move if possible
  void CreaturePositioning()
  {

    if (_stopFalling == false)
    {
      transform.Translate(Vector3.down * Time.deltaTime);
    }

    // We know if the creature is selected if it is not on the ground (if the y value is above a certain number)
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
      // make this false so we know the creature is not down right now (We turned this true in the CreatureIndivMovementPattern() right after this, so if _isDown is changed to false after that method call, the HideMovementPattern() won't be called b/c _isDown must be true to enter it's if statement)
      _isDown = false;
      CreatureIndivMovementPattern();

      // When I implement a turn system, we will search for the the current player by searching for the tag so we know who can press keys. We dont want Player 2 to press keys during Player 1's turn. We will use the tag to set _whoseTurnIsIt. Then we will exclude all other players except the one with the tag equal to _whoseTurnIsIt
      // When it is player 1's turn, at anytime the creature is in the air and regardless of the player's position, if they press the B key, the creature will be placed down
      if (_whoseTurnIsIt.Equals("Player_1") && Input.GetKeyDown(KeyCode.B))
      {
        transform.position = new Vector3(transform.position.x, _creatureGroundY, transform.position.z);

        _isSelected = false;
        _isDown = true;
      }



    }
    else if (_isSelected == false && _isDown == true && _forwardMovementHits != null && _backwardMovementHits != null && _leftMovementHits != null && _rightMovementHits != null)
    {

      // use Game Data's method to turn each space back to normal color
      _movementPatterns.HideMovementPattern(_forwardMovementHits);
      _movementPatterns.HideMovementPattern(_backwardMovementHits);

      _movementPatterns.HideMovementPattern(_leftMovementHits);

      _movementPatterns.HideMovementPattern(_rightMovementHits);



    }

    else if (_isSelected == false && _isDown == true && _forwardMovementHits == null && _backwardMovementHits == null && _leftMovementHits == null && _rightMovementHits == null)
    {
      // Do nothing since this is called when the game just starts and all the RaycastHit arrays are null
    }
  }

  // calculates which spaces the creature can move and highlights them
  void CreatureIndivMovementPattern()
  {
    Vector3 thisCreaturesPosition = transform.position;

    AllMovementPatterns(thisCreaturesPosition);
  }


  public void setMoveDistance(float NumOfSpacesToMove)
  {
    _moveDistance = NumOfSpacesToMove;
  }

  public float getMoveDistance()
  {
    return _moveDistance;
  }


  void SaveLocally()
  {
    CreatureToSerialize cts = new CreatureToSerialize(_moveDistance);

    _gameData.AddToCreatureDictionary(cts);
  }



  void MovementPattern(RaycastHit[] hitPoints, float xOffset, float zOffset)
  {
    // we get to this method when the creature is in the air, so now we need to check if we pressed the Space key and if we did, compare the player's position with the positions in the movement pattern. If you get a match, move the creature there
    if (Input.GetKeyDown(KeyCode.Space))
    {
      Player p1 = GameObject.Find("Player_1").GetComponent<Player>();
      float playerPositionX = p1.transform.position.x;
      float playerPositionZ = p1.transform.position.z;

      for (int i = 0; i < hitPoints.Length; i++)
      {
        RaycastHit currentHit = hitPoints[i];

        float currentHitX = currentHit.point.x + xOffset;

        // we Round the only the Z float and then add 1 to fix the ray's calculating error. This only affects the Z and NOT the X
        float currentHitZ = Mathf.Round(currentHit.point.z) + zOffset;

        if (playerPositionX == currentHitX && playerPositionZ == currentHitZ)
        {
          Vector3 newLocation = new Vector3(currentHitX, _creatureGroundY, currentHitZ);
          transform.position = newLocation;

          _isSelected = false;
          _isDown = true;



          break;  // to avoid casting another movement pattern
        }
      }
    }
  }

  // Calculates the creature's entire movement pattern by passing in it's position
  void AllMovementPatterns(Vector3 creaturePosition)
  {
    // forward ray results
    _forwardMovementHits = _movementPatterns.ForwardMovementPattern(creaturePosition, _moveDistance, _spaceSize);

    MovementPattern(_forwardMovementHits, _forwardXOffset, _forwardZOffset);

    // backward ray results
    _backwardMovementHits = _movementPatterns.BackwardMovementPattern(creaturePosition, _moveDistance, _spaceSize);

    MovementPattern(_backwardMovementHits, _backwardXOffset, _backwardZOffset);

    // left ray results
    _leftMovementHits = _movementPatterns.LeftMovementPattern(creaturePosition, _moveDistance, _spaceSize);

    MovementPattern(_leftMovementHits, _leftXOffset, _leftZOffset);

    // right ray results
    _rightMovementHits = _movementPatterns.RightMovementPattern(creaturePosition, _moveDistance, _spaceSize);

    MovementPattern(_rightMovementHits, _rightXOffset, _rightZOffset);
  }


} // End of Creature class

public class CreatureToSerialize
{
  public float moveDistance;

  public CreatureToSerialize(float moveDistance)
  {
    this.moveDistance = moveDistance;
  }


}