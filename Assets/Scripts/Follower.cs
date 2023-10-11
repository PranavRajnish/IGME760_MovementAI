using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Player player;
    public Grid grid;
    public float targetRadius;
    public float maxSpeed;
    public float rotationOffset;

    private AStar astar;
    private Vector3 targetPosition;
    private Queue<Node> path;

    // Start is called before the first frame update
    void Start()
    {
        astar = GetComponent<AStar>();
    }

    // Update is called once per frame
    void Update()
    {
        //Follow();
        if(grid.NodeFromWorldPoint(transform.position) != grid.NodeFromWorldPoint(player.transform.position))
        {
            path = astar.FindPath(transform.position, player.transform.position);
        }

        FollowPath();
    }

    private void Follow()
    {
        Vector2 direction = player.transform.position - transform.position;
        float distance = direction.magnitude;

        if (distance <= targetRadius)
        {
            return;
        }

        Vector2 velocity = direction.normalized * (distance / targetRadius);

        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        FaceTarget(velocity);

        float newX = transform.position.x + velocity.x * Time.deltaTime;
        float newY = transform.position.y + velocity.y * Time.deltaTime;

        transform.position = new Vector2(newX, newY);
    }

    private void FollowPath()
    {
        if (path != null && path.Count > 0)
        {
            Node currentNode = grid.NodeFromWorldPoint(transform.position);
            if (currentNode == path.Peek())
            {
                path.Dequeue();
            }
            else
            {
                targetPosition = path.Peek().position;
                //Move();
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * maxSpeed);
                Vector3 direction = targetPosition - transform.position;
                FaceTarget(direction);
            }
        }

    }

    private void FaceTarget(Vector3 velocity)
    {
        if (velocity.magnitude > 0)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            angle += rotationOffset;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
