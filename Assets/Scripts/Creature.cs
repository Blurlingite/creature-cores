using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{

  private Board _board;
  private GameData _gameData; // get move and attack patterns
  private MovementPatterns _movementPatterns;
  private AttackPatterns _attackPatterns;
  [SerializeField]
  private GameObject _attackSeekerPrefab;
  private Vector3 currentPosition;
  private Vector3 oldPosition;
  // The player of this creature
  private string _whoseTurnIsIt = "Player_1";
  // When implementing turns, reset all these bools to these default values, so Attack Seekers can respawn with each creature movement pattern
  private bool _stopFalling = false;
  private bool _isSelected, _isDown = false;
  private bool _summonAtkSeekers, _destroyAtkSeekers = false;
  // helps stops summoning attack seekers when they are actually being summoned. Which means this can stay true once changed to true for the whole game w/o causing errors
  private bool _stopSummoningAtkSeekers = false;
  [SerializeField]
  private bool _wasEnemyHitByRay = false;
  [SerializeField]
  private bool _isNewLocation = false;
  private bool _isEnemySensedByAttackSeeker = false;
  private float _creatureRiseHeight = 3.0f;
  // The Y position the creature needs to be to be on the ground
  private float _creatureGroundY = 1.04f;
  // size of each space on the board (x or y value)
  private float _spaceSize;
  private int layerMask = 9;

  // everywhere this creature can move
  [SerializeField]
  private RaycastHit[] _forwardMovementHits;
  private float _forwardXOffset = 0.0f;
  private float _forwardZOffset = 1.0f;
  private RaycastHit[] _forwardAttackHits;
  private float _forwardXAtkOffset = 0.0f;
  private float _forwardZAtkOffset = 0.5f;

  private RaycastHit[] _backwardMovementHits;
  private float _backwardZOffset = -1.0f;
  private float _backwardXOffset = 0.0f;

  private RaycastHit[] _leftMovementHits;
  private float _leftXOffset = -0.5f;
  private float _leftZOffset = 0.0f;

  private RaycastHit[] _rightMovementHits;
  private float _rightXOffset = 0.5f;
  private float _rightZOffset = 0.0f;

  private List<RaycastHit> allRayHits = new List<RaycastHit>();


  // All variables that will be saved to the file go here:

  // Player Number
  private short _playerOwner = 1;
  private Player _player;
  // number of spaces this creature can move
  private float _moveDistance = 3.0f;
  private float _attackDistance = 2.0f;
  private string _moveLine = "Straight";
  private string _attackLine = "Straight";

  // Start is called before the first frame update
  void Start()
  {
    FindAndLoadResources();

    SaveLocally();
  }



  // Update is called once per frame
  void Update()
  {


    Debug.DrawRay(transform.position, new Vector3(-1, 0, -1) * _moveDistance * _spaceSize, Color.red);



    if (_spaceSize <= 0.0f)
    {
      Debug.LogError("Cannot get space size, Ray will not be casted");
    }

    switch (_moveLine)
    {
      case "Straight":
        CreatureStraightPositioning();
        break;
      case "Diagonal":
        CreatureDiagonalPositioning();
        break;
      default:
        Debug.Log("Other value");
        break;
    }


    switch (_attackLine)
    {
      case "Straight":
        CreatureStraightAttack();
        break;
      default:
        Debug.Log("Other value");
        break;
    }


  }

  // Defines the creature's position (for straight line movement) currently and where it can move if possible
  void CreatureStraightPositioning()
  {
    if (_stopFalling == false)
    {
      transform.Translate(Vector3.down * Time.deltaTime);
    }

    // We know if the creature is selected if it is not on the ground (if the y value is above a certain number)
    // RAISE & NOT MOVED
    if (transform.position.y >= 3 && currentPosition == oldPosition)
    {
      _isSelected = true;
      _isDown = false;
      _summonAtkSeekers = true;

      // set the currently selected Creature on the player so the player knows it's position
      _player.setCurrentlySelectedCreature(this.gameObject.GetComponent<Creature>());
    }
    // RAISE & MOVED
    else if (transform.position.y >= 3 && currentPosition != oldPosition)
    {
      _isSelected = true;
      _isDown = false;
      _summonAtkSeekers = false;
    }

    // NOT RAISE & NOT MOVED
    else if (transform.position.y < 3 && currentPosition == oldPosition)
    {
      _isSelected = false;
      _isDown = true;
      _summonAtkSeekers = false;
    }

    // NOT RAISED & MOVED
    else
    {
      _isSelected = false;
      _isDown = true;
      _summonAtkSeekers = false;

      ClearBoardColor();
    }

    if (_isSelected == true)
    {
      // make this false so we know the creature is not down right now (We turned this true in the CreatureIndivMovementPattern() right after this, so if _isDown is changed to false after that method call, the HideMovementPattern() won't be called b/c _isDown must be true to enter it's if statement)
      CreatureIndivMovementPattern();

      // When I implement a turn system, we will search for the the current player by searching for the tag so we know who can press keys. We dont want Player 2 to press keys during Player 1's turn. We will use the tag to set _whoseTurnIsIt. Then we will exclude all other players except the one with the tag equal to _whoseTurnIsIt
      // When it is player 1's turn, at anytime the creature is in the air and regardless of the player's position, if they press the B key, the creature will be placed down
      if (_whoseTurnIsIt.Equals("Player_1") && _isSelected == true && Input.GetKeyDown(KeyCode.B))
      {
        transform.position = new Vector3(transform.position.x, _creatureGroundY, transform.position.z);
      }
    }
    else if (_isSelected == false && _isDown == true && _forwardMovementHits != null && _backwardMovementHits != null && _leftMovementHits != null && _rightMovementHits != null)
    {
      HideAllMovementPatterns();
    }
    else if (_isSelected == false && _isDown == true && _forwardMovementHits == null && _backwardMovementHits == null && _leftMovementHits == null && _rightMovementHits == null)
    {
      // Do nothing since this is called when the game just starts and all the RaycastHit arrays are null
    }
  }

  // determines where the creature can attack (in a straight line only)
  public void CreatureStraightAttack()
  {
    // RAISE & NOT MOVE
    if (_isSelected == true && _isDown == false && _summonAtkSeekers == true)
    {
      _destroyAtkSeekers = false;
    }
    // RAISE & MOVE
    else if (_isSelected == true && _isDown == false && _summonAtkSeekers == false)
    {
      _destroyAtkSeekers = false;
    }

    // NOT RAISE & NOT MOVE
    else if (_isSelected == false && _isDown == true && _summonAtkSeekers == false)
    {
      _destroyAtkSeekers = true;

      if (_isNewLocation == true)
      {
        allRayHits.Clear();

        CreatureAttackRays();

        if (allRayHits.Count == 0)
        {
          _isEnemySensedByAttackSeeker = false;
        }
      }
    }

    // NOT RAISED & MOVED
    else
    {
      _destroyAtkSeekers = true;
    }

  }

  // calculates which spaces the creature can move and highlights them
  void CreatureIndivMovementPattern()
  {
    Vector3 thisCreaturesPosition = transform.position;

    AllMovementPatterns(thisCreaturesPosition);
  }



  void MovementPattern(RaycastHit[] movementHitPoints, float xOffset, float zOffset)
  {
    float playerPositionX = _player.transform.position.x;
    float playerPositionZ = _player.transform.position.z;
    // we get to this method when the creature is in the air, so now we need to check if we pressed the Space key and if we did, compare the player's position with the positions in the movement pattern. If you get a match, move the creature there
    if (Input.GetKeyDown(KeyCode.Space))
    {
      for (int i = 0; i < movementHitPoints.Length; i++)
      {
        RaycastHit currentHit = movementHitPoints[i];

        float currentHitX = currentHit.point.x + xOffset;

        // we Round the only the Z float and then add 1 to fix the ray's calculating error. This only affects the Z and NOT the X
        float currentHitZ = Mathf.Round(currentHit.point.z) + zOffset;

        if (playerPositionX == currentHitX && playerPositionZ == currentHitZ)
        {
          Vector3 newLocation = new Vector3(currentHitX, _creatureGroundY, currentHitZ);
          transform.position = newLocation;

          _isNewLocation = true;

          _isSelected = false;
          _isDown = true;

          try
          {
            _attackPatterns.HideAtkPattern(movementHitPoints);
          }
          catch (System.Exception)
          {
            // throw;
          }

          break; // to avoid casting another movement pattern
        }
      }
    }

  }

  void HideAllMovementPatterns()
  {
    // use Game Data's method to turn each space back to normal color
    _movementPatterns.HideMovementPattern(_forwardMovementHits);
    _movementPatterns.HideMovementPattern(_backwardMovementHits);
    _movementPatterns.HideMovementPattern(_leftMovementHits);
    _movementPatterns.HideMovementPattern(_rightMovementHits);
  }


  // Calculates the creature's entire movement pattern by passing in it's position
  void AllMovementPatterns(Vector3 creaturePosition)
  {

    // forward ray results
    _forwardMovementHits = _movementPatterns.ForwardStraightMovementPattern(creaturePosition, _moveDistance, _spaceSize);

    MovementPattern(_forwardMovementHits, _forwardXOffset, _forwardZOffset);

    // backward ray results
    _backwardMovementHits = _movementPatterns.BackwardStraightMovementPattern(creaturePosition, _moveDistance, _spaceSize);

    MovementPattern(_backwardMovementHits, _backwardXOffset, _backwardZOffset);

    // left ray results
    _leftMovementHits = _movementPatterns.LeftStraightMovementPattern(creaturePosition, _moveDistance, _spaceSize);

    MovementPattern(_leftMovementHits, _leftXOffset, _leftZOffset);

    // right ray results
    _rightMovementHits = _movementPatterns.RightStraightMovementPattern(creaturePosition, _moveDistance, _spaceSize);

    MovementPattern(_rightMovementHits, _rightXOffset, _rightZOffset);


  }

  void AllDiagonalMovementPatterns(Vector3 creaturePosition)
  {

    // forward right diagonal ray results
    _forwardMovementHits = _movementPatterns.ForwardRightDiagonalMovementPattern(creaturePosition, _moveDistance, _spaceSize);

    MovementPattern(_forwardMovementHits, _forwardXOffset, _forwardZOffset);

    // forward left diagonal ray results
    _leftMovementHits = _movementPatterns.ForwardLeftDiagonalMovementPattern(creaturePosition, _moveDistance, _spaceSize);

    MovementPattern(_leftMovementHits, _leftXOffset, _leftZOffset);

    // backward right diagonal ray results
    _rightMovementHits = _movementPatterns.BackwardRightDiagonalMovementPattern(creaturePosition, _moveDistance, _spaceSize);

    MovementPattern(_rightMovementHits, _forwardXOffset, _forwardZOffset);


    // backward left diagonal ray results
    _backwardMovementHits = _movementPatterns.BackwardLeftDiagonalMovementPattern(creaturePosition, _moveDistance, _spaceSize);

    MovementPattern(_backwardMovementHits, _forwardXOffset, _forwardZOffset);

  }

  void SummonAttackSeekers(RaycastHit[] forwardRayHits, RaycastHit[] backwardRayHits, RaycastHit[] leftRayHits, RaycastHit[] rightRayHits, float forwardXOffset, float forwardZOffset, float backwardXOffset, float backwardZOffset, float leftXOffset, float leftZOffset, float rightXOffset, float rightZOffset)
  {


    if (_stopSummoningAtkSeekers == false)
    {
      // forward
      for (int i = 0; i < forwardRayHits.Length; i++)
      {
        RaycastHit currentHit = forwardRayHits[i];

        Vector3 currentLocation = forwardRayHits[i].point;

        float currentHitX = currentHit.point.x + forwardXOffset;
        float currentHitZ = Mathf.Round(currentLocation.z) + forwardZOffset;

        Vector3 updatedLocation = new Vector3(currentHitX, currentLocation.y + 3.0f, currentHitZ);

        AttackSeeker atkSeeker = Instantiate(_attackSeekerPrefab, updatedLocation, Quaternion.identity).GetComponent<AttackSeeker>();

        atkSeeker.setAtkSeekerID(_gameData.getAttackSeekers().Count);

        _gameData.addAtkSeeker(atkSeeker);
      }

      if (backwardRayHits != null)
      {
        // backward
        for (int i = 0; i < backwardRayHits.Length; i++)
        {
          RaycastHit currentHit = backwardRayHits[i];

          Vector3 currentLocation = backwardRayHits[i].point;

          float currentHitX = currentHit.point.x + backwardXOffset;
          float currentHitZ = Mathf.Round(currentLocation.z) + backwardZOffset;

          Vector3 updatedLocation = new Vector3(currentHitX, currentLocation.y + 3.0f, currentHitZ);

          AttackSeeker atkSeeker = Instantiate(_attackSeekerPrefab, updatedLocation, Quaternion.identity).GetComponent<AttackSeeker>();

          atkSeeker.setAtkSeekerID(_gameData.getAttackSeekers().Count);

          _gameData.addAtkSeeker(atkSeeker);
        }
      }

      if (leftRayHits != null)
      {
        // left
        for (int i = 0; i < leftRayHits.Length; i++)
        {
          RaycastHit currentHit = leftRayHits[i];

          Vector3 currentLocation = leftRayHits[i].point;

          float currentHitX = currentHit.point.x + leftXOffset;
          float currentHitZ = Mathf.Round(currentLocation.z) + leftZOffset;

          Vector3 updatedLocation = new Vector3(currentHitX, currentLocation.y + 3.0f, currentHitZ);

          AttackSeeker atkSeeker = Instantiate(_attackSeekerPrefab, updatedLocation, Quaternion.identity).GetComponent<AttackSeeker>();

          atkSeeker.setAtkSeekerID(_gameData.getAttackSeekers().Count);

          _gameData.addAtkSeeker(atkSeeker);
        }
      }

      if (rightRayHits != null)
      {
        // right
        for (int i = 0; i < rightRayHits.Length; i++)
        {
          RaycastHit currentHit = rightRayHits[i];

          Vector3 currentLocation = rightRayHits[i].point;

          float currentHitX = currentHit.point.x + rightXOffset;
          float currentHitZ = Mathf.Round(currentLocation.z) + rightZOffset;

          Vector3 updatedLocation = new Vector3(currentHitX, currentLocation.y + 3.0f, currentHitZ);

          AttackSeeker atkSeeker = Instantiate(_attackSeekerPrefab, updatedLocation, Quaternion.identity).GetComponent<AttackSeeker>();

          atkSeeker.setAtkSeekerID(_gameData.getAttackSeekers().Count);

          _gameData.addAtkSeeker(atkSeeker);
        }
      }
    }

  }
  // changes all spaces on board to white
  public void ClearBoardColor()
  {
    Renderer[] allSpaceRenderers = _board.GetComponentsInChildren<Renderer>();

    foreach (Renderer r in allSpaceRenderers)
    {
      MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();

      r.GetPropertyBlock(_propBlock);
      // Assign our new value.
      _propBlock.SetColor("_Color", Color.white);
      // Apply the edited values to the renderer.
      r.SetPropertyBlock(_propBlock);
    }
  }

  // shoots rays in all directions from creature while it is down to detect enemies in range
  void CreatureAttackRays()
  {
    AttackRayFromCreature(transform.position, Vector3.forward, _attackDistance, _spaceSize, layerMask);
    AttackRayFromCreature(transform.position, Vector3.back, _attackDistance, _spaceSize, layerMask);
    AttackRayFromCreature(transform.position, Vector3.left, _attackDistance, _spaceSize, layerMask);
    AttackRayFromCreature(transform.position, Vector3.right, _attackDistance, _spaceSize, layerMask);
  }

  // shoots a ray when the creature is down to detect enemies within range
  public void AttackRayFromCreature(Vector3 creaturePosition, Vector3 direction, float maxDistance, float spaceSize, int rayLayermask)
  {
    Vector3 rayDirection = transform.TransformDirection(direction);

    float rayDistance = maxDistance * spaceSize;

    RaycastHit[] hits = Physics.RaycastAll(creaturePosition, rayDirection, rayDistance, rayLayermask);

    int numOfEnemyHits = 0;

    foreach (RaycastHit r in hits)
    {
      // allRayHits.Add (r);
      if (r.transform.gameObject.CompareTag("Enemy"))
      {
        allRayHits.Add(r);
        // needs to be true so the path leading to the enemy is colored in gray with SpaceColorer()
        _wasEnemyHitByRay = true;
        // notify attack seeker so we can attack by pressing a certain key
        _isEnemySensedByAttackSeeker = true;
        numOfEnemyHits++;
      }
    }

    // If this is 0 that means there were no enemy hits, so no space in range should be colored
    if (numOfEnemyHits == 0)
    {
      _wasEnemyHitByRay = false;

      foreach (RaycastHit nonEnemy in hits)
      {
        _attackPatterns.HideAtkPattern(hits);
      }
    }

    // If an enemy was hit, color the spaces in that direction
    SpaceColorer(hits, _wasEnemyHitByRay);

    Debug.DrawRay(creaturePosition, rayDirection * rayDistance, Color.red);

  }

  public void SpaceColorer(RaycastHit[] hits, bool isEnemyHit)
  {

    if (isEnemyHit == true)
    {
      // Color each space that was hit with attack color
      foreach (RaycastHit rch in hits)
      {
        try
        {
          Renderer parentSpace = rch.transform.parent.GetComponent<Renderer>();
          _attackPatterns.SpaceColorSwitcher(parentSpace, Color.gray);
        }
        catch (NullReferenceException e)
        {
          // just doing this so the error goes away
          e.ToString();
        }
      }
    }
    else
    {
      // do nothing so we don't color the space
    }

  }


  void OnTriggerStay(Collider other)
  {
    // Current position START
    currentPosition = transform.position;
    oldPosition = currentPosition;

    _player.setCurrentlySelectedCreature(this.gameObject.GetComponent<Creature>());

    // If player, move this creature upwards , then cast 4 rays equal to how far this creature can move (ex. 3 spaces forward, backward, left, right)
    if (other.CompareTag("Player_1") && (Input.GetKeyDown(KeyCode.Space)) && _moveLine.Equals("Straight"))
    {

      transform.position = new Vector3(transform.position.x, _creatureRiseHeight, transform.position.z);

      AllMovementPatterns(transform.position);

      SummonAttackSeekers(_forwardMovementHits, _backwardMovementHits, _leftMovementHits, _rightMovementHits, _forwardXOffset, _forwardZOffset, _backwardXOffset, _backwardZOffset, _leftXOffset, _leftZOffset, _rightXOffset, _rightZOffset);
    }
    else if (other.CompareTag("Player_1") && (Input.GetKeyDown(KeyCode.Space)) && _moveLine.Equals("Diagonal"))
    {
      transform.position = new Vector3(transform.position.x, _creatureRiseHeight, transform.position.z);

      AllDiagonalMovementPatterns(transform.position);

      SummonAttackSeekers(_forwardMovementHits, _backwardMovementHits, _leftMovementHits, _rightMovementHits, _forwardXOffset, _forwardZOffset, _backwardXOffset, _backwardZOffset, _leftXOffset, _leftZOffset, _rightXOffset, _rightZOffset);
    }

    // If the B key is pressed, put the creature back down where it was
    if (other.CompareTag("Player_1") && _isSelected == true && Input.GetKeyDown(KeyCode.B))
    {
      transform.position = new Vector3(transform.position.x, _creatureGroundY, transform.position.z);
      _isNewLocation = false;
    }

    if (other.CompareTag("Center_Cube"))
    {
      Renderer _renderer = other.transform.parent.GetComponent<Renderer>();

      MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();

      _renderer.GetPropertyBlock(_propBlock);
      // Assign our new value.
      _propBlock.SetColor("_Color", Color.white);
      // Apply the edited values to the renderer.
      _renderer.SetPropertyBlock(_propBlock);
    }

  }

  void OnTriggerEnter(Collider other)
  {

    if (other.CompareTag("Board"))
    {
      _stopFalling = true;
    }

  }

  public float getMoveDistance()
  {
    return _moveDistance;
  }

  public void setMoveDistance(float NumOfSpacesToMove)
  {
    _moveDistance = NumOfSpacesToMove;
  }

  public string getAttackLine()
  {
    return _attackLine;
  }

  public float getAttackDistance()
  {
    return _attackDistance;
  }

  public float getSpaceSize()
  {
    return _spaceSize;
  }

  public bool getDestroyAtkSeekers()
  {
    return _destroyAtkSeekers;
  }

  public bool getIsDown()
  {
    return _isDown;
  }

  public bool getIsEnemySensedByAttackSeeker()
  {
    return _isEnemySensedByAttackSeeker;
  }

  public void setIsEnemySensedByAttackSeeker(bool isSensed)
  {
    _isEnemySensedByAttackSeeker = isSensed;
  }

  void FindAndLoadResources()
  {
    _board = GameObject.Find("Board").GetComponent<Board>();

    // get the size of each space from the Board component
    if (_board != null)
    {
      _spaceSize = _board.getSpaceSize();
    }

    _player = GameObject.Find("Player_" + _playerOwner).GetComponent<Player>();

    // get the size of each space from the Board component
    if (_player == null)
    {
      Debug.LogError("Player is null ::Creature.cs::Start()");
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

    _attackPatterns = _gameData.GetComponent<AttackPatterns>();

    if (_attackPatterns == null)
    {
      Debug.LogError("Attack Patterns script is NULL and should be on Game Data object");
    }
  }

  void SaveLocally()
  {
    CreatureToSerialize cts = new CreatureToSerialize(_moveDistance);

    _gameData.AddToCreatureDictionary(cts);
  }






  void CreatureDiagonalPositioning()
  {
    if (_isSelected == true)
    {
      AllDiagonalMovementPatterns(transform.position);
    }
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