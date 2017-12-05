﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;
// includes the MapGen namespace methods
using MapGen;
using UnityEngine.SceneManagement;


public class MapExample : MonoBehaviour
{
    [SerializeField]
    GameObject yesTile, noTile, startTile, goalTile, player, enemy1, enemy2;

    [SerializeField]
    int maxX, maxY;

    public MapTile[,] map;
    List<MapTile> walkables;

	void Awake ()
    {
        PrimGenerator primGen = new PrimGenerator();
        // generate a map of size 20x20 with no extra walls removed
        MapTile[,] tiles1 = primGen.MapGen(maxX, maxY, 0.3f);
        PerlinGenerator perlinGen = new PerlinGenerator();
        // generates a map of size 20x20 with a large constraint (generates a tightly-packed map)
        map = perlinGen.MapGen(maxX, maxY, 5.0f);
        GameObject playerAI = Instantiate(player, new Vector3(0, 0, -.5f), Quaternion.identity);
        GameObject enemyAI1 = Instantiate(enemy1, new Vector3(0, 0, -.5f), Quaternion.identity);

        playerAI.GetComponent<playerScript>().map = map;
        //enemy1.GetComponent<enemyScript>().map = tiles3;
        walkables = new List<MapTile>();

        foreach (MapTile m in map)
        {
            if (m.Walkable)
            {
                walkables.Add(m);
            }
        }

        foreach (MapTile m in map)
        {

            Vector3 pos = new Vector3(m.X + transform.position.x, m.Y + transform.position.y);
            int walkableint = walkables.Count;
            System.Random rnd = new System.Random();
            int rndnum = rnd.Next(0, walkableint - 1);
            
            if (m.IsStart)
            {
                Instantiate(startTile, pos, Quaternion.identity);
                playerAI.transform.position = new Vector3(m.X + transform.position.x, m.Y + transform.position.y, -.5f);
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
                if (m == walkables[rndnum])
                {
                    enemyAI1.transform.position = new Vector3(m.X + transform.position.x, m.Y + transform.position.y, -.5f);
                }
            }
            else if (!m.Walkable)
            {
                Instantiate(noTile, pos, Quaternion.identity);
            }
            
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("menuScene");
        }
        

    }
}

