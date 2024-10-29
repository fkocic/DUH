using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public float spawnInterval, spawnParticleDuration;
    public EnemyTemplate[] spawnEnemies;
    public Transform[] spawnPositions;

    public int maxEnemyNumber = 20;
    public int enemyNumber;


    private void Start()
    {       
        enemyNumber = 0;
        StartCoroutine(checkForManager());        
    }

    private IEnumerator checkForManager()
    {
        yield return new WaitForSeconds(spawnInterval);

        if (MainManager.Game)
            StartCoroutine(waitInterval());
        else
            StartCoroutine(checkForManager());                    
    }

    private IEnumerator waitInterval()
    {
        if (enemyNumber < maxEnemyNumber && MainManager.Game)
        {
            yield return new WaitForSeconds(spawnInterval);
            StartCoroutine(SpawnEnemy());
            StartCoroutine(waitInterval());
            enemyNumber++;
        }


        if (enemyNumber == maxEnemyNumber)
            MainManager.Game.allEnemiesSpawned = true;
    }

    private IEnumerator SpawnEnemy()
    {
        maxEnemyNumber = MainManager.Game.enemyNumber;
        Vector3 pos = spawnPositions[Random.Range(0, spawnPositions.Length)].position;
        MainManager.Pooling.PlaceParticle(particleType.enemySpawn, pos, Vector3.one);

        yield return new WaitForSeconds(spawnParticleDuration);

        Transform newEnemy = MainManager.Pooling.TakeEnemy(spawnEnemies[Random.Range(0, spawnEnemies.Length)]);
        newEnemy?.GetComponent<AIThink_Base>().StartEnemy(pos);
    }
}
