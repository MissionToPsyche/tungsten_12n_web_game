using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Rarity
{
    Common, Commodity, Exotic
}

public class Resource
{
    //Resource info
    public string Name { get; set; }
    public Vector2 Position { get; set; }
    public Color Color { get; set; }
    public Rarity rarity;

    //Deposit info
    public Vector3 depositSize { get; set; }
    public int depositAmount { get; set; }
    public bool isDepleted;

    //public GameObject prefab; //for when each resource has its own prefab

    public Resource(Vector2 pos)
    {
        Position = pos;
        depositSize = randomizeSize();
        //amount of resources in an ore deposit is related to its size
        depositAmount = (int)(depositSize.x + depositSize.y) * 25;
    }

    protected Vector3 randomizeSize()
    {
        float x = Random.Range(0.5f, 5f);
        float y = Random.Range(0.5f, 5f);

        Vector3 valToReturn = new Vector3(x, y, y);
        return valToReturn;
    }

}

//----------------------<COMMON RESOURCES>----------------------//
public class Iron : Resource
{
    public Iron(Vector2 pos) : base(pos)
    {
        Name = "Iron";
        Color = Color.red;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 100;
        rarity = Rarity.Common;
    }
}

public class Nickel : Resource
{
    public Nickel(Vector2 pos) : base(pos)
    {
        Name = "Nickel";
        Color = Color.gray;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 100;
        rarity = Rarity.Common;
    }
}

public class Cobalt : Resource
{
    public Cobalt(Vector2 pos) : base(pos)
    {
        Name = "Cobalt";
        Color = Color.white;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 100;
        rarity = Rarity.Common;
    }
}

//----------------------<COMMODITY RESOURCES>----------------------//
public class Platinum : Resource
{
    public Platinum(Vector2 pos) : base(pos)
    {
        Name = "Platinum";
        Color = Color.cyan;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 50;
        rarity = Rarity.Commodity;
    }
}

public class Gold : Resource
{
    public Gold(Vector2 pos) : base(pos)
    {
        Name = "Gold";
        Color = Color.yellow;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 50;
        rarity = Rarity.Commodity;
    }
}

public class Technetium : Resource
{
    public Technetium(Vector2 pos) : base(pos)
    {
        Name = "Technetium";
        Color = Color.green;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 50;
        rarity = Rarity.Commodity;
    }
}

//----------------------<EXOTIC RESOURCES>----------------------//
public class Tungsten : Resource
{
    public Tungsten(Vector2 pos) : base(pos)
    {
        Name = "Tungsten";
        Color = Color.blue;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 25;
        rarity = Rarity.Exotic;
    }
}

public class Iridium : Resource
{
    public Iridium(Vector2 pos) : base(pos)
    {
        Name = "Iridium";
        Color = Color.black;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 25;
        rarity = Rarity.Exotic;
    }
}