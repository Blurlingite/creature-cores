using System.Collections;
using System.Collections.Generic;
using System.IO; // needed to use File class
using System;
using UnityEngine;

public class GameData : MonoBehaviour
{

  private IDictionary<short, CreatureToSerialize> p1Creatures = new Dictionary<short, CreatureToSerialize>();

  private List<AttackSeeker> attackSeekers = new List<AttackSeeker>();


  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  // void Update()
  // {

  // When Scene ends, save data to player's file we can put this method call in void OnDestroy() so it only runs once, but for now it will be here
  //  Player's Symbol data: it's attribute, level, exp it has,     exp to next level, etc.
  //  Player's entire library of Creatures: each creature's attribute, stats, level, exp gaines, exp need for next level, maximum level, etc. When you buy a creature it gets added to the library and then saved to the file

  // SaveToFile();

  // When a Scene starts, load from the player's file

  // }


  // To make this method dynamic, get all Player objects in the scene and get their unique name or tag (Player_1 for player 1, Player_2 for player 2, etc.) 
  // Then depending on that change the Dictionary you use and the file name used
  void SaveToFile()
  {
    for (short i = 0; i < p1Creatures.Count; i++)
    {
      // need this to store the result
      CreatureToSerialize creature;

      // get each creature using the index> Each creature will have it's own 8 digit ID within itself so we can recognize duplicate creatures that have different stats
      // i here is the order in which it appears in the Dictionary
      p1Creatures.TryGetValue(i, out creature);

      string json = JsonUtility.ToJson(creature);
      File.WriteAllText(Application.dataPath + "/SaveFiles/Player1Save.txt", json);
    }
  }


  public void AddToCreatureDictionary(CreatureToSerialize creature)
  {
    // get size of dict and assign that as the ID. If 0, ID is 0. Now there will be 1 creature in there so when we add the next creature, there will be 1 in the dictionary so the ID will be 1, etc.
    short dictionaryID = (Int16)p1Creatures.Count;
    p1Creatures.Add(dictionaryID, creature);
  }


  public void addAtkSeeker(AttackSeeker atkSeeker)
  {
    attackSeekers.Add(atkSeeker);
  }


  public List<AttackSeeker> getAttackSeekers()
  {
    return attackSeekers;
  }




} // end of GameData class

