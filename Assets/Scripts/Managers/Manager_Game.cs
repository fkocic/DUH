using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Manager_Game : MonoBehaviour
{
    private int currentLevel;
    private int levelsPassed = -1;
    private bool isFading;
    private float targetAlpha;

    public int enemyNumber = 20;
    public bool isLevelOver;
    public bool allEnemiesSpawned;

    [SerializeField] Image transitionPanel;

    private void Start()
    {
        StartCoroutine(StartInitialization());
    }

    private void FixedUpdate()
    {
        if (isFading)
            FadeTransparencyImage();
    }

    public void InitializeLevel()
    {
        FadeIn();
        isLevelOver = false;
        allEnemiesSpawned = false;
        enemyNumber += 5;
    }

    private IEnumerator StartInitialization()
    {
        transitionPanel.gameObject.SetActive(true);
        transitionPanel.color = new Color(0.0f, 0.0f, 0.0f, 1);
        yield return new WaitForSeconds(0.5f);
        FadeIn();

    }

    public void GenerateNextLevel()
    {
        FadeOut();
        StartCoroutine(DelayedGeneration());
    }

    private IEnumerator DelayedGeneration()
    {
        yield return new WaitForSeconds(1);
        SetUpNextLevel();
        
    }

    private void SetUpNextLevel()
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
        SetUpNextLevel();
        Destroy(transform.parent.transform.parent.gameObject);
    }

    public void FadeIn()
    {
        transitionPanel.gameObject.SetActive(true);
        transitionPanel.color = new Color(0.0f, 0.0f, 0.0f, 1);
        targetAlpha = 0;
        isFading = true;
    }

    public void FadeOut()
    {
        transitionPanel.gameObject.SetActive(true);
        transitionPanel.color = new Color(0.0f, 0.0f, 0.0f, 0);
        targetAlpha = 1;
        isFading = true;
    }

    private void FadeTransparencyImage()
    {
        float newAlpha;
        if (targetAlpha == 1)
        {
            newAlpha = transitionPanel.color.a + Time.deltaTime * 1.5f;
            if (newAlpha > 1)
                newAlpha = 1;
        }            
        else
        {
            newAlpha = transitionPanel.color.a - Time.deltaTime * 1.5f;
            if (newAlpha < 0)
                newAlpha = 0;
        }
            

        var newColor = new Color(0.0f, 0.0f, 0.0f, newAlpha);
        transitionPanel.color = newColor;

        if (transitionPanel.color.a == targetAlpha)
        {
            if (transitionPanel.color.a == 0)
                transitionPanel.gameObject.SetActive(false);

            isFading = false;
        }

    }

    public void CheckIfLevelOver()
    {
        if (allEnemiesSpawned && MainManager.Pooling.CheckIfEnemyPoolEmpty())
            isLevelOver = true;
    }

}
