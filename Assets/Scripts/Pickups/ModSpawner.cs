using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModSpawner : MonoBehaviour
{
    bool hasSpawned = false;

    private void Update()
    {
        if (MainManager.Game.isLevelOver && !hasSpawned)
        {
            SpawnMod();
            hasSpawned = true;
        }
    }

    private void SpawnMod()
    {
        MainManager.Pooling.PlaceParticle(particleType.enemyDrop, transform.position, Vector3.one);
        Instantiate(MainManager.Spawning.ModDrop(), transform.position, transform.rotation, transform);
    }
}
