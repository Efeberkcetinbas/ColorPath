using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


[System.Serializable]
public class IncrementalData
{
    public GameObject image; // Holds the image GameObject
    public string name;      // Holds the name
    public string info;      // Holds the info
}
public class IncrementalManager : MonoBehaviour
{   
    [SerializeField] private GameObject infoSpecialPanel;
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI infoText;

    // List of IncrementalData to hold all the images, names, and info
    [SerializeField] private List<IncrementalData> incrementalDataList = new List<IncrementalData>();
    [SerializeField] private Ease ease;

    public void OpenIncrementalInfo(int index)
    {
        if (index < 0 || index >= incrementalDataList.Count) return; // Safety check for index

        // Activate the info panel
        infoSpecialPanel.SetActive(true);
        infoSpecialPanel.transform.DOScale(Vector3.one,0.5f).SetEase(ease);
        // Hide all images and only show the selected one
        for (int i = 0; i < incrementalDataList.Count; i++)
        {
            incrementalDataList[i].image.SetActive(i == index);
        }

        // Set header and info text
        headerText.text = incrementalDataList[index].name;
        infoText.text = incrementalDataList[index].info;
    }

    public void BackButton()
    {
        infoSpecialPanel.transform.DOScale(Vector3.zero,0.5f).SetEase(ease).OnComplete(()=>{
            infoSpecialPanel.SetActive(false);;
        });
        
    }
}

