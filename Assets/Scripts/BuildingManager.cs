using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }
    private BuildingTypeSO activeBuildingType;
    private BuildingTypeListSO buildingTypeList;
    private Camera mainCamera;

    [SerializeField] private Building HQbuilding;

    public event EventHandler<OnActiveBuildingTypeButtonChangedEventArgs> OnActiveBuildingTypeButtonChanged;

    public class OnActiveBuildingTypeButtonChangedEventArgs: EventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }

    private void Awake()
    {
        Instance =this;
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
    }
    void Start()
    {
        mainCamera = Camera.main;
        HQbuilding.GetComponent<HealthSystem>().OnDied += HQ_OnDied;
    }

    private void HQ_OnDied(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
        GameOverUI.Instance.Show();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)&& !EventSystem.current.IsPointerOverGameObject())
        {
            if (activeBuildingType != null){
                if (CanSpanwnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string errorMesage))
                {
                    if (ResourceManager.Instance.CanAfford(activeBuildingType.construccionResourcesCostArray))
                    {
                        //Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
                        BuildingConstruction.Create(UtilsClass.GetMouseWorldPosition(),activeBuildingType);
                        ResourceManager.Instance.SpendResources(activeBuildingType.construccionResourcesCostArray);
                        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
                    }
                    else
                    {
                        TooltipUI.Instance.Show("Cant afford " + activeBuildingType.GetConstuctionResourceCostString(), new TooltipUI.TooltipTimer { timer = 2f });
                    }
                }
                else
                {
                    TooltipUI.Instance.Show(errorMesage, new TooltipUI.TooltipTimer { timer = 2f });
                }
               
            }
        }
        

    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeButtonChanged?.Invoke(this, new OnActiveBuildingTypeButtonChangedEventArgs{ activeBuildingType = activeBuildingType });
    }

    public BuildingTypeSO GetActiveBuildingTypr()
    {
        return activeBuildingType;
    }

    private bool CanSpanwnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage)
    {
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();
        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3) boxCollider2D.offset, boxCollider2D.size, 0);

        bool isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear)
        {
            errorMessage = "Area is not clear!";
            return false;
        }

        collider2DArray = Physics2D.OverlapCircleAll(position + (Vector3)boxCollider2D.offset, buildingType.minConstruccionRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            BuildingTyprHolder buildingTyprHolder = collider2D.GetComponent<BuildingTyprHolder>();
            if (buildingTyprHolder != null)
            {
                if(buildingTyprHolder.buildingType== buildingType)
                {
                    errorMessage = "Too close to another building of the same type!";
                    return false;
                }
            }
        }
        if (buildingType.hasResourceGeneratorData)
        {
            ResourceGeneratorData resourceGeneratorData = buildingType.resourceGeneratorData;
            int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmout(resourceGeneratorData, position);
            if (nearbyResourceAmount == 0)
            {
                errorMessage = "There are no nearby Resource Nodes";
                return false;
            }
        }
       

        float maxConstruccionRadius = 25f;
        collider2DArray = Physics2D.OverlapCircleAll(position + (Vector3)boxCollider2D.offset, maxConstruccionRadius);

       
        foreach (Collider2D collider2D in collider2DArray)
        {
            BuildingTyprHolder buildingTyprHolder = collider2D.GetComponent<BuildingTyprHolder>();
            if (buildingTyprHolder != null)
            {
                errorMessage = "";
                return true;
            }
        }
        errorMessage = "Too far for another building";
        return false;
    }

    public Building GetHQbuilding()
    {
        return HQbuilding;
    }
}
