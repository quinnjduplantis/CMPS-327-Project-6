using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MapGen;

public class aStar : MonoBehaviour
{

    MapExample grid;
    public List<Node> path;

    public float speed = 5;
    private int waypoint = 0;
    public bool solvable = true;
    public int pathCount;


    void Awake()
    {
        grid = GetComponent<MapExample>();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void FindPath(Node start, Node target)
    {
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        Node startNode = start;
        Node goalNode = target;
        openList.Add(startNode);
        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++) //finds node with least fCost
            {
                if (openList[i].getFcost < currentNode.getFcost || openList[i].getFcost == currentNode.getFcost)
                {
                    if (openList[i].hCost < currentNode.hCost)
                    {
                        currentNode = openList[i];
                    }
                }
            }

            openList.Remove(currentNode); //removes that node
            closedList.Add(currentNode); //adds it to other list
            if (currentNode.x == goalNode.x && currentNode.y == goalNode.y) //if you have reached the goal, done
            {
                backwardPath(startNode, goalNode);
                break;
            }

            foreach (Node neighbor in neighbors(currentNode))
            {
                if (neighbor.walkable && !closedList.Contains(neighbor))//if the neighbor walkable or closedList doesn't contain neighbor
                {
                    int newfCost = currentNode.gCost + getDistance(currentNode, neighbor);//changes current gCost to new cost
                    if (newfCost < neighbor.gCost || !openList.Contains(neighbor))//if the new cost is less than neighbors gCost or neighbor is not in openList
                    {
                        neighbor.gCost = newfCost;
                        neighbor.hCost = getDistance(neighbor, goalNode);
                        neighbor.parent = currentNode;
                        if (!openList.Contains(neighbor))//if neighbor isnt in openList, add it
                        {
                            openList.Add(neighbor);
                        }
                    }
                }
            }
        }
    }

    int getDistance(Node a, Node b)//hCost
    {
        int xDistance = Mathf.Abs(a.x - b.x); //absolute value of distance between nodes on x-axis
        int yDistance = Mathf.Abs(a.y - b.y); // " on y-axis
        return xDistance + yDistance;
    }

    List<Node> neighbors(Node current)
    {

        List<Node> neighbors = new List<Node>(); //creates list of neighbors
        for (int x = -1; x <= 1; x++) //searches a 3x3 block around the node
        {
            for (int y = -1; y <= 1; y++)
            {
                if ((x == 0 && (y == -1 || y == 1)) || (y == 0 && (x == -1 || x == 1)))//checks only up, down, left, right
                {
                    int checkX = current.x + x;
                    int checkY = current.y + y;
                    if (checkX >= 0 && checkX < grid.sizeX && checkY >= 0 && checkY < grid.sizeY) //checks if node is inside of grid
                    {
                        neighbors.Add(grid.node[checkX, checkY]);
                    }
                }
            }
        }
        return neighbors;
    }

    void backwardPath(Node startNode, Node goalNode)
    {
        path = new List<Node>();
        pathCount = 0;
        Node currentNode = goalNode;
        while (currentNode.parent != null)
        {
            Instantiate(grid.pathPrefab, new Vector3(currentNode.x, 0, currentNode.y), Quaternion.identity, grid.pathParent.transform);
            currentNode = currentNode.parent;
            path.Add(currentNode);
            pathCount++;
        }
        path.Reverse();
        path.Add(goalNode);
        //Debug.Log(path.Count);
    }

    public List<Node> getPath()
    {
        //Debug.Log(pathCount);
        return path;
    }

}
