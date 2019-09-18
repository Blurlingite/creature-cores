using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
  [SerializeField]
  private Material _movePatternColor;

  [SerializeField]
  private Material _hideMovePatternColor;


  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }


  public Material getMovePatternColor()
  {
    return _movePatternColor;
  }


  public Material getHideMovePatternColor()
  {
    return _hideMovePatternColor;
  }


}
