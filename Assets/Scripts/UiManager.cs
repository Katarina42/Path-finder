using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    [Header("Grid")]
    public TMP_InputField gridXText;
    public TMP_InputField gridYText;

    [Header("Player")]
    public TMP_InputField playerXText;
    public TMP_InputField playerYText;

    [Header("Enemy")]
    public TMP_InputField enemyXText;
    public TMP_InputField enemyYText;

    [Header("Automatic run")]
    public Toggle automaticRunToggle;

    public void OnPlay()
    {
        ParseTextInput();
        GameManager.Instance.auto = automaticRunToggle.isOn;
        SceneManager.LoadScene("Grid");
    }

    private void ParseTextInput()
    {
        if(!gridXText.text.Equals(""))
            int.TryParse(gridXText.text, out GameManager.Instance.grid.x);
        if (!gridYText.text.Equals(""))
            int.TryParse(gridYText.text, out GameManager.Instance.grid.y);
        if (!playerXText.text.Equals(""))
            int.TryParse(playerXText.text, out GameManager.Instance.player.x);
        if (!playerYText.text.Equals(""))
            int.TryParse(playerYText.text, out GameManager.Instance.player.y);
        if (!enemyXText.text.Equals(""))
            int.TryParse(enemyXText.text, out GameManager.Instance.enemy.x);
        if (!enemyYText.text.Equals(""))
            int.TryParse(enemyYText.text, out GameManager.Instance.enemy.y);
    }
}
