using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelProperty : MonoBehaviour
{
    public List<PlayerMovement> levelPlayersList =new List<PlayerMovement>();
    public List<PlayerMovement> tempList =new List<PlayerMovement>();


    public void SetTempList()
    {
        if(tempList.Count > 0)
        {
            tempList.Clear();
        }
        
        
        for (int i = 0; i < levelPlayersList.Count; i++)
        {
            tempList.Add(levelPlayersList[i]);
        }
    } 

    
}
