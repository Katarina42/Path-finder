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
        int.TryParse(gridXText.text, out GameManager.Instance.grid.x);
        int.TryParse(gridYText.text, out GameManager.Instance.grid.y);
        int.TryParse(playerXText.text, out GameManager.Instance.player.x);
        int.TryParse(playerYText.text, out GameManager.Instance.player.y);
        int.TryParse(enemyXText.text, out GameManager.Instance.enemy.x);
        int.TryParse(enemyYText.text, out GameManager.Instance.enemy.y);
    }
}
