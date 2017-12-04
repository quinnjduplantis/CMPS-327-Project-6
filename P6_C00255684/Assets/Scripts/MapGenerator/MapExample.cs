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
        MapTile[,] tiles1 = primGen.MapGen(maxX, maxY, 0.3f);
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
                playerAI.GetComponent<playerScript>().xMax = maxX;
                playerAI.GetComponent<playerScript>().yMax = maxY;
                playerAI.GetComponent<playerScript>().start = m;

            }
            else if (m.IsGoal)
            {
                Instantiate(goalTile, pos, Quaternion.identity);
                playerAI.GetComponent<playerScript>().goal = m;
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

