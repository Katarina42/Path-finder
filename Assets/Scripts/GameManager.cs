using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }     
    }

    #region Setup data

    private const float TILE_SIZE = 100;
    public GameObject gridTilePrefab;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Point grid;
    public Point player;
    public Point enemy;

    private GameObject gridParent;
    private PathFinder pathFinder;

    #endregion

    #region Game data

    private uint level;
    public uint Level
    {
        get { return level; }
        set
        {
            if(value!=level)
            {
                level = value;
                InitializeGame();
            }
        }
    }

    public GameObject playBtn;
    public GameObject runBtn;

    #endregion

    #region Unity

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(this.gameObject);

        grid = new Point(10,10);
        enemy = new Point(9, 4);
        player = new Point(0, 4);

        level = 0;
        DataScreens.levelsData = new List<LevelData>();
        SceneManager.sceneLoaded += LevelLoaded;
    }

    public void LevelLoaded(Scene scene,LoadSceneMode mode)
    {
        if(scene.name!="Menu")
            Level++;
    }

    #endregion

    #region Game initialization

    public void InitializeGame()
    {
        StartCoroutine(Initializing());
    }

    IEnumerator Initializing()
    {
        yield return new WaitForSeconds(Time.deltaTime * 3);
        gridParent = GameObject.FindGameObjectWithTag("Grid");
        SetButtons();
        SetGrid();
        SetPlayer();
        SetEnemy();
    }

    private void SetGrid()
    {
        //seting grid
        RectTransform rect = gridParent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(grid.x * TILE_SIZE, grid.y * TILE_SIZE);
        pathFinder = new PathFinder(grid.x, grid.y);

        //seting tiles
        for (int i = 0; i < grid.x; i++)
        {
            for (int j = 0; j < grid.y; j++)
            {
                GameObject tile = Instantiate(gridTilePrefab, gridParent.transform);
                pathFinder.gridMatrix[i, j].tile = tile;
                pathFinder.gridMatrix[i, j].visited = false;
            }
        }

        SetBlocks();
    }

    private void SetBlocks()
    {
        for (int i = 0; i < level; i++)
        {
            int x = Random.Range(0, grid.x);
            int y = Random.Range(0, grid.y);

            pathFinder.gridMatrix[x, y].tile.GetComponent<Image>().color = Color.black;
            pathFinder.gridMatrix[x, y].visited = true;
        }
    }

    private void SetPlayer()
    {
        Instantiate(playerPrefab, pathFinder.gridMatrix[player.x,player.y].tile.transform);

    }

    private void SetEnemy()
    {
        Instantiate(enemyPrefab, pathFinder.gridMatrix[enemy.x, enemy.y].tile.transform);
    }

    #endregion

    #region Game Ui

    private void SetButtons()
    {
        playBtn = GameObject.FindGameObjectWithTag("Play");
        runBtn = GameObject.FindGameObjectWithTag("Run");

        playBtn.GetComponent<Button>().onClick.AddListener(() => OnPlay());
        runBtn.GetComponent<Button>().onClick.AddListener(() => OnRun());
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("Grid");
    }

    public void OnRun()
    {
        if (pathFinder.FindPath())
        {
            runBtn.SetActive(false);
            DataScreens.levelsData.Add(new LevelData(pathFinder.algorithamName, pathFinder.tilesChecked, pathFinder.timeSpent));
        }
        else
        {
            SceneManager.LoadScene("Menu");
        } 
    }

    #endregion
}

