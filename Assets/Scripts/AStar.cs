using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public Grid grid;

    public Queue<Node> FindPath(Vector3 startPostition, Vector3 endPostition)
    {
        Node startNode = grid.NodeFromWorldPoint(startPostition);
        Node endNode = grid.NodeFromWorldPoint(endPostition);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);
        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }

            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                return new Queue<Node>(RetracePath(startNode, endNode));
            }
                

            foreach(Node neighbour in grid.GetNeighbours(currentNode))
            {
                if(neighbour.bIsObstacle || closedSet.Contains(neighbour))
                { 
                    continue;
                }

                int newCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newCost < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCost;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = currentNode;
                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        //grid.path = path;
        return path;
    }

    int GetDistance(Node a, Node b)
    {
        int distX = Mathf.Abs(a.gridX - b.gridX);
        int distY = Mathf.Abs(a.gridY - b.gridY);

        if(distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }
    }
}
