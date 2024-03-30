public class ExtractorMining : AbstractExtractorMining
{
    Extractor ExtractorClass;
    void Start(){
        buildingType = BuildingComponents.BuildingType.Extractor;
        //QueryTechLevel(buildingType);
        DragAndDropExtractor.OnPlacementEvent += LinkToResource;
        gravityBody = GetComponent<GravityBody2D>();
        ExtractorClass = new();
        mineInterval = ExtractorClass.GetMineInterval();
        amountToMine = ExtractorClass.GetAmountToMine();
        baseBreakChance = ExtractorClass.GetBaseBreakChance();
    }

    void Update(){
        MineIfPlaced();
    }
}