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

    #region Unity

    private void Awake()
    {
        if (instance != null && instance != this)
            DestroyImmediate(this.gameObject);

        instance = this;

        DontDestroyOnLoad(this.gameObject);

        grid = new Point(10,10);
        enemy = new Point(9, 4);
        player = new Point(0, 4);
    }

    #endregion

    #region Game initialization

    public void InitializeGame()
    {
        StartCoroutine(Initializing());
    }

    IEnumerator Initializing()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Grid") != null);
        gridParent = GameObject.FindGameObjectWithTag("Grid");

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
}

public class PathFinder
{
    public Grid[,] gridMatrix;
    private Queue<Point> que;
    private int length;

    public PathFinder(int m,int n)
    {
        gridMatrix = new Grid[m,n];
        que = new Queue<Point>();

        Point enemyPoint = FindShortestPath();
        if (enemyPoint.x == -1)
            Debug.Log("There is no path");
        else
        {
            int counter = PrintPath(enemyPoint);

            Debug.Log("Path length is " + counter);
        }

    }

    private int PrintPath(Point point)
    {
        if (gridMatrix[point.x,point.y].parent != null)
        {
            length++;
            Debug.Log("" + point.x + "," + point.y);
            PrintPath(gridMatrix[point.x, point.y].parent);
        }

        return length;
    }

    private Point FindShortestPath()
    {
        int x = GameManager.Instance.player.x;
        int y = GameManager.Instance.player.y;

        Point startPoint = new Point (x,y);
        gridMatrix[x, y].parent = null;
        que.Enqueue(startPoint);
        gridMatrix[x,y].visited = true;

        while(que.Count>0)
        {
            Point currentPoint = que.Dequeue();
            x = currentPoint.x;
            y = currentPoint.y;

            //Debug.Log("Current " + x + ":" + y);

            if (currentPoint.x == GameManager.Instance.enemy.x && currentPoint.y == GameManager.Instance.enemy.y)
            {
                return currentPoint;
            }

            //left
            if(ValidPoint(x-1,y) && !gridMatrix[x-1, y].visited)
            {
                gridMatrix[x - 1, y].visited = true;
                gridMatrix[x-1, y].parent = currentPoint;
                Point point = new Point(x - 1, y);
                que.Enqueue(point);
            }

            //right
            if (ValidPoint(x+1, y) && !gridMatrix[x+1, y].visited)
            {
                gridMatrix[x + 1, y].visited = true;
                gridMatrix[x+1, y].parent = currentPoint;
                Point point = new Point(x + 1, y);
                que.Enqueue(point);
            }
            //up
            if (ValidPoint(x, y+1) && !gridMatrix[x, y +1].visited)
            {
                gridMatrix[x, y+1].visited = true;
                gridMatrix[x, y+1].parent = currentPoint;
                Point point = new Point(x, y + 1);
                que.Enqueue(point);
            }
            //down
            if (ValidPoint(x, y-1) && !gridMatrix[x, y - 1].visited)
            {
                gridMatrix[x , y-1].visited = true;
                gridMatrix[x, y-1].parent = currentPoint;
                Point point = new Point(x, y - 1);
                que.Enqueue(point);
            }

        }

        return new Point(-1, -1);
    }

    private bool ValidPoint(int x,int y)
    {
        if (x >= 0 && x < GameManager.Instance.grid.x && y >= 0 && y < GameManager.Instance.grid.y)
            return true;

        return false;
    }

}

public struct Grid
{
    public GameObject tile;
    public bool visited;
    public Point parent;
}

public class Point
{
    public int x;
    public int y;

    public Point(int x, int y )
    {
        this.x = x;
        this.y = y;
    }
}
