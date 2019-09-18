using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
  [SerializeField]
  private GameObject _normalCube;



  // Start is called before the first frame update
  void Start()
  {
    DrawBoard();
  }

  void DrawBoard()
  {

    for (float i = 1; i <= 16; i++)
    {
      float nextLine = 4.0f * i;


      for (float j = 1; j <= 16; j++)
      {
        Vector3 position = new Vector3((4.0f * j), 0, nextLine);
        Instantiate(_normalCube, position, Quaternion.identity);

      }

    }

  }
}
