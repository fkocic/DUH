using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove_Base : MonoBehaviour
{
    [HideInInspector]public float moveSpeed, turnSpeed, defaultTurnSpeed, defaultMoveSpeed;
    [HideInInspector]public Animator anim;

    public virtual void SetupValues(float moveSpeed, float turnSpeed, Animator baseAnimator)
    {
        this.moveSpeed = moveSpeed;
        this.turnSpeed = turnSpeed;
        defaultTurnSpeed = turnSpeed;
        defaultMoveSpeed = moveSpeed;
        anim = baseAnimator;
    }

    public virtual void SetPosition(Vector3 position)
    {
        //
    }

    public virtual void MoveTo(Vector3 destination, int counter)
    {
        //
    }

    public virtual void Stop()
    {
        //
    }

    public virtual void LookAt(Vector3 position, float aimSpeed)
    {
        //
    }

    public virtual void ChangeSpeedPercent(int percentChange, float duration)
    {
        //
    }
}
