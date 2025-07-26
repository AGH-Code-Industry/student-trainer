using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameLevels", menuName = "Settings/Game Level Manifest")]
public class GameLevels : ScriptableObject
{
    public string[] menuScenes;
    public string[] playerScenes;
    public string defaultLevel = "Student City";
    public string defaultSpawnPoint;
    public string defaultQuest; // tymczasowo, przynajmniej na potrzeby demo2
}
