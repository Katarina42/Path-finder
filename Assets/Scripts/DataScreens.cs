using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataScreens : MonoBehaviour
{
    public GameObject levelBtnPrefab;
    public TextMeshProUGUI dataText;
    public static List<LevelData> levelsData;


    public void OpenDataPanel()
    {
        Debug.Log("" + levelsData.Count);
        this.gameObject.SetActive(true);
        for(int i=1;i<GameManager.Instance.Level;i++)
        {
            var btn = Instantiate(levelBtnPrefab, this.transform);
            btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Level " + i + " data";
            btn.GetComponent<Button>().onClick.AddListener(() => SetData(btn));
        }
    }
    public void CloseDataPanel()
    {
        this.gameObject.SetActive(false);
    }

    public void SetData(GameObject btn)
    {
        int level = btn.transform.GetSiblingIndex()-1;
        dataText.text= PrintLevelData(level);
    }

    private  string PrintLevelData(int level)
    {
        return "" + levelsData[level].PrintData();
    }
}

public struct LevelData
{
    public string algorithamName;
    public uint tilesChecked;
    public float timeSpent;

    public LevelData(string algorithamName, uint tilesChecked,float timeSpent)
    {
        this.algorithamName = algorithamName;
        this.tilesChecked = tilesChecked;
        this.timeSpent = timeSpent;
    }

    public string PrintData()
    {
        return this.algorithamName + ":\nNumber of tiles checked:" + tilesChecked + "\nTime:" + timeSpent;
    }
}
