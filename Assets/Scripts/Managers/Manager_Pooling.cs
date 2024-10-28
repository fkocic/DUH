using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Manager_Pooling : MonoBehaviour
{
    [Header("Player Bullets")]
    [SerializeField] List<Transform> playerBulletPool = new List<Transform>();
    [SerializeField] GameObject playerBulletPrefab;
    [SerializeField] int playerBulletPoolSize;
    [SerializeField] Transform bulletParent;

    [Header("Enemy Bullets")]
    [SerializeField] List<Transform> enemyBulletPool = new List<Transform>();
    [SerializeField] GameObject enemyBulletPrefab;
    [SerializeField] int enemyBulletPoolSize;

    [Header("Impacts")]
    [SerializeField] List<Transform> impactEffectPool = new List<Transform>();
    [SerializeField] GameObject impactPrefab;
    [SerializeField] int impactPoolSize;
    [SerializeField] Transform impactParent;
    int impactPoolCounter;

    [Header("Enemies")]
    [SerializeField] List<EnemyTemplate> allEnemies = new List<EnemyTemplate>();
    [SerializeField] List<GameObject> allEnemiesPrefabs = new List<GameObject>();
    Dictionary<EnemyTemplate, List<Transform>> enemies = new Dictionary<EnemyTemplate, List<Transform>>();
    [SerializeField] int enemyPoolSize;
    [SerializeField] Transform enemyParent;

    [Header("Enemy Particle Effects")]
    [SerializeField] GameObject enemySpawnPrefab;
    [SerializeField] GameObject enemyDiePrefab;
    [SerializeField] GameObject enemyDropPrefab;
    [SerializeField] GameObject explosionPrefab;
    Dictionary<particleType, List<Transform>> enemyParticles = new Dictionary<particleType, List<Transform>>();
    [SerializeField] int particlePoolSize;
    [SerializeField] Transform particleParent;
    int particleSpawnCounter, particleDieCounter, particleDropCounter, explosionCounter;

    public void SetupValues()
    {
        SpawnBullets();
        SpawnImpacts();
        SpawnEnemies();
        SpawnEnemyParticles();
    }

    #region Spawn

    private void SpawnBullets()
    {
        for (int i = 0; i < playerBulletPoolSize; i++)
        {
            playerBulletPool.Add(Instantiate(playerBulletPrefab, bulletParent).transform);
            playerBulletPool[i].GetComponent<Bullet>().SetupValues();
            playerBulletPool[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < enemyBulletPoolSize; i++)
        {
            enemyBulletPool.Add(Instantiate(enemyBulletPrefab, bulletParent).transform);
            enemyBulletPool[i].GetComponent<Bullet>().SetupValues();
            enemyBulletPool[i].gameObject.SetActive(false);
        }
    }

    private void SpawnImpacts()
    {
        for (int i = 0; i < impactPoolSize; i++)
        {
            impactEffectPool.Add(Instantiate(impactPrefab, impactParent).transform);
            impactEffectPool[i].gameObject.SetActive(false);
        }
    }

    private void SpawnEnemies()
    {
        for(int i = 0; i < allEnemies.Count; i++)
        {
            List<Transform> tempList = new List<Transform>();
            for (int j = 0; j < enemyPoolSize; j++)
            {
                tempList.Add(Instantiate(allEnemiesPrefabs[i], enemyParent).transform);
                tempList[j].GetComponent<AIThink_Base>().SetupValues();
            }

            enemies.Add(allEnemies[i], tempList);
            foreach (Transform enemObject in enemies[allEnemies[i]])
            {
                enemObject.gameObject.SetActive(false);
            }
        }
    }

    private void SpawnEnemyParticles()
    {
        List<Transform> tempListSpawn = new List<Transform>();
        List<Transform> tempListDie = new List<Transform>();
        List<Transform> tempListDrop = new List<Transform>();
        List<Transform> tempListExplosion = new List<Transform>();

        for (int i = 0; i < particlePoolSize; i++)
        {
            tempListSpawn.Add(Instantiate(enemySpawnPrefab, particleParent).transform);
            tempListDie.Add(Instantiate(enemyDiePrefab, particleParent).transform);
            tempListDrop.Add(Instantiate(enemyDropPrefab, particleParent).transform);
            tempListExplosion.Add(Instantiate(explosionPrefab, particleParent).transform);
        }

        enemyParticles.Add(particleType.enemySpawn, tempListSpawn);
        enemyParticles.Add(particleType.enemyDie, tempListDie);
        enemyParticles.Add(particleType.enemyDrop, tempListDrop);
        enemyParticles.Add(particleType.explosion, tempListExplosion);
    }

    #endregion

    #region Bullets

    public Transform TakeBullet(bool isPlayer)
    {
        if (isPlayer)
            return TakePlayerBullet();
        else
            return TakeEnemyBullet();
    }

    private Transform TakePlayerBullet()
    {
        if (playerBulletPool.Count > 0)
        {
            Transform bullet = playerBulletPool[0];
            playerBulletPool.RemoveAt(0);
            return bullet;
        }

        return null;
    }

    private Transform TakeEnemyBullet()
    {
        if (enemyBulletPool.Count > 0)
        {
            Transform bullet = enemyBulletPool[0];
            enemyBulletPool.RemoveAt(0);
            return bullet;
        }

        return null;
    }

    public void ReturnBullet(Transform bullet, bool isPlayer)
    {
        if (isPlayer)
            ReturnPlayerBullet(bullet);
        else
            ReturnEnemyBullet(bullet);
    }

    private void ReturnPlayerBullet(Transform bullet)
    {
        if (!playerBulletPool.Contains(bullet))
        {
            playerBulletPool.Add(bullet);
        }
    }

    private void ReturnEnemyBullet(Transform bullet)
    {
        if (!enemyBulletPool.Contains(bullet))
        {
            enemyBulletPool.Add(bullet);
        }
    }

    #endregion

    #region Impacts

    public void PlaceImpact(Vector3 pos, Vector3 normal, Vector3 scale)
    {
        if (normal == Vector3.zero)
            return;

        impactEffectPool[impactPoolCounter].gameObject.SetActive(true);
        impactEffectPool[impactPoolCounter].position = pos;
        impactEffectPool[impactPoolCounter].GetComponent<DecalProjector>().size = scale;
        impactEffectPool[impactPoolCounter].rotation = Quaternion.LookRotation(normal);
        impactEffectPool[impactPoolCounter].GetComponent<Impact>().PlayImpact();

        impactPoolCounter++;
        if (impactPoolCounter >= impactPoolSize)
            impactPoolCounter = 0;
    }

    public void ResetImpacts()
    {
        impactPoolCounter = 0;

        foreach(Transform dec in impactEffectPool)
            dec.gameObject.SetActive(false);
    }

    #endregion

    #region Enemies

    public Transform TakeEnemy(EnemyTemplate enem)
    {
        if (enemies[enem].Count > 0)
        {
            Transform newEnemy = enemies[enem][0];
            enemies[enem].RemoveAt(0);
            return newEnemy;
        }

        return null;
    }

    public void ReturnEnemy(EnemyTemplate enem, Transform enemyObject)
    {
        if (!enemies[enem].Contains(enemyObject))
        {
            enemies[enem].Add(enemyObject);
        }
    }

    #endregion

    #region Particles

    public void PlaceParticle(particleType type, Vector3 pos, Vector3 scale)
    {
        if (!enemyParticles.ContainsKey(type))
            return;

        if (type == particleType.enemySpawn)
            PlaceSpawn(pos);
        else if(type == particleType.enemyDie)
            PlaceDie(pos);
        else if (type == particleType.enemyDrop)
            PlaceDrop(pos);
        else if (type == particleType.explosion)
            PlaceExplosion(pos, scale);
    }

    private void PlaceSpawn(Vector3 pos)
    {
        enemyParticles[particleType.enemySpawn][particleSpawnCounter].position = pos;
        enemyParticles[particleType.enemySpawn][particleSpawnCounter].GetComponent<Impact>().PlayImpact();

        particleSpawnCounter++;
        if (particleSpawnCounter >= particlePoolSize)
            particleSpawnCounter = 0;
    }

    private void PlaceDie(Vector3 pos)
    {
        enemyParticles[particleType.enemyDie][particleDieCounter].position = pos;
        enemyParticles[particleType.enemyDie][particleDieCounter].GetComponent<Impact>().PlayImpact();

        particleDieCounter++;
        if (particleDieCounter >= particlePoolSize)
            particleDieCounter = 0;
    }

    private void PlaceDrop(Vector3 pos)
    {
        enemyParticles[particleType.enemyDrop][particleDropCounter].position = pos;
        enemyParticles[particleType.enemyDrop][particleDropCounter].GetComponent<Impact>().PlayImpact();

        particleDropCounter++;
        if (particleDropCounter >= particlePoolSize)
            particleDropCounter = 0;
    }

    private void PlaceExplosion(Vector3 pos, Vector3 scale)
    {
        enemyParticles[particleType.explosion][explosionCounter].position = pos;
        enemyParticles[particleType.explosion][explosionCounter].localScale = scale / 2;
        enemyParticles[particleType.explosion][explosionCounter].GetComponent<Impact>().PlayImpact();

        explosionCounter++;
        if (explosionCounter >= particlePoolSize)
            explosionCounter = 0;
    }

    #endregion
}

public enum particleType
{
    enemySpawn,
    enemyDie,
    enemyDrop,
    explosion
}