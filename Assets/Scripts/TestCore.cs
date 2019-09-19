using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCore : MonoBehaviour {

  private Board _board;
  private GameData _gameData;
  private MovementPatterns _movementPatterns;
  private Ray ray;
  private RaycastHit[] _allHits;
  [SerializeField]
  // num of spaces to move * each space's size
  private float _moveDistance = 3.0f;
  // size of each space on the board (x or y value)
  private float _spaceSize;
  private bool _isSelected, _isPlacedBackDown = false;
  // Start is called before the first frame update
  void Start () {
    _board = GameObject.Find ("Board").GetComponent<Board> ();

    // get the size of each space from the Board component
    if (_board != null) {
      _spaceSize = _board.getSpaceSize ();

    }

    _gameData = GameObject.Find ("Game_Data").GetComponent<GameData> ();
    if (_gameData == null) {
      Debug.LogError ("Game Data object is NULL");

    }

    _movementPatterns = GameObject.Find ("Game_Data").GetComponent<MovementPatterns> ();

    if (_movementPatterns == null) {
      Debug.LogError ("Movement Patterns script is NULL and should be on Game Data object");

    }
  }

  // Update is called once per frame
  void Update () {

    if (_spaceSize <= 0.0f) {
      Debug.LogError ("Cannot get space size, Ray will not be casted");
    }

    // We know if the core is selected if it is not on the ground (if the y value is above a certain number)
    if (transform.position.y >= 3) {
      _isSelected = true;

    } else {
      _isSelected = false;
    }

    if (_isSelected == true) {
      CoreIndivMovementPattern ();

      // make this false so we know the core is not down right now
      _isPlacedBackDown = false;

    } else if (_isSelected == false && _isPlacedBackDown == false && _allHits != null) {

      // use Game Data's method to turn each space back to normal color
      _movementPatterns.HideMovementPattern (_allHits);

      // don't need to set RayCast array to null since it gets reassigned whenever the core is selected, instead use _isPlacedBackDown to know that the core was placed back down
      _isPlacedBackDown = true;

    }

  }

  void CoreIndivMovementPattern () {
    Vector3 thisCoresPosition = transform.position;

    _allHits = _movementPatterns.CalculateMovementPattern (thisCoresPosition, _moveDistance, _spaceSize);
  }

}