using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void OnPlay()
    {
        ParseTextInput();
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
