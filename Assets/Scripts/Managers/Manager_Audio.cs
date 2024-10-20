using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Manager_Audio : MonoBehaviour
{
    public AudioClip[] musicClips;
    public AudioSource musicAudioOne, musicAudioTwo;
    public bool isPlayingMusic;

    public void SetupValues()
    {
        StartRandomMusic();
    }

    public void StartRandomMusic()
    {
        isPlayingMusic = true;
        StartCoroutine(PlayRandomMusic());
    }

    private IEnumerator PlayRandomMusic()
    {
        if (!isPlayingMusic)
            yield break;

        AudioClip ranClip = musicClips[Random.Range(0, musicClips.Length)];

        if (musicAudioOne.isPlaying)
        {
            musicAudioOne.DOFade(0f, 3f).OnComplete(StopMusicOne);
            musicAudioTwo.volume = 0;
            musicAudioTwo.PlayOneShot(ranClip);
            musicAudioTwo.DOFade(1f, 3f);
        }
        else
        {
            musicAudioTwo.DOFade(0f, 3f).OnComplete(StopMusicTwo);
            musicAudioOne.volume = 0;
            musicAudioOne.PlayOneShot(ranClip);
            musicAudioOne.DOFade(1f, 3f);
        }

        yield return new WaitForSeconds(ranClip.length - 5f);
        StartCoroutine(PlayRandomMusic());
    }

    public void StopAllMusic()
    {
        isPlayingMusic = false;
        musicAudioOne.DOFade(0f, 3f).OnComplete(StopMusicOne);
        musicAudioTwo.DOFade(0f, 3f).OnComplete(StopMusicTwo);
    }

    private void StopMusicOne()
    {
        musicAudioOne.Stop();
    }

    private void StopMusicTwo()
    {
        musicAudioTwo.Stop();
    }
}
