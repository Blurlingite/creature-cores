using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCore : MonoBehaviour
{

  private float _spacesToMove = 3.0f;
  private Core _coreComponent;

  // Start is called before the first frame update
  void Start()
  {
    // This script and the Core.cs script will be on the same object so just get the component (Core.cs)
    _coreComponent = GetComponent<Core>();
    // If not null give Core.cs this script's name so that Core.cs can get this script with GetComponent<>();
    if (_coreComponent == null)
    {
      Debug.Log("Core.cs is NULL ::Test Core.cs::Start()");
    }

  }

  // Update is called once per frame
  void Update()
  {

    _coreComponent.setMoveDistance(_spacesToMove);


  }



}