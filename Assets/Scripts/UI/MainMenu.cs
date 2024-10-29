using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        int rnd = Random.Range(1, 11);
        if (rnd == 10)
            SceneManager.LoadScene("Level" + rnd.ToString());
        else
            SceneManager.LoadScene("Level0" + rnd.ToString());
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
