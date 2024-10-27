using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager_Game : MonoBehaviour
{
    private int currentLevel;
    private int levelsPassed = -1;

    public int enemyNumber = 20;
    public bool isLevelOver;

    public void InitializeLevel()
    {
        isLevelOver = false;
        enemyNumber += 5;
    }

    public void GenerateNextLevel()
    {
        levelsPassed++;
        GetRandomLevel();
        if (currentLevel == 10)
            SceneManager.LoadScene("Level" + currentLevel.ToString());
        else
            SceneManager.LoadScene("Level0" + currentLevel.ToString());

        InitializeLevel();
    }
    
    private void GetRandomLevel()
    {
        int rnd = Random.Range(1, 11);

        if (rnd == currentLevel)
        {
            GetRandomLevel();
            return;
        }
            
        currentLevel = rnd;
        return;
    }

    public void SetLevelOver()
    {
        isLevelOver = true;
    }

    public void RestartGame()
    {
        GenerateNextLevel();
        Destroy(transform.parent.transform.parent.gameObject);
    }
}
