using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProyectile : MonoBehaviour
{

    public static ArrowProyectile Create(Vector3 position, Enemy enemy)
    {
        Transform ArrowProyectile = Resources.Load<Transform>("ArrowProyectile");
        Transform arrowTransform = Instantiate(ArrowProyectile, position, Quaternion.identity);
        ArrowProyectile arrowProyectile = arrowTransform.GetComponent<ArrowProyectile>();
        arrowProyectile.SetTarget(enemy);

        return arrowProyectile;
    }
    private Enemy targetEnemy;
    private Vector3 LastDirection;
    private float timeToDie = 2f;
    private void SetTarget(Enemy targetEnemy) 
    {
        this.targetEnemy = targetEnemy;
    }

    private void Update()
    {
        Vector3 moveDir;
        if (targetEnemy != null)
        {
            moveDir= (targetEnemy.transform.position - transform.position).normalized;
            LastDirection = moveDir;
        }
        else
        {
            moveDir = LastDirection;
        }
        
        float moveSpeed = 20f;
        transform.position += moveDir * Time.deltaTime * moveSpeed;
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDir));
        timeToDie -= Time.deltaTime;

        if(timeToDie< 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy=collision.GetComponent<Enemy>();
        if (enemy!=null)
        {
            int damageAmount = 10;
            enemy.GetComponent<HealthSystem>().Damage(damageAmount);
            Destroy(gameObject);
        }
    }
}
