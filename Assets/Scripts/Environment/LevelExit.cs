using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && MainManager.Game.isLevelOver)
        {
            MainManager.Game.GenerateNextLevel();
        }
    }
}
