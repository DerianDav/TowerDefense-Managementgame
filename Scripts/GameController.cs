using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{

    public GameObject enemySpawnPoint;
    public GameObject enemyFinishPoint;
    float spawnPointX;
    float spawnPointY;

    public GameObject enemy;
    public float enemiesLeftToSpawn;
    public float secondsBetweenSpawn = 1;
    public float towerDefenseScale = 10;
    public float resourceManagementScale = 1;
    public Tilemap towerDefenseMainMap;
    public Tile grassTile;
    public Tile pathTile;

    public Queue<Enums.Direction> enemyRoute;
    // Start is called before the first frame update
    void Start()
    {
        spawnPointX = enemySpawnPoint.transform.position.x;
        spawnPointY = enemySpawnPoint.transform.position.y;
        enemiesLeftToSpawn = 10;
        enemyRoute = new Queue<Enums.Direction>();

        calcEnemyRoute();
        Debug.Log(enemyRoute.Count + " = count");
        StartCoroutine(spawnEnemies());
        Debug.Log(enemyRoute.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3Int pos = new Vector3Int(3, 3);
        if(towerDefenseMainMap.GetTile(pos) == grassTile)
            towerDefenseMainMap.SetTile(pos, pathTile);
        else
            towerDefenseMainMap.SetTile(pos,grassTile);
        towerDefenseMainMap.RefreshTile(pos);
    }

    IEnumerator spawnEnemies() 
    {
        while (true)
        {
            if (enemiesLeftToSpawn > 0)
            {
                GameObject enemyOb = Instantiate(enemy, new Vector2(spawnPointX, spawnPointY), Quaternion.identity);
                enemiesLeftToSpawn--;
                enemyOb.GetComponent<MonsterController>().route = new Queue<Enums.Direction>(enemyRoute) ;

            }
            yield return new WaitForSeconds(secondsBetweenSpawn);
        }
    }

    private void calcEnemyRoute() 
    {
        Vector3Int startingPoint = towerDefenseMainMap.WorldToCell(enemySpawnPoint.transform.position);
        Vector3Int endPoint = towerDefenseMainMap.WorldToCell(enemyFinishPoint.transform.position);
        Vector3Int prevLoc = startingPoint;
        Vector3Int curTileLoc = startingPoint;
        int count = 0;
        while (curTileLoc != endPoint)
        {
            //Debug.Log(curTileLoc);
            if (count > 30) { Debug.Log("return"); return; }
            if ((new Vector3Int(curTileLoc.x, curTileLoc.y + 1) != prevLoc) && towerDefenseMainMap.GetTile(new Vector3Int(curTileLoc.x, curTileLoc.y + 1)) == pathTile)
            {
                Debug.Log("N " + count);
                enemyRoute.Enqueue(Enums.Direction.NORTH);
                prevLoc = curTileLoc;
                curTileLoc.y += 1;
            }
            else if ((new Vector3Int(curTileLoc.x, curTileLoc.y - 1) != prevLoc) && towerDefenseMainMap.GetTile(new Vector3Int(curTileLoc.x, curTileLoc.y - 1)) == pathTile)
            {
                Debug.Log("S " + count);
                enemyRoute.Enqueue(Enums.Direction.SOUTH);
                prevLoc = curTileLoc;
                curTileLoc.y -= 1;
            }
            else if ((new Vector3Int(curTileLoc.x + 1, curTileLoc.y) != prevLoc) && towerDefenseMainMap.GetTile(new Vector3Int(curTileLoc.x + 1, curTileLoc.y)) == pathTile)
            {
                Debug.Log("W " + count);
                enemyRoute.Enqueue(Enums.Direction.WEST);
                prevLoc = curTileLoc;
                curTileLoc.x += 1;
            }
            else
            {
                Debug.Log("E " + count);
                enemyRoute.Enqueue(Enums.Direction.EAST);
                prevLoc = curTileLoc;
                curTileLoc.x -= 1;
            }
            count++;
        }
    }
}
