using System.Collections;
using System.Collections.Generic;
using System.IO; // needed to use File class
using UnityEngine;

public class GameData : MonoBehaviour
{


  void Awake()
  {
    // SaveObj saveThis = new SaveObj
    // {
    //   bestNum = 5,
    //   name = "Nicole"
    // };

    // string json = JsonUtility.ToJson(saveThis);

    // File.WriteAllText(Application.dataPath + "/SaveFiles/Player1Save.txt", json);



    // GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

    // for (int i = 0; i < allObjects.Length; i++)
    // {
    //   GameObject current = allObjects[i];
    //   if (current.CompareTag("Creature"))
    //   {

    //     Debug.Log("Saved Creature");

    //   }

    // }







    // SaveObj saveThis = new SaveObj
    // {
    //   bestNum = 88,
    //   name = "HANnnnnnnnnA"
    // };

    // AnObject bn = saveThis;

    // string json = JsonUtility.ToJson(bn);

    // File.WriteAllText(Application.dataPath + "/SaveFiles/Player1Save.txt", json);

    // Debug.Log("OCEAN");



  }
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

    // When Scene ends, save data to player's file
    //  Player's Symbol data: it's attribute, level, exp it has,     exp to next level, etc.
    //  Player's entire library of Creatures: each creature's attribute, stats, level, exp gaines, exp need for next level, maximum level, etc. When you buy a creature it gets added to the library and then saved to the file

    Save();

    // When a Scene starts, load from the player's file

  }

  void Save()
  {

  }

  private class AnObject
  {
    // public int bestNum;
    // public string name;

    // public int getBestNum()
    // {
    //   return bestNum;
    // }

  }

  private class SaveObj : AnObject
  {
    public int bestNum = 33;
    public string name = "Jess";

  }





}

