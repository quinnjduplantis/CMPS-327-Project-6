using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapGen;

public class playerScript : MonoBehaviour {

    public MapTile[,] map;
    //public Vector2 goal, start;
    MapTile goal, start;
    public Node current, goalNode, startNode;
    public List<Node> open, closed;
    public int xMax, yMax;

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
        Mapping();
        FindPath(start, goal);
	}

    void Mapping()
    {
        foreach (MapTile m in map)
        {
            if (m.IsGoal)
            {
                goal = m;
                startNode = new Node(m);
            }
            goal = m;
            if (m.IsStart)
            {
                start = m;
                startNode = new Node(m);
            }
        }

        current = startNode;
        open.Add(current);
        closed.Add(current);
        if (closed.Contains(new Node(start)))
            Debug.Log("Already closed.");
    }

    void FindPath(MapTile start, MapTile target)
    {
        bool done = false;
        //current.f = 100; //maybe different number-- check later if issue

        while (open.Count > 0)
        {
            for (int i = 0; i < open.Count; i++)
            {
                if (open[i].f < current.f || open[i].f == current.f && open[i].h < current.h)
                {
                    current = open[i];
                }
            }

            closed.Add(current);
            open.Remove(current);
            //foreach (Node n in closed)
            //{
            //    if (n.tile == goalNode.tile)
            //    {
            //        done = true;
            //        break;
            //    }
            //}
            //if (done)
            //{
            //    break;
            //}

            if (current.tile == goalNode.tile)
            {
                break;
            }

            List<Node> adj = current.adjacents(map, xMax, yMax, start, goal);

            foreach (Node a in adj)
            {
                if (!a.tile.Walkable || closed.Contains(a))
                {
                    continue;
                }
                bool inopen = false;
                if (open.Contains(a))
                {
                    inopen = true;
                    break;
                }
                if (!inopen)
                {
                    a.parent = current;
                    open.Add(a);
                    break;
                }
            }
        }
    }
}

public class Node
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
        h = (Mathf.Abs(m.X - start.X) * 10) + (Mathf.Abs(m.X - start.Y) * 10);
        g = (Mathf.Abs(goal.X - m.X) * 10) + (Mathf.Abs(goal.Y - m.Y) * 10);
    }

    public List<Node> adjacents(MapTile[,] map, int xMax, int yMax, MapTile start, MapTile goal)
    {
        List<Node> adjTiles = new List<Node>();
        if (tile.X - 1 >= 0 && tile.Walkable)
        {
            adjTiles.Add(new Node(map[tile.X - 1, tile.Y], start, goal));
        }
        if (tile.X + 1 <= xMax && tile.Walkable)
        {
            adjTiles.Add(new Node(map[tile.X + 1, tile.Y], start, goal));
        }
        if (tile.Y - 1 >= 0 && tile.Walkable)
        {
            adjTiles.Add(new Node(map[tile.X, tile.Y - 1], start, goal));
        }
        if (tile.Y + 1 <= yMax && tile.Walkable)
        {
            adjTiles.Add(new Node(map[tile.X, tile.Y + 1], start, goal));
        }
        return adjTiles;
    }
}
