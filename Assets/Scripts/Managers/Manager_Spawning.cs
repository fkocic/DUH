using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Spawning : MonoBehaviour
{
    [SerializeField] List<GameObject> healthPickups = new List<GameObject>();
    [SerializeField] List<GameObject> ammoPickups = new List<GameObject>();
    [SerializeField] List<GameObject> modPickups = new List<GameObject>();
    [SerializeField] int noDropNumber = 10;

    private List<GameObject> enemyDropPickupPool = new List<GameObject>();
    private GameObject noDrop = null;


    private void Start()
    {
        enemyDropPickupPool.AddRange(healthPickups);
        enemyDropPickupPool.AddRange(ammoPickups);
        for (int i = 0; i < noDropNumber; i++)
        {
            enemyDropPickupPool.Add(noDrop);
        }
    }

    public GameObject EnemyDrop()
    {
        return enemyDropPickupPool[Random.Range(0, enemyDropPickupPool.Count)];
    }

    public GameObject ModDrop()
    {
        /*
        int i = Random.Range(0, modPickups.Count);
        GameObject mod = modPickups[i];
        modPickups.RemoveAt(i);
        return mod;
        */

        return modPickups[Random.Range(0, modPickups.Count)];
    }
}
