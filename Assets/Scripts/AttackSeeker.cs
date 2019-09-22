using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSeeker : MonoBehaviour
{
  List<Vector3> attackCoordinates = new List<Vector3>();
  // private bool _activateSeeker = false;
  Player player;
  // Start is called before the first frame update
  void Start()
  {

    player = GameObject.Find("Player_1").GetComponent<Player>();


  }

  // Update is called once per frame
  void Update()
  {

    // while (_activateSeeker == true)
    // {


    //   Creature creature = player.getCurrentlySelectedCreature();

    //   float spaceSize = creature.getSpaceSize();

    //   // int layerMask = player.getlayerMask();

    //   float attackDistance = creature.getAttackDistance();


    //   // Fire a ray in each direction
    //   AttackSeekerRay(Vector3.forward, attackDistance, spaceSize);
    //   AttackSeekerRay(Vector3.back, attackDistance, spaceSize);
    //   AttackSeekerRay(Vector3.left, attackDistance, spaceSize);
    //   AttackSeekerRay(Vector3.right, attackDistance, spaceSize);

    //   break; // avoid endless loop while keeping rays firing
    // }

  }



  void OnTriggerStay(Collider other)
  {

    if (other.CompareTag("Player_1"))
    {

      // _activateSeeker = true;
      Debug.Log("JJJJJJ");
      Player player = other.transform.gameObject.GetComponent<Player>();

      Creature creature = player.getCurrentlySelectedCreature();

      float spaceSize = creature.getSpaceSize();

      int layerMask = player.getlayerMask();

      float attackDistance = creature.getAttackDistance();


      // Fire a ray in each direction
      AttackSeekerRay(Vector3.forward, attackDistance, spaceSize);
      AttackSeekerRay(Vector3.back, attackDistance, spaceSize);
      AttackSeekerRay(Vector3.left, attackDistance, spaceSize);
      AttackSeekerRay(Vector3.right, attackDistance, spaceSize);
    }


  }

  void AttackSeekerRay(Vector3 direction, float maxDistance, float spaceSize)
  {
    Vector3 pos = transform.position;

    Vector3 rayDirection = transform.TransformDirection(direction);

    float rayDistance = maxDistance * spaceSize;


    RaycastHit[] hits = Physics.RaycastAll(pos, rayDirection, rayDistance);


    Debug.DrawRay(pos, rayDirection * rayDistance, Color.red);

    for (int i = 0; i < hits.Length; i++)
    {
      // add to list so we can compare each coordinates with player position
      attackCoordinates.Add(hits[i].point);
    }

  }




}
