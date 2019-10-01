using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSeeker : MonoBehaviour
{

  [SerializeField]
  private int atkSeekerID;

  private Player player;
  private Creature playerSelectedCreature;
  private AttackPatterns _attackPatterns;
  private GameData _gameData;

  // private List<Vector3> attackCoordinates = new List<Vector3>();

  private List<RaycastHit> _forwardAtkHits;
  private List<RaycastHit> _backwardAtkHits;
  private List<RaycastHit> _leftAtkHits;
  private List<RaycastHit> _rightAtkHits;

  // Start is called before the first frame update
  void Start()
  {
    _gameData = GameObject.Find("Game_Data").GetComponent<GameData>();

    player = GameObject.Find("Player_1").GetComponent<Player>();

    playerSelectedCreature = player.getCurrentlySelectedCreature();

    _attackPatterns = _gameData.GetComponent<AttackPatterns>();
  }

  // Update is called once per frame
  void Update()
  {
    bool selfDestruct = playerSelectedCreature.getDestroyAtkSeekers();

    if (selfDestruct == true)
    {
      Destroy(this.gameObject);
    }
  }


  void OnTriggerStay(Collider other)
  {
    if (other.CompareTag("Player_1"))
    {
      Player player = other.transform.gameObject.GetComponent<Player>();

      Creature creature = player.getCurrentlySelectedCreature();

      Vector3 position = transform.position;


      float spaceSize = creature.getSpaceSize();
      int layerMask = player.getlayerMask();
      float attackDistance = creature.getAttackDistance();

      if (!Input.GetKey(KeyCode.B) && creature.getAttackLine().Equals("Straight"))
      {
        _forwardAtkHits = _attackPatterns.StraightAttackSeekerRay(position, Vector3.forward, attackDistance, spaceSize, layerMask);

        _backwardAtkHits = _attackPatterns.StraightAttackSeekerRay(position, Vector3.back, attackDistance, spaceSize, layerMask);

        _leftAtkHits = _attackPatterns.StraightAttackSeekerRay(position, Vector3.left, attackDistance, spaceSize, layerMask);

        _rightAtkHits = _attackPatterns.StraightAttackSeekerRay(position, Vector3.right, attackDistance, spaceSize, layerMask);
      }

      else if (!Input.GetKey(KeyCode.B) && creature.getAttackLine().Equals("Diagonal"))
      {

        // forward right ray
        _forwardAtkHits = _attackPatterns.DiagonalAttackSeekerRay(position, new Vector3(1, 0, 1), attackDistance, spaceSize, layerMask);

        // forward left ray
        _leftAtkHits = _attackPatterns.DiagonalAttackSeekerRay(position, new Vector3(-1, 0, 1), attackDistance, spaceSize, layerMask);

        // backward left ray
        _backwardAtkHits = _attackPatterns.DiagonalAttackSeekerRay(position, new Vector3(-1, 0, -1), attackDistance, spaceSize, layerMask);

        // backward right ray
        _rightAtkHits = _attackPatterns.DiagonalAttackSeekerRay(position, new Vector3(1, 0, -1), attackDistance, spaceSize, layerMask);

      }

    }

    if (other.CompareTag("Creature"))
    {
      Debug.Log("core");
    }
  }


  void OnTriggerExit(Collider other)
  {
    // hide attack pattern when player leaves this attack seeker
    if (other.CompareTag("Player_1"))
    {
      _attackPatterns.HideAtkPattern(_forwardAtkHits);
      _attackPatterns.HideAtkPattern(_backwardAtkHits);
      _attackPatterns.HideAtkPattern(_leftAtkHits);
      _attackPatterns.HideAtkPattern(_rightAtkHits);
    }
  }

  public void setAtkSeekerID(int seekerID)
  {
    atkSeekerID = seekerID;
  }



} // end of AttackSeeker class
