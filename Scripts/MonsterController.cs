using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Searcher.SearcherWindow.Alignment;


public class MonsterController: MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    float moveSpeed = 20f;
    public int health;
    private Enums.Direction curDirection;
    public GameController gameController;
    public Queue<Enums.Direction> route;

    public Vector2 lastDirectionalChange;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        health = 150;
        lastDirectionalChange = this.transform.position;
      //  route = new Queue<Enums.Direction>(gameController.enemyRoute);
        Debug.Log("Route count = " + route.Count);
        moveMonster(route.Dequeue());
    }

    // Update is called once per frame
    void Update()
    {
       
       

    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(this.transform.position, lastDirectionalChange) >= 10)
        {
            Debug.Log("triggered");
            moveMonster(route.Dequeue());
            lastDirectionalChange = this.transform.position;
        }
       

    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        Destroy(collision.gameObject);
        health -= 10;
        Debug.Log("Health = " + health);

        if (health < 0)
            Destroy(this.gameObject);
    }
    

    private void moveMonster(Enums.Direction dir) 
    {
        if (curDirection == Enums.Direction.NORTH)
        {
            rigidbody2d.velocity = new Vector2(0, moveSpeed);
            curDirection = dir;

        }
        else if (curDirection == Enums.Direction.SOUTH)
        {
            rigidbody2d.velocity = new Vector2(0, -moveSpeed);
            curDirection = dir;

        }
        else if (curDirection == Enums.Direction.WEST)
        {
            rigidbody2d.velocity = new Vector2(moveSpeed, 0);
            curDirection = dir;
        }
        else
        {
            rigidbody2d.velocity = new Vector2(-moveSpeed, 0);
            curDirection = dir;

        }
        return;
    }
    public float getMoveSpeed() { return moveSpeed; }
    public Enums.Direction getCurDirection() { return curDirection; }
}
