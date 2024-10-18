using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public float spawnInterval, spawnParticleDuration;
    public EnemyTemplate[] spawnEnemies;
    public Transform[] spawnPositions;

    private void Start()
    {
        StartCoroutine(waitInterval());
    }

    private IEnumerator waitInterval()
    {
        yield return new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnEnemy());
        StartCoroutine(waitInterval());
    }

    private IEnumerator SpawnEnemy()
    {
        Vector3 pos = spawnPositions[Random.Range(0, spawnPositions.Length)].position;
        MainManager.Pooling.PlaceParticle(particleType.enemySpawn, pos, Vector3.one);

        yield return new WaitForSeconds(spawnParticleDuration);

        Transform newEnemy = MainManager.Pooling.TakeEnemy(spawnEnemies[Random.Range(0, spawnEnemies.Length)]);
        newEnemy?.GetComponent<AIThink_Base>().StartEnemy(pos);
    }
}
