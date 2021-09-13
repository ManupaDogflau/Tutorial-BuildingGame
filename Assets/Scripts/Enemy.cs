using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public static Enemy Create(Vector3 position)
    {
        Transform enemyTransform=Instantiate(GameAssets.Instance.enemyPrefab, new Vector3(position.x,position.y,0), Quaternion.identity);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }
    private Transform targetTransform;
    private Rigidbody2D rigidbody2d;
    private float LookforTargetTimerMax = .2f;
    private float LookforTargetTimer;
    private HealthSystem healthSystem;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        if (BuildingManager.Instance.GetHQbuilding() != null)
        {
            targetTransform = BuildingManager.Instance.GetHQbuilding().transform;
        }
        LookforTargetTimer = Random.Range(0f, LookforTargetTimerMax);
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
    }

    private void Update()
    {
        HandleMovement();
        HandleTargettinng();

        
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        Instantiate(GameAssets.Instance.enemyDiesParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Building building=collision.gameObject.GetComponent<Building>();
        if (building != null)
        {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(10);
            this.healthSystem.Damage(this.healthSystem.GetHealthAmountMax());
        }
    }

    private void HandleMovement()
    {
        if (targetTransform != null)
        {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;
            float moveSpeed = 6f;
            rigidbody2d.velocity = moveDir * moveSpeed;
        }
        else
        {
            rigidbody2d.velocity = Vector2.zero;
        }
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
    private void LookForTargets()
    {
        float targetMaxRadius = 10f;
        Collider2D[] colliderArray=Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach(Collider2D collider2D in colliderArray)
        {
            Building building = collider2D.GetComponent<Building>();
            if (building != null)
            {
                if (targetTransform == null)
                {
                    targetTransform = building.transform;
                }
                else
                {
                    if (Vector3.Distance(transform.position, building.transform.position) < Vector3.Distance(transform.position, targetTransform.position))
                    {
                        targetTransform = building.transform;
                    }
                }
            }
        }
        if (targetTransform == null)
        {
            if (BuildingManager.Instance.GetHQbuilding() != null)
            {
                targetTransform = BuildingManager.Instance.GetHQbuilding().transform;
            }
        }
    }
}
