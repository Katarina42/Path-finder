using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }     
    }

    #region setup

    private const float TILE_SIZE = 100;
    public GameObject gridTilePrefab;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Point grid;
    public Point player;
    public Point enemy;

    private GameObject gridParent;

    #endregion

    private void Awake()
    {
        if (instance != null && instance != this)
            DestroyImmediate(this.gameObject);

        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    public void InitializeGame()
    {
        StartCoroutine(Initializing());
    }

    private void SetGrid()
    {
        //seting grid
        RectTransform rect = gridParent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(grid.x * TILE_SIZE, grid.y * TILE_SIZE);

        //seting tiles
        for (int i = 0; i < grid.x * grid.y; i++)
        {
            Instantiate(gridTilePrefab, gridParent.transform);
        }
    }

    IEnumerator Initializing()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Grid") != null);
        gridParent = GameObject.FindGameObjectWithTag("Grid");

        SetGrid();
        SetPlayer();
        SetEnemy();
    }

    private void SetPlayer()
    {
        Instantiate(playerPrefab, gridParent.transform);

    }

    private void SetEnemy()
    {
        Instantiate(enemyPrefab, gridParent.transform);

    }

}

public struct Point
{
    public int x;
    public int y;
}
