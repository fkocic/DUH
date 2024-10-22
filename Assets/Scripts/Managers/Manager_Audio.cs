using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Manager_Audio : MonoBehaviour
{
    public AudioClip[] musicClips;
    public AudioSource musicAudioOne, musicAudioTwo;
    public bool isPlayingMusic;

    int currentMusicClip;

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

        int pickedMusicClip;
        do
        {
            pickedMusicClip = Random.Range(0, musicClips.Length);
        } while (currentMusicClip == pickedMusicClip);

        currentMusicClip = pickedMusicClip;

        if (musicAudioOne.isPlaying)
        {
            musicAudioOne.DOFade(0f, 3f).OnComplete(StopMusicOne);
            musicAudioTwo.volume = 0;
            musicAudioTwo.PlayOneShot(musicClips[currentMusicClip]);
            musicAudioTwo.DOFade(1f, 3f);
        }
        else
        {
            musicAudioTwo.DOFade(0f, 3f).OnComplete(StopMusicTwo);
            musicAudioOne.volume = 0;
            musicAudioOne.PlayOneShot(musicClips[currentMusicClip]);
            musicAudioOne.DOFade(1f, 3f);
        }

        yield return new WaitForSeconds(musicClips[currentMusicClip].length - 5f);
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
