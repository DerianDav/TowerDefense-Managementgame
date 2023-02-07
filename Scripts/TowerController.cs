using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerController : MonoBehaviour
{
    public GameController gameController;
    public GameObject LaserFab;
    private GameObject LaserShot;
    private float towerCooldownTime = 0.2f; // the time between shots 
    public float range = 25f;
    public GameObject target;
    public string enemyTag = "Enemy";


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("getTarget", 0f, 0.3f);
        StartCoroutine(towerShoot());


    }

    private void Update()
    {
       
    }

    void getTarget() 
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        float minDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        { 
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (minDistance > distanceToEnemy)
            {
                minDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

        }

        if (nearestEnemy != null && minDistance <= range)
            target = nearestEnemy;
        else
            target = null;
    }
    IEnumerator towerShoot()
    {
        while (true)
        {
            if (target != null)
            {
                LaserShot = Instantiate(LaserFab, this.transform.position, Quaternion.identity);
                if (interceptionDirection(target.transform.position, this.transform.position, target.GetComponent<Rigidbody2D>().velocity, LaserShot.GetComponent<ShotController>().getBaseSpeed(), out var direction))
                {
                    LaserShot.GetComponent<Rigidbody2D>().velocity = direction * LaserShot.GetComponent<ShotController>().getBaseSpeed();
                }
                else
                    LaserShot.GetComponent<Rigidbody2D>().velocity = (target.transform.position - this.transform.position).normalized * LaserShot.GetComponent<ShotController>().getBaseSpeed();
            }
            yield return new WaitForSeconds(towerCooldownTime);
        }
        
    }





    //Math for Tower Prediction

    public bool interceptionDirection(Vector2 a, Vector2 b, Vector2 vA, float sB, out Vector2 result)
    {
        var aToB = b - a;
        var dC = aToB.magnitude;
        var alpha = Vector2.Angle(aToB, vA) * Mathf.Deg2Rad;
        var sA = vA.magnitude;
        var r = sA / sB;
        if (solveQuadratic(1 - r * r, 2 * r * dC * Mathf.Cos(alpha), -(dC * dC), out var root1, out var root2) == 0)
        {
            result = Vector2.zero;
            return false;
        }

        var dA = Mathf.Max(root1, root2);
        var t = dA / sB;
        var c = a + vA * t;
        result = (c - b).normalized;
        return true;

    }

    private int solveQuadratic(float a, float b, float c, out float root1, out float root2)
    {
        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            root1 = -1;
            root2 = -1;
        }

        root1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
        root2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);

        return discriminant > 0 ? 2 : 1;
    }


    //Draws range disctance in Unity
    void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

