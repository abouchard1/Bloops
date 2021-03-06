﻿using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bloc
{
    //MUR, PERSONNAGE, PORTAIL, PIEGE
    public string type;

    public int pos_x;
    public int pos_y;
    public int rotation = 0;

    public Bloc(string type, int pos_x, int pos_y, int rotation)
    {
        this.type = type;
        this.pos_x = pos_x;
        this.pos_y = pos_y;
        this.rotation = rotation;
    } 
}

[System.Serializable]
public class Map
{
    public string name;
    public string world;
    public List<string> rules;
    public List<Bloc> blocs;
    public Bloc character;
    public Bloc portal;

    public Map()
    {
        this.name = "";
        this.world = "";
        this.rules = new List<string>();
        this.blocs = new List<Bloc>();
    }
}

public class Utils
{
    public static Map LoadJsonMap(string fileName)
    {
        string text;

        if(Application.isEditor)
        {
            text = System.IO.File.ReadAllText($@"Assets\Resources\Map\{fileName}.json");
        } else
        {
            TextAsset textAsset = Resources.Load<TextAsset>($@"Map/{fileName}");
            text = textAsset.text;
        }
        return JsonUtility.FromJson<Map>(text);
    }

    public static void SaveJsonMap(string fileName, Map map)
    {
        string text = JsonUtility.ToJson(map);

        System.IO.File.WriteAllText($@"Assets\Resources\Map\{fileName}", text);
    }
}
