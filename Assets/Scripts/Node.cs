using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2 position;
    public int gCost;
    public int hCost;
    public int gridX;
    public int gridY;
    public bool bIsObstacle;
    public Node parent;

    public Node(bool bIsObstacle, Vector2 position , int gridX, int gridY)
    {
        this.bIsObstacle = bIsObstacle;
        this.position = position;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
