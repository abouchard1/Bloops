﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class InitMap : MonoBehaviour
{
/*  
 * Attribut fixe qui ne doivent pas bouger         
 */
    private int columnLength = 17;
    private int rowLength = 9;
    private float xStart = -8.5f;
    private float yStart = 3.5f;
    private float x = 1f;
    private float y = 1f;

    public GameObject prefab_wall;
    public GameObject prefab_character;
    public BoxCollider2D prefab_finisher;

    public class GameObjectPosition
    {
        public int x;
        public int y;

        public GameObjectPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    private List<GameObject> InstanceReferences = new List<GameObject>() { };
    private List<GameObjectPosition> gameObjectPositions = new List<GameObjectPosition>() { };
    /*
     * valeur à set avant le lancement de la scène
     */

    public string levelName = "template.txt";
    private Map map;


    void Start()
    {
        levelName = $"niveau_{GameSceneInfo.level}.json";
        map = Utils.LoadJsonMap(levelName);
        LoadMap();
    }

    private void InstantiateCharacter(int xPos, int yPos)
    {
        GameObject character = Instantiate(prefab_character, new Vector3(xStart + (x * xPos), yStart + (y * -yPos)), Quaternion.identity);
        character.transform.localScale += new Vector3(-0.2f, -0.2f, -0.01f);
        InstanceReferences.Add(character);
    }


    private void LoadMap()
    {
        foreach (Bloc bloc in map.blocs)
        {
            int xPos = bloc.pos_x;
            int yPos = bloc.pos_y;

            gameObjectPositions.Add(new GameObjectPosition(xPos, yPos));
            if (bloc.type == "character")
            {
                InstantiateCharacter(xPos, yPos);
            }
            else if (bloc.type == "finisher")
            {
                Instantiate(prefab_finisher, new Vector3(xStart + (x * xPos), yStart + (y * -yPos)), Quaternion.identity);
            }
            else if (bloc.type == "wall")
            {
                InstanceReferences.Add(Instantiate(prefab_wall, new Vector3(xStart + (x * xPos), yStart + (y * -yPos)), Quaternion.identity));
            }
        }
    }
}