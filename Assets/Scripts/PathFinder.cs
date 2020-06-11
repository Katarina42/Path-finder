using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    public Grid[,] gridMatrix;
    private Queue<Point> que;
    public uint tilesChecked;
    public string algorithamName = "Breadth First Search";
    public float timeSpent;
    public int length;

    public PathFinder(int m, int n)
    {
        gridMatrix = new Grid[m, n];
        que = new Queue<Point>();
        tilesChecked = 0;
        length = 0;
    }

    public bool FindPath()
    {
        Point enemyPoint = FindShortestPath();
        if (enemyPoint.x == -1)
        {
            Debug.Log("There is no path");
            return false;
        }
        else
        {
            int counter = PrintPath(enemyPoint);

            Debug.Log("Path length is " + counter);

            return true;
        }
    }

    private int PrintPath(Point point)
    {
        GameManager.Instance.playerPath.Push(point);

        if (gridMatrix[point.x, point.y].parent != null)
        {
            length++;
            //Debug.Log("" + point.x + "," + point.y);
            PrintPath(gridMatrix[point.x, point.y].parent);
        }

        return length;
    }
    private Point FindShortestPath()
    {
        int x = GameManager.Instance.player.x;
        int y = GameManager.Instance.player.y;

        Point startPoint = new Point(x, y);
        gridMatrix[x, y].parent = null;
        que.Enqueue(startPoint);
        gridMatrix[x, y].visited = true;

        while (que.Count > 0)
        {
            tilesChecked++;
            Point currentPoint = que.Dequeue();
            x = currentPoint.x;
            y = currentPoint.y;

            if (currentPoint.x == GameManager.Instance.enemy.x && currentPoint.y == GameManager.Instance.enemy.y)
                return currentPoint;

            //left
            if (ValidPoint(x - 1, y) && !gridMatrix[x - 1, y].visited)
            {
                gridMatrix[x - 1, y].visited = true;
                gridMatrix[x - 1, y].parent = currentPoint;
                Point point = new Point(x - 1, y);
                que.Enqueue(point);
            }

            //right
            if (ValidPoint(x + 1, y) && !gridMatrix[x + 1, y].visited)
            {
                gridMatrix[x + 1, y].visited = true;
                gridMatrix[x + 1, y].parent = currentPoint;
                Point point = new Point(x + 1, y);
                que.Enqueue(point);
            }

            //up
            if (ValidPoint(x, y + 1) && !gridMatrix[x, y + 1].visited)
            {
                gridMatrix[x, y + 1].visited = true;
                gridMatrix[x, y + 1].parent = currentPoint;
                Point point = new Point(x, y + 1);
                que.Enqueue(point);
            }

            //down
            if (ValidPoint(x, y - 1) && !gridMatrix[x, y - 1].visited)
            {
                gridMatrix[x, y - 1].visited = true;
                gridMatrix[x, y - 1].parent = currentPoint;
                Point point = new Point(x, y - 1);
                que.Enqueue(point);
            }

        }

        return new Point(-1, -1);
    }
    private bool ValidPoint(int x, int y)
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

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

