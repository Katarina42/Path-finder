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
    private const float PLAYER_SPEED = 5;

    public GameObject gridTilePrefab;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Point grid;
    public Point player;
    private Point playerCurrent;
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

    public Stack<Point> playerPath = new Stack<Point>();

    public bool auto;

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
        playerCurrent = new Point(0, 4); 

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

        if (auto)
            OnRun();
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

        if (auto)
            runBtn.SetActive(false);
        else
            runBtn.GetComponent<Button>().onClick.AddListener(() => OnRun());
    }

    public void OnPlay()
    {
        if (playerCurrent.x == enemy.x && playerCurrent.y == enemy.y) ;
            SceneManager.LoadScene("Grid");
    }

    public void OnRun()
    {
        if (pathFinder.FindPath())
        {
            runBtn.SetActive(false);
            DataScreens.levelsData.Add(new LevelData(pathFinder.algorithamName, pathFinder.tilesChecked, pathFinder.timeSpent));
            MovePlayer();
        }
        else
        {
            SceneManager.LoadScene("Menu");
        } 
    }

    #endregion

    #region Player movement

    private void MovePlayer()
    {
        playerCurrent.x = player.x;
        playerCurrent.y = player.y;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        GameObject playerObj = pathFinder.gridMatrix[playerCurrent.x, playerCurrent.y].tile.transform.GetChild(0).gameObject;
        float currTime = 0;
        Point currPos = playerPath.Pop();
        Transform endPos = pathFinder.gridMatrix[currPos.x, currPos.y].tile.transform;
        while(currTime < 1 && playerObj!=null)
        {
            playerObj.transform.SetParent(pathFinder.gridMatrix[currPos.x, currPos.y].tile.transform);
            playerCurrent.x = currPos.x;
            playerCurrent.y = currPos.y;
            playerObj.transform.position = Vector3.Lerp(playerObj.transform.position, endPos.position, Time.deltaTime*PLAYER_SPEED);
            currTime += Time.deltaTime*PLAYER_SPEED;
            yield return null;
        }

        if (playerPath.Count > 0)
            StartCoroutine(Move());

    }

    #endregion
}

