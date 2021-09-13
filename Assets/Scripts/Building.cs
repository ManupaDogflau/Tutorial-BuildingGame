using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private HealthSystem healthSystem;
    private BuildingTypeSO buildingType;
    private Transform buildingDemolishbtn;
    private Transform buildingRepairBtn;

    private void Awake()
    {
        buildingDemolishbtn = transform.Find("BuildingDemolishBtn");
        buildingRepairBtn = transform.Find("BuildingRepairBtn");
        if (buildingDemolishbtn != null)
        {
            buildingDemolishbtn.gameObject.SetActive(false);
        }
        HideBuildingRepairBtn();
    }
    private void Start()
    {
        buildingType=GetComponent<BuildingTyprHolder>().buildingType;
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.SetHealthAmountMax(buildingType.HealthAmountMax,true);
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);
        ShowBuildingRepairBtn();
    }

    private void HealthSystem_OnHealed(object sender, EventArgs e)
    {
        
        if (healthSystem.IsFullHealth())
        {
            HideBuildingRepairBtn();
        }
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
        Instantiate(Resources.Load("pfBuildingDestroyedParticles"), transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnMouseEnter()
    {
        if (buildingDemolishbtn != null)
        {
            buildingDemolishbtn.gameObject.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (buildingDemolishbtn != null)
        {
            buildingDemolishbtn.gameObject.SetActive(false);
        }
    }

    private void ShowBuildingRepairBtn()
    {
        if (buildingRepairBtn != null)
        {
            buildingRepairBtn.gameObject.SetActive(true);
        }
    }
    private void HideBuildingRepairBtn()
    {
        if (buildingRepairBtn != null)
        {
            buildingRepairBtn.gameObject.SetActive(false);
        }
    }
}
