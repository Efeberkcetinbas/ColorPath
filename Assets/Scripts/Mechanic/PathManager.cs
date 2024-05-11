using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private List<PathCreator> pathCreators=new List<PathCreator>();
    public int index;

    //Yeni Levele gectigimizde o levelde liste degisir. Hangi renkler varsa onlar eklenir.

    private void SetOrder()
    {
        for (int i = 0; i < pathCreators.Count; i++)
        {
            pathCreators[i].isOrderMe=false;
        }

        pathCreators[index].isOrderMe=true;

        //index secilen küpün index olacak. Simdilik sirayla yapiyorum. Gosterme acisindan.
    }
}
