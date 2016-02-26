using UnityEngine;
using System.Collections;

public class BaddieSpawner : MonoBehaviour
{
    public float distanceBetweenBaddies;
    public float baddieSpeed;

    //baddies come out in cycles of X baddies and then the speed is increased
    public int baddiesPerCycle;
    public float baddieSpeedIncreasePerCycle;

    private int baddiesLeftInCurrentCycle;

    //The columns (x-axis) that projectiles should progress up
    public Transform[] spawnLanes;

    //The row (y-axis) where projectiles initialy spawn
    public Transform projectileSpawnRow;

    //Baddie prefab
    public GameObject baddie;

    //Indicator prefab
    public GameObject indicatorPrefab;

    public Player.PlayerRegion playerRegion;

    //In order to accomodate changes in global projectile speed, this will indicate when the next set of projectiles should spawn
    private GameObject nextProjectileIndicator;

    private ObjectPool baddieObjectPool = new ObjectPool();

    // Use this for initialization
    void Start ()
    {
        if (spawnLanes.Length <= 1) throw new System.Exception("Not enough spawn lanes");

        baddieObjectPool.Initialize(20);
        baddieObjectPool.objectPrefab = baddie;
        baddieObjectPool.parentContainer = gameObject;

        baddiesLeftInCurrentCycle = baddiesPerCycle;

        nextProjectileIndicator = Instantiate(indicatorPrefab);
        nextProjectileIndicator.GetComponent<Rigidbody2D>().AddForce(Vector2.up * baddieSpeed);
        ResetProjectileIndicator();

        Resolver.Instance.GetController<EventHandler>().Register(Events.Game.PlayerDead, OnPlayerDead);
    }

    void OnDestroy()
    {
        Resolver.Instance.GetController<EventHandler>().UnRegister(Events.Game.PlayerDead, OnPlayerDead);
    }

    // Update is called once per frame
    void Update ()
    {
        if (nextProjectileIndicator.transform.position.y >= projectileSpawnRow.position.y)
        {
            ResetProjectileIndicator();
            SpawnNewProjectiles();
        }
	}

    private void ResetProjectileIndicator()
    {
        nextProjectileIndicator.transform.Translate(new Vector2(0, -distanceBetweenBaddies));  
    }

    private void SpawnNewProjectiles()
    {
        GameObject newBaddie = baddieObjectPool.InitNewObject();

        if (Random.value < 0.5f)
        {
            newBaddie.transform.position = new Vector2(spawnLanes[0].position.x, projectileSpawnRow.position.y);
            newBaddie.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            newBaddie.transform.position = new Vector2(spawnLanes[1].position.x, projectileSpawnRow.position.y);
            newBaddie.GetComponent<SpriteRenderer>().flipX = false;
        }

        newBaddie.GetComponent<Rigidbody2D>().AddForce(Vector2.up * baddieSpeed);
        newBaddie.GetComponent<Animator>().speed = 2.0f;

        --baddiesLeftInCurrentCycle;
        if (baddiesLeftInCurrentCycle <= 0)
        {
            baddiesLeftInCurrentCycle = baddiesPerCycle;
            baddieSpeed += baddieSpeedIncreasePerCycle;
            nextProjectileIndicator.GetComponent<Rigidbody2D>().AddForce(Vector2.up * baddieSpeedIncreasePerCycle);
        }
    }

    void OnPlayerDead(int id, object param)
    {
        Player.PlayerRegion deadPlayerRegion = (Player.PlayerRegion)param;

        //if it's in 2-tree mode and the sloth in the other tree dies, speed up this tree by 50%
        if (playerRegion != Player.PlayerRegion.Both && 
            playerRegion != deadPlayerRegion)
        {
            baddieSpeed += (float)(baddieSpeed * 0.5);
        }
        else
        {
            baddieSpeedIncreasePerCycle = 0.0f;
        }
    }

}
