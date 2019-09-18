using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCube : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {


  }


  void OnTriggerEnter(Collider other)
  {

    if (other.CompareTag("Player"))
    {
      // change top of cube to green
      Debug.Log("player");
    }



  }
}
