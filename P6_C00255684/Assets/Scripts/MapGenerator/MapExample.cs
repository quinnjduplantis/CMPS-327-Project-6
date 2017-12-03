using System.Collections.Generic;
using UnityEngine;
using System.Collections;
// includes the MapGen namespace methods
using MapGen;

public class MapExample : MonoBehaviour
{
    [SerializeField]
    GameObject yesTile, noTile, startTile, goalTile, player, enemy;

    [SerializeField]
    int maxX, maxY;


	void Start ()
    {
        PrimGenerator primGen = new PrimGenerator();
        // generate a map of size 20x20 with no extra walls removed
        MapTile[,] tiles1 = primGen.MapGen(maxX, maxY, 0.0f);
        PerlinGenerator perlinGen = new PerlinGenerator();
        // generates a map of size 20x20 with a large constraint (generates a tightly-packed map)
        MapTile[,] tiles3 = perlinGen.MapGen(maxX, maxY, 5.0f);
        GameObject playerAI = Instantiate(player, new Vector3(0, 0, -.5f), Quaternion.identity);

        foreach (MapTile m in tiles1)
        {

            Vector3 pos = new Vector3(m.X + transform.position.x, m.Y + transform.position.y);

            if (m.IsStart)
            {
                Instantiate(startTile, pos, Quaternion.identity);
                playerAI.transform.position = new Vector3(m.X + transform.position.x, m.Y + transform.position.y, -.5f);
                playerAI.GetComponent<playerScript>().map = tiles1;
                playerAI.GetComponent<playerScript>().start = new Vector2(m.X, m.Y);
            }
            else if (m.IsGoal)
            {
                Instantiate(goalTile, pos, Quaternion.identity);
                playerAI.GetComponent<playerScript>().goal = new Vector2(m.X, m.Y);
            }
            else if (m.Walkable)
            {
                Instantiate(yesTile, pos, Quaternion.identity);
            }
            else if (!m.Walkable)
            {
                Instantiate(noTile, pos, Quaternion.identity);
            }
            
        }
        
    }
}

public class node
{
    MapTile m;
    public node parent;
    int f, g, h;

    public List<MapTile> adjacents(MapTile[,] map, int xMax, int yMax)
    {
        List<MapTile> adjTiles = new List<MapTile>();
        if (m.X - 1 >= 0)
        {
            adjTiles.Add(map[m.X - 1, m.Y]);
        }
        if (m.X + 1 <= xMax)
        {
            adjTiles.Add(map[m.X + 1, m.Y]);
        }
        if (m.Y - 1 >= 0)
        {
            adjTiles.Add(map[m.X, m.Y - 1]);
        }
        if (m.Y + 1 <= yMax)
        {
            adjTiles.Add(map[m.X, m.Y + 1]);
        }

        return adjTiles;
    }

    public node(MapTile m, int g, int h)
    {

    }

    public node(MapTile m, node Parent, int g, int h)
    {
        this.m = m;
        this.g = g;
        this.h = h;
        this.parent = Parent;
    }


}