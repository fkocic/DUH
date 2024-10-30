using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [SerializeField] List<GameObject> doors = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && MainManager.Game.isLevelOver)
        {
            MainManager.Game.GenerateNextLevel();
        }
    }

    public void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            Destroy(door);
        }
    }
}
