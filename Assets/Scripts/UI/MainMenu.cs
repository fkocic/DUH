using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource menuMusic;

    private void Start()
    {
        //PlayMusic();
    }

    public void StartGame()
    {
        int rnd = Random.Range(1, 10);
        if (rnd == 10)
            SceneManager.LoadScene("FinalLevel" + rnd.ToString());
        else
            SceneManager.LoadScene("FinalLevel0" + rnd.ToString());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PlayMusic()
    {
        menuMusic.Play();
    }
}
