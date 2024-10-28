using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlaySound : StateMachineBehaviour
{
    public AudioClip clip;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MainManager.Shooting.PlayAudio(clip);
    }
}
