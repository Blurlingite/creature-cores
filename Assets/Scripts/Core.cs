using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script should not be communicating with any other "Core" script so that we can keep common behaviors of Cores (like knowing when to move) here and not have to copy & paste that code into each "Core" script
public class Core : MonoBehaviour {

  private bool _stopFalling = false;
  private float _coreRiseHeight = 3.0f;
  // Start is called before the first frame update
  void Start () {

  }

  // Update is called once per frame
  void Update () {

    if (_stopFalling == false) {
      transform.Translate (Vector3.down * Time.deltaTime);

    }

  }

  void OnTriggerStay (Collider other) {

    // If player, move this core upwards , then cast 4 rays equal to how far this core can move (ex. 3 spaces forward, backward, left, right)
    if (other.CompareTag ("Player") && (Input.GetKeyDown (KeyCode.Space))) {

      // Debug.Log("up");
      transform.position = new Vector3 (transform.position.x, _coreRiseHeight, transform.position.z);

    }

    // If the B key is pressed, put the core back down where it was
    if (other.CompareTag ("Player") && Input.GetKeyDown (KeyCode.B)) {

      // Debug.Log("BBBBBB");

      transform.position = new Vector3 (transform.position.x, (_coreRiseHeight - 1.96f), transform.position.z);

    }

  }

  void OnTriggerEnter (Collider other) {

    if (other.CompareTag ("Board")) {
      _stopFalling = true;

    }

  }
}