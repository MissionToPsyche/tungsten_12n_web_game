using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Rarity
{
    Common, Commodity, Exotic
}
/* ! Resource ID is the position of the class in this file starting from Iron to Iridium
*   Iron = 1
*   Nickel = 2
*   Silver = 3
*   Platinum = 4
*   Gold = 5
*   Technitium = 6
*   Tungsten = 7
*   Iridium = 8
*/
public class Resource
{
    //Resource info
    public string Name { get; set; }
    public Vector2 Position { get; set; }
    public Color Color { get; set; }
    public Rarity rarity;
    protected int resourceID;
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
    public int GetResourceID(){
        return resourceID;
    }
}

//----------------------<COMMON RESOURCES>----------------------//
public class Iron : Resource
{
    public Iron(Vector2 pos) : base(pos)
    {
        Name = "Iron";
        resourceID = 1;
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
        resourceID = 2;
        Color = Color.gray;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 100;
        rarity = Rarity.Common;
    }
}

public class Silver : Resource
{
    public Silver(Vector2 pos) : base(pos)
    {
        Name = "Silver";
        resourceID = 3;
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
        resourceID = 4;
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
        resourceID = 5;
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
        resourceID = 6;
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
        resourceID = 7;
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
        resourceID = 8;
        Color = Color.black;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 25;
        rarity = Rarity.Exotic;
    }
}