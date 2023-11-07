using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public float nodeRadius;
    public Vector2 gridSize;
    public LayerMask obstacleLayer;

    private Node[,] grid;
    private int nodeCountX, nodeCountY;
    private float nodeDiameter;

    public List<Node> path;
    public GameObject target;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        nodeCountX = Mathf.RoundToInt(gridSize.x/ nodeDiameter );
        nodeCountY = Mathf.RoundToInt(gridSize.y / nodeDiameter );
        
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[nodeCountX, nodeCountY];
        Vector2 worldBottomLeft = new Vector2(transform.position.x, transform.position.y) - (Vector2.right * gridSize.x/2) - (Vector2.up * gridSize.y/2);

        for (int i = 0; i < nodeCountX; i++) 
        {
            for(int j = 0; j < nodeCountY; j++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (i * nodeDiameter + nodeRadius) + Vector2.up * (j * nodeDiameter + nodeRadius);
                Ray ray = new Ray(new Vector3(worldPoint.x, worldPoint.y, Camera.main.transform.position.z), new Vector3(0, 0, 1));
                //bool obstacle = !(Physics.Raycast(ray, 50f, obstacleLayer.value));
                bool obstacle = Physics2D.OverlapCircle(worldPoint, nodeRadius, obstacleLayer.value);
                grid[i, j] = new Node(obstacle, worldPoint, i , j);
                
            }
        }
    }

    public Node NodeFromWorldPoint(Vector2 worldPosition)
    {
        float fracX = (worldPosition.x + gridSize.x/2)/gridSize.x;
        float fracY = (worldPosition.y + gridSize.y / 2) / gridSize.y;
        fracX = Mathf.Clamp01(fracX);
        fracY = Mathf.Clamp01(fracY);

        int x = Mathf.RoundToInt((gridSize.x - 1) * fracX);
        int y = Mathf.RoundToInt((gridSize.y - 1) * fracY);

        return grid[x,y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> list = new List<Node>();

        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j<= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                int newI = node.gridX + i;
                int newJ = node.gridY + j;

                if(newI >= 0 && newJ >= 0 && newI < nodeCountX && newJ < nodeCountY)
                {
                    list.Add(grid[newI,newJ]);
                }
            }
        }

        return list;
    }


    private void OnDrawGizmos()
    {
        if(grid != null)
        {
            //Node targetNode = NodeFromWorldPoint(target.transform.position);
            foreach(Node point in  grid)
            {
                Gizmos.color = Color.yellow;
                if (path != null)
                {
                    if (path.Contains(point))
                    {
                        Gizmos.color = Color.green;
                    }
                }

               /* if(targetNode == point)
                {
                    Gizmos.color = Color.red;
                }*/

                if(point.bIsObstacle)
                {
                    Gizmos.color = Color.magenta;
                }
                
                Gizmos.DrawCube(new Vector3(point.position.x, point.position.y, 0), Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
