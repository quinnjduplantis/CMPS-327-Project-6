using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapGen;

public class playerScript : MonoBehaviour {

    public MapTile[,] map;
    public MapTile goal, start;
    public Node current, goalNode, startNode;
    public List<Node> open, closed, myPath;
    public int xMax, yMax;
    public float speed = 20.0f;
    public Vector3 currentPos;
    public State state;

    public enum State
    {
        Idle, Moving, Remake
    }

	// Use this for initialization
	void Start () {
        // Find startNode
        // Find goalNode
        // Put startNode in open list
        // Adjacents starting from startNode
        // Find f of all adj Nodes
        // Set all adj Nodes parent to current Node
        // Put all adj Nodes in open list, move startNode to closed list
        // Find open with best f
        // Find adj Nodes of best f open Node
        //      Loop
        // If Node you hit isGoal > Exit
        // If open is empty > No path > Exit
        // NO REPEATS: Before adding open, check closed and open
        open = new List<Node>();
        closed = new List<Node>();
        myPath = new List<Node>();
        state = State.Idle;
        FindPath(start, goal);
	}

    void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Moving:
                if (myPath.Count > 0)
                {
                    Vector3 distance = new Vector3(myPath[0].tile.X, myPath[0].tile.Y, transform.position.z) - transform.position;
                    if (distance.magnitude < 0.1f)
                    {
                        transform.position = new Vector3(myPath[0].tile.X, myPath[0].tile.Y, transform.position.z);
                        myPath.Remove(myPath[0]);
                    }
                    transform.position += distance.normalized * Time.deltaTime;
                } else
                {
                    state = State.Remake;
                }
                break;
            case State.Remake:
                state = State.Idle;
                break;
        }
    }

    void FindPath(MapTile start, MapTile target)
    {
        current = new Node(start, start, target);
        open.Add(current);
        bool done = false;
        //current.f = 100; //maybe different number-- check later if issue
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
        if (done) {
            myPath.Add(current);
            while (!(current.parent == null))
            {
                current = current.parent;
                myPath.Add(current);
            }
            myPath.Reverse();
            state = State.Moving;
            Debug.Log("I has a path");
        } else {
            Debug.Log("No valid path found.");
        }
    }

    public void OnDrawGizmos()
    {
        foreach (Node node in myPath)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(new Vector3(node.tile.X, node.tile.Y, 1), .5f);
        }
    }
}

public class Node : System.IEquatable<Node>
{
    public MapTile tile;
    public Node parent;
    public int g, h;

    public int f
    {
        get
        {
            return g + h;
        }
    }

    public Node()
    {
        tile = new MapTile();
        g = 0;
        h = 0;
    }

    public Node(MapTile m)
    {
        tile = m;
    }

    public Node(MapTile m, MapTile start, MapTile goal)
    {
        tile = m;
        h = (Mathf.Abs(m.X - start.X) * 10) + (Mathf.Abs(m.Y - start.Y) * 10);
        g = (Mathf.Abs(goal.X - m.X) * 10) + (Mathf.Abs(goal.Y - m.Y) * 10);
    }

    public List<Node> adjacents(MapTile[,] map, int xMax, int yMax, MapTile start, MapTile goal)
    {
        List<Node> adjTiles = new List<Node>();
        if (tile.X - 1 >= 0 && tile.Walkable)
        {
            adjTiles.Add(new Node(map[tile.X - 1, tile.Y], start, goal));
        }
        if (tile.X + 1 < xMax && tile.Walkable)
        {
            adjTiles.Add(new Node(map[tile.X + 1, tile.Y], start, goal));
        }
        if (tile.Y - 1 >= 0 && tile.Walkable)
        {
            adjTiles.Add(new Node(map[tile.X, tile.Y - 1], start, goal));
        }
        if (tile.Y + 1 < yMax && tile.Walkable)
        {
            adjTiles.Add(new Node(map[tile.X, tile.Y + 1], start, goal));
        }
        return adjTiles;
    }

    public override string ToString()
    {
        return "[" + tile.X + " , " + tile.Y + "]" + " f = " + f + " g = " + g + " h = " + h;
    }

    public bool Equals (Node other)
    {
        if (this.tile.X == other.tile.X && this.tile.Y == other.tile.Y)
        {
            return true;
        }
        else return false;
    }
}
