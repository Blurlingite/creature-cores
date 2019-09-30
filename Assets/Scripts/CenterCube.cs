using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCube : MonoBehaviour
{

  private bool _isStillColliding = false;
  [SerializeField]
  private bool _isEnemyOnSpace = false;



  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  // void Update()
  // {

  // }



  void OnTriggerEnter(Collider other)
  {

    if (other.CompareTag("Enemy"))
    {
      _isEnemyOnSpace = true;
      Debug.Log("eeeee");
    }

  }


  public bool getIsEnemyOnSpace()
  {
    return _isEnemyOnSpace;

  }

}