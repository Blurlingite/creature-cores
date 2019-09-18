using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{

  private bool _stopFalling = false;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

    if (_stopFalling == false)
    {
      transform.Translate(Vector3.down * Time.deltaTime);

    }


  }



  void OnTriggerStay(Collider other)
  {

    // If player, move this core upwards , then cast 4 rays equal to how far this core can move (ex. 3 spaces forward, backward, left, right)
    if (other.CompareTag("Player"))
    {

      Debug.Log("up");
      transform.position = new Vector3(transform.position.x, 3, transform.position.z);
    }

  }


  void OnTriggerEnter(Collider other)
  {

    if (other.CompareTag("Board"))
    {
      _stopFalling = true;

    }

  }
}
