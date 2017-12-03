using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapGen;

public class playerScript : MonoBehaviour {

    public MapTile[,] map;
    public Vector2 goal, start;
    public node myNode;
    public List<node> TODO, DONE;

	// Use this for initialization
	void Start () {
        // Find startNode
        // Find goalNode
        // Put startNode in TODO list
        // Adjacents starting from startNode
        // Find f of all adj nodes
        // Put all adj nodes in TODO list, move startNode to DONE list
        // Find TODO with best f
        // Find adj nodes of best f TODO node
        //      Loop
        // If node you hit isGoal > Exit
        // If TODO is empty > No path > Exit
        // NO REPEATS: Before adding TODO, check DONE and TODO
		foreach (MapTile m in map)
        {
            if (m.IsStart)
            {
                node startNode = new node(m, calculateG(m, goal), 0);
            }
            else if (m.IsGoal)
            {
                node goalNode = new node(m, 0, calculateH(m, start));
            }
            else if (m.Walkable)
            {
                
            }
        }
	}
	
    public int calculateG(MapTile current, Vector2 goal)
    {
        int x = Mathf.Abs((int) goal.x - current.X);
        int y = Mathf.Abs((int) goal.y - current.Y);
        return x + y;
    }

    public int calculateH(MapTile current, Vector2 start)
    {
        int x = Mathf.Abs((int) start.x - current.X);
        int y = Mathf.Abs((int) start.y - current.Y);
        return x + y;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
