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
    public Node parent;

    public Node(Vector2 position , int gridX, int gridY)
    {
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
