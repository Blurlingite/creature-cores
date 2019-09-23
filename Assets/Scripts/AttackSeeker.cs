using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSeeker : MonoBehaviour
{
  List<Vector3> attackCoordinates = new List<Vector3>();
  // private bool _activateSeeker = false;
  Player player;
  Creature playerSelectedCreature;
  [SerializeField]
  private int atkSeekerID;
  private AttackPatterns _attackPatterns;
  private GameData _gameData;

  private RaycastHit[] _forwardAtkHits;
  private RaycastHit[] _backwardAtkHits;
  private RaycastHit[] _leftAtkHits;
  private RaycastHit[] _rightAtkHits;
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

      Vector3 position = transform.position;
      // Debug.Log("JJJJJJ");
      Player player = other.transform.gameObject.GetComponent<Player>();

      Creature creature = player.getCurrentlySelectedCreature();

      float spaceSize = creature.getSpaceSize();

      int layerMask = player.getlayerMask();

      float attackDistance = creature.getAttackDistance();


      // Fire a ray in each direction
      // AttackSeekerRay(Vector3.forward, attackDistance, spaceSize, layerMask);
      // AttackSeekerRay(Vector3.back, attackDistance, spaceSize, layerMask);
      // AttackSeekerRay(Vector3.left, attackDistance, spaceSize, layerMask);
      // AttackSeekerRay(Vector3.right, attackDistance, spaceSize, layerMask);

      _forwardAtkHits = _attackPatterns.StraightAttackSeekerRay(position, Vector3.forward, attackDistance, spaceSize, layerMask, attackCoordinates);

      _backwardAtkHits = _attackPatterns.StraightAttackSeekerRay(position, Vector3.back, attackDistance, spaceSize, layerMask, attackCoordinates);

      _leftAtkHits = _attackPatterns.StraightAttackSeekerRay(position, Vector3.left, attackDistance, spaceSize, layerMask, attackCoordinates);

      _rightAtkHits = _attackPatterns.StraightAttackSeekerRay(position, Vector3.right, attackDistance, spaceSize, layerMask, attackCoordinates);
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
      // Debug.Log("go");
      _attackPatterns.HideAtkPattern(_forwardAtkHits);
      _attackPatterns.HideAtkPattern(_backwardAtkHits);
      _attackPatterns.HideAtkPattern(_leftAtkHits);
      _attackPatterns.HideAtkPattern(_rightAtkHits);

    }



  }


  // void AttackSeekerRay(Vector3 direction, float maxDistance, float spaceSize, int layerMask)
  // {
  //   Vector3 pos = transform.position;

  //   Vector3 rayDirection = transform.TransformDirection(direction);

  //   float rayDistance = maxDistance * spaceSize;


  //   RaycastHit[] hits = Physics.RaycastAll(pos, rayDirection, rayDistance, layerMask);


  //   Debug.DrawRay(pos, rayDirection * rayDistance, Color.red);

  //   for (int i = 0; i < hits.Length; i++)
  //   {
  //     // add to list so we can compare each coordinates with player position
  //     attackCoordinates.Add(hits[i].point);
  //   }

  // }


  public void setAtkSeekerID(int seekerID)
  {
    atkSeekerID = seekerID;
  }

}
