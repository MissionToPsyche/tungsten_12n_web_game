using UnityEngine;

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
    public BuildingComponents.ResourceType resourceType;
    //Deposit info
    public Vector3 depositSize { get; set; }
    public int depositAmount { get; set; }
    public int originalAmount;
    public bool isDepleted;
    protected float xSize;
    protected float ySize;
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
        xSize = Random.Range(0.5f, 5f);
        ySize = Random.Range(0.5f, 5f);

        Vector3 valToReturn = new Vector3(xSize, ySize, 1);
        return valToReturn;
    }
    public BuildingComponents.ResourceType GetResourceType(){
        return resourceType;
    }
    public int GetDepositAmount(){
        return depositAmount;
    }
    public void ReduceDepositAmount(int minedAmount){
        depositAmount -= minedAmount;
    }
    public Vector3 ReduceSize(int AfterAmount, int changeAmt){
        Debug.LogWarning("Before xSize: " + xSize  + "\tySize: " + ySize + "\tDepositSize: " + depositSize);
        float percentDecrease = (float)AfterAmount / originalAmount;
        float splitDecrease = 1 - (1 - percentDecrease) / 2;
        xSize *= splitDecrease;
        ySize *= splitDecrease;
        Debug.LogWarning("After xSize: " + xSize + "\tsplitDecrease: " + splitDecrease + "\tySize: " + ySize);
        return new Vector3(xSize, ySize, 1);
    }
}

//----------------------<COMMON RESOURCES>----------------------//
public class Iron : Resource
{
    public Iron(Vector2 pos) : base(pos)
    {
        Name = "Iron";
        resourceType = BuildingComponents.ResourceType.Iron;
        Color = Color.red;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 100;
        originalAmount = depositAmount;
        rarity = Rarity.Common;
    }
}

public class Nickel : Resource
{
    public Nickel(Vector2 pos) : base(pos)
    {
        Name = "Nickel";
        resourceType = BuildingComponents.ResourceType.Nickel;
        Color = Color.gray;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 100;
        originalAmount = depositAmount;
        rarity = Rarity.Common;
    }
}

public class Cobalt : Resource
{
    public Cobalt(Vector2 pos) : base(pos)
    {
        Name = "Cobalt";
        resourceType = BuildingComponents.ResourceType.Cobalt;
        Color = Color.white;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 100;
        originalAmount = depositAmount;
        rarity = Rarity.Common;
    }
}

//----------------------<COMMODITY RESOURCES>----------------------//
public class Platinum : Resource
{
    public Platinum(Vector2 pos) : base(pos)
    {
        Name = "Platinum";
        resourceType = BuildingComponents.ResourceType.Platinum;
        Color = Color.cyan;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 50;
        originalAmount = depositAmount;
        rarity = Rarity.Commodity;
    }
}

public class Gold : Resource
{
    public Gold(Vector2 pos) : base(pos)
    {
        Name = "Gold";
        resourceType = BuildingComponents.ResourceType.Gold;
        Color = Color.yellow;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 50;
        originalAmount = depositAmount;
        rarity = Rarity.Commodity;
    }
}

public class Technetium : Resource
{
    public Technetium(Vector2 pos) : base(pos)
    {
        Name = "Technetium";
        resourceType = BuildingComponents.ResourceType.Technitium;
        Color = Color.green;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 50;
        originalAmount = depositAmount;
        rarity = Rarity.Commodity;
    }
}

//----------------------<EXOTIC RESOURCES>----------------------//
public class Tungsten : Resource
{
    public Tungsten(Vector2 pos) : base(pos)
    {
        Name = "Tungsten";
        resourceType = BuildingComponents.ResourceType.Tungsten;
        Color = Color.blue;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 25;
        originalAmount = depositAmount;
        rarity = Rarity.Exotic;
    }
}

public class Iridium : Resource
{
    public Iridium(Vector2 pos) : base(pos)
    {
        Name = "Iridium";
        resourceType = BuildingComponents.ResourceType.Iridium;
        Color = Color.black;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 25;
        originalAmount = depositAmount;
        rarity = Rarity.Exotic;
    }
}