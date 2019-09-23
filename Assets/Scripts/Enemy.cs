using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }



  void OnTriggerStay(Collider other)
  {
    if (other.CompareTag("Player_1"))
    {

      Player player = other.GetComponent<Player>();

      Creature creature = player.getCurrentlySelectedCreature();

      if (creature.getIsEnemyHit() == true && Input.GetKeyDown(KeyCode.X))
      {
        Debug.Log("Attack");
      }
    }

  }
}
