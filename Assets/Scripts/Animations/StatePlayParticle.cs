using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayParticle : StateMachineBehaviour
{
    public int particleNum;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MainManager.Effects.PlayParticleSmoke(particleNum);
    }
}
