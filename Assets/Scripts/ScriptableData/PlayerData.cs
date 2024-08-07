using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData", order = 1)]
public class PlayerData : ScriptableObject 
{
    public int numberOfPlayers;
    public int pathCompletedCounter;
    public int successPathCompletedCounter;

    public Color selectedColor;
    
}
