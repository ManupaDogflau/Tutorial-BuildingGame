using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDemolishBtn : MonoBehaviour
{
    [SerializeField]private Building building;
    // Start is called before the first frame update
    private void Awake()
    {
        transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            BuildingTypeSO buildingType=building.GetComponent<BuildingTyprHolder>().buildingType;
            foreach (ResourceAmount resourceAmount in buildingType.construccionResourcesCostArray)
            {
                ResourceManager.Instance.AddResource(resourceAmount.resourceType, Mathf.FloorToInt(resourceAmount.amount * .6f));
            }
            SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
            Instantiate(Resources.Load("pfBuildingDestroyedParticles"), transform.position, Quaternion.identity);
            Destroy(building.gameObject);
        });
    }
}
