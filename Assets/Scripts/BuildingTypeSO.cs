using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/BuildingTypes")]
public class BuildingTypeSO : ScriptableObject
{

    public string nameString;
    public Transform prefab;
    public bool hasResourceGeneratorData;
    public ResourceGeneratorData resourceGeneratorData;
    public Sprite sprite;
    public float minConstruccionRadius;
    public ResourceAmount[] construccionResourcesCostArray;
    public int HealthAmountMax;
    public float constructionTimerMax;

    public string GetConstuctionResourceCostString()
    {
        string str = "";
        foreach (ResourceAmount resourceAmount in construccionResourcesCostArray)
        {
            str +="<color=#" +resourceAmount.resourceType.colorHex+ ">" + resourceAmount.resourceType.nameShort + ": " + resourceAmount.amount +"</color> ";
        }
        return str;
    }
}
