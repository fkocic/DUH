using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PickupType
{
    EnemyDrop,
    Mod
}

public class Pickup_Spawner : MonoBehaviour
{
    [SerializeField] PickupType pickupType;

    private LayerMask mask;

    private void Start()
    {
        mask = LayerMask.GetMask("Default", "Ledge");
    }

    public void SpawnPickup()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, mask))
        {
            GameObject objectToSpawn = null; //= MainManager.Spawning.EnemyDrop();

            if (pickupType == PickupType.Mod)
                objectToSpawn = MainManager.Spawning.ModDrop();

            else if (pickupType == PickupType.EnemyDrop)
                objectToSpawn = MainManager.Spawning.EnemyDrop();

            if (objectToSpawn == null)
                return;

            Instantiate(objectToSpawn, hit.point + Vector3.up, Quaternion.identity);
        }
    }

    private void SpawnEnemyDropPickup()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, mask))
        {
            GameObject objectToSpawn = MainManager.Spawning.EnemyDrop();

            if (objectToSpawn == null)
                return;

            Instantiate(objectToSpawn, hit.point + Vector3.up, Quaternion.identity);             
        }
    }
}
 