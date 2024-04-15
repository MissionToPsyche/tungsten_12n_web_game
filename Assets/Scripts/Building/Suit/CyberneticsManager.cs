using UnityEngine;
using System.Collections;

public class CyberneticsManager: MonoBehaviour
{
    public static CyberneticsManager Instance { get; private set; }
    [Header("Events")]
    [SerializeField] private IntEvent UpdateCyberUI;

    // Not for display
    private Cybernetics cybernetics;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        cybernetics = new();
    }

    private IEnumerator ChargeCoroutine(){
        while(true){
            if(cybernetics.GetCurrentCharge() < cybernetics.GetCurrentMaxCharge()){
                cybernetics.IncrementCharge();
                UpdateCyberUI.Raise(cybernetics.GetCurrentCharge());
            }
            //yield return new WaitForSeconds(cybernetics.GetCurrentWaitAmount());
            yield return new WaitForSeconds(2f);
        }
    }
    
    // Events
    public void OnTechUpEvent(packet.TechUpPacket packet){
        if(packet.building == BuildingComponents.BuildingType.Cybernetics){
            cybernetics.UpdateTechTier();
        }
    }

    public bool HasCyberneticCharge(){
        if(cybernetics.IsBuilt()){
            return cybernetics.HasCharge();
        }else{
            return false;
        }
    }
    public void SetCyberneticsBuilt(){
        cybernetics.SetBuilt();
        StartCoroutine(ChargeCoroutine());
    }
    public void UseCharge(){
        cybernetics.UseCharge();
    }
    // API
    public bool IsBuilt(){
        return cybernetics.IsBuilt();
    }
}