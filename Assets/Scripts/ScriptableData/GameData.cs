using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 0)]
public class GameData : ScriptableObject 
{

    public int score;
    public int increaseScore;
    public int levelIndex;
    public int levelNumber;
    public int lifeTime;

    public bool isGameEnd=false;
    public bool isPlayerDead=false;
    public bool isPlayerGhost=false;
}
