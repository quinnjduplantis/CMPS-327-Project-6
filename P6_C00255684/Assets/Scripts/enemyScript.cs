using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapGen;

public class enemyScript : MonoBehaviour {

    public MapTile[,] map;
    public MapTile goal, start;
    public Node current, goalNode, startNode;
    public List<Node> open, closed, myPath;
    public int xMax, yMax, distTraveled;
    public float speed = 20.0f;
    public Vector3 playerPos, myPos;
    public State state;
    public GameObject player;

    public enum State
    {
        Idle, RenewPath, Chasing
    }

    // Use this for initialization
    void Start()
    {
        // Run AStar, but wait a certain number of tiles moved before recalculating it.
        open = new List<Node>();
        closed = new List<Node>();
        myPath = new List<Node>();
        state = State.Idle;
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform.position;
        goal = map[Mathf.RoundToInt(playerPos.x), Mathf.RoundToInt(playerPos.y)];
        myPos = transform.position;
        start = map[Mathf.RoundToInt(myPos.x), Mathf.RoundToInt(myPos.y)];
        FindPath(start, goal);
        
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.RenewPath:
                distTraveled = 0;
                playerPos = player.transform.position;
                goal = map[Mathf.RoundToInt(playerPos.x), Mathf.RoundToInt(playerPos.y)];
                myPos = transform.position;
                start = map[Mathf.RoundToInt(myPos.x), Mathf.RoundToInt(myPos.y)];
                FindPath(start, goal);
                state = State.Chasing;
                break;
            case State.Chasing:
                if (myPath.Count > 0)
                {
                    // Make some kind of distance traveled, check if certain distTraveled and still not at goal, RenewPath
                    Vector3 distance = new Vector3(myPath[0].tile.X, myPath[0].tile.Y, transform.position.z) - transform.position;
                    if (distance.magnitude < 0.1f)
                    {
                        transform.position = new Vector3(myPath[0].tile.X, myPath[0].tile.Y, transform.position.z);
                        myPath.Remove(myPath[0]);
                    }
                    transform.position += distance.normalized * Time.deltaTime;
                }
                else
                {
                    state = State.Idle;
                }
                break;
        }
    }

    void FindPath(MapTile start, MapTile target)
    {
        current = new Node(start, start, target);
        open.Add(current);
        bool done = false;
        //Debug.Log(current.ToString());
        while (open.Count > 0)
        {
            current = open[0];
            //Debug.Log(current.ToString());
            for (int i = 0; i < open.Count; i++)
            {
                if (open[i].f < current.f)
                {
                    current = open[i];
                }
            }

            closed.Add(current);
            open.Remove(current);

            if (current.tile == target)
            {
                done = true;
                break;
            }

            List<Node> adj = current.adjacents(map, xMax, yMax, start, goal);

            foreach (Node a in adj)
            {
                if (!a.tile.Walkable || closed.Contains(a) || open.Contains(a))
                {
                    continue;
                }
                else
                {
                    a.parent = current;
                    open.Add(a);
                    Debug.Log(a.ToString());
                }
            }
        }
        if (done)
        {
            myPath.Add(current);
            while (!(current.parent == null))
            {
                current = current.parent;
                myPath.Add(current);
            }
            myPath.Reverse();
            state = State.Chasing;
            Debug.Log("Enemy has a path");
        }
        else
        {
            Debug.Log("Enemy has no valid path found.");
        }
    }

    public void OnDrawGizmos()
    {
        foreach (Node node in myPath)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(new Vector3(node.tile.X, node.tile.Y, 1), .5f);
        }
    }
}