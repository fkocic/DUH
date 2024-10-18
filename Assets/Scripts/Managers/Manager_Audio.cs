using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Audio : MonoBehaviour
{
    public AudioClip[] musicClips;
    public AudioSource musicAudio;

    public void SetupValues()
    {
        //
    }

    public void PlayRandomMusic()
    {
        //tween in between clips
        musicAudio.PlayOneShot(musicClips[Random.Range(0, musicClips.Length)]);
    }
}
