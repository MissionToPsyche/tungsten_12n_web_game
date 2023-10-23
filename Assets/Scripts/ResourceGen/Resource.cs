using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class Resource
{
    public Vector2 Position { get; set; }
    public string Name { get; set; }
    public Color Color { get; set; }
    public int depositAmount;
    public float depositSize;
    public GameObject prefab; //for when each resource has its own prefab
    public bool isDepleted;

    // Constructor to initialize properties
    public Resource(Vector2 pos)
    {
        Position = pos;
        depositSize = randomizeSize();
        //amount of resources in an ore deposit is related to its size
        depositAmount = (int)(depositSize * 100);
    }

    protected float randomizeSize()
    {
        return Random.Range(0.5f, 3.5f);
    }

}


public class Iron : Resource
{

    public Iron(Vector2 pos) : base(pos)
    {
        Name = "Iron";
        Color = Color.red;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize * 100);
    }

}

public class Nickel : Resource
{
    public int NickelValue { get; set; }

    public Nickel(Vector2 pos) : base(pos)
    {
        Name = "Nickel";
        Color = Color.gray;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize * 100);
    }

}