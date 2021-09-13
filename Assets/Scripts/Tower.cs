using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private float shootTimer;
    [SerializeField] private float shootTimerMax;
    private float LookforTargetTimer;
    private float LookforTargetTimerMax=.2f;
    private Enemy targetEnemy;
    private Vector3 proyectileSpawnPosition;

    private void Awake()
    {
        proyectileSpawnPosition = transform.Find("ProyectileSpawnPosition").transform.position;
    }

    private void Update()
    {
        HandleTargettinng();
        HandleShooting();
    }
    private void HandleTargettinng()
    {
        LookforTargetTimer -= Time.deltaTime;
        if (LookforTargetTimer <= 0f)
        {
            LookforTargetTimer += LookforTargetTimerMax;
            LookForTargets();
        }
    }

    private void HandleShooting()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            shootTimer += shootTimerMax;
            if (targetEnemy != null)
            {
                ArrowProyectile.Create(proyectileSpawnPosition, targetEnemy);
            }
        }
    }

    private void LookForTargets()
    {
        float targetMaxRadius = 20f;
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in colliderArray)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                }
                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, targetEnemy.transform.position))
                    {
                        targetEnemy = enemy;
                    }
                }
            }
        }
    }
}
