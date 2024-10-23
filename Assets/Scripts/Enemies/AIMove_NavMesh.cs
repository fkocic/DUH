using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMove_NavMesh : AIMove_Base
{
    NavMeshAgent agent;

    [SerializeField] AnimationCurve jumpCurve = new AnimationCurve();
    [SerializeField] float jumpSpeed;
    [SerializeField] float wanderDistance;

    Quaternion finalRotation;
    NavMeshPath newPath;

    public override void SetupValues(float moveSpeed, float turnSpeed, Animator baseAnimator)
    {
        newPath = new NavMeshPath();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.angularSpeed = 0;
        finalRotation = Quaternion.identity;
        base.SetupValues(moveSpeed, turnSpeed, baseAnimator);
    }

    public override void SetPosition(Vector3 position)
    {
        NavMeshHit correctedPos;
        Vector3 addedRandomness = Random.insideUnitSphere * 2;
        addedRandomness.y = Mathf.Abs(addedRandomness.y);
        position += addedRandomness;

        if (NavMesh.SamplePosition(position, out correctedPos, 3, NavMesh.AllAreas))
            agent.Warp(correctedPos.position);
    }

    public override void MoveTo(Vector3 destination, int counter)
    {
        agent.isStopped = false;

        NavMeshHit correctedPos;

        if (NavMesh.SamplePosition(destination, out correctedPos, 3, NavMesh.AllAreas))
        {
            NavMesh.CalculatePath(transform.position, correctedPos.position, NavMesh.AllAreas, newPath);
            if (newPath.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetPath(newPath);
                LookAt(agent.steeringTarget, defaultTurnSpeed);
                return;
            }
        }

        LookAt(correctedPos.position, defaultTurnSpeed);
    }

    //private void Wander()
    //{
    //    agent.isStopped = false;
    //    if (newPath.status == NavMeshPathStatus.PathComplete && agent.hasPath)
    //    {
    //        LookAt(agent.steeringTarget, defaultTurnSpeed);
    //    }
    //    else
    //    {
    //        NavMeshHit wanderPos;

    //        if (NavMesh.SamplePosition(transform.position + Random.insideUnitSphere * wanderDistance, out wanderPos, wanderDistance, NavMesh.AllAreas))
    //        {
    //            NavMesh.CalculatePath(transform.position, wanderPos.position, NavMesh.AllAreas, newPath);
    //            if (newPath.status == NavMeshPathStatus.PathComplete)
    //            {
    //                agent.SetPath(newPath);
    //                LookAt(agent.steeringTarget, defaultTurnSpeed);
    //            }
    //        }
    //    }
    //}

    public override void Stop()
    {
        agent.isStopped = true;
    }

    private void Update()
    {
        if (agent.isOnOffMeshLink)
        {
            StartCoroutine(jumpWait(jumpSpeed));
            agent.CompleteOffMeshLink();
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, turnSpeed * Time.deltaTime);
        anim?.SetFloat("moveSpeed", agent.velocity.magnitude);
    }

    private IEnumerator jumpWait(float duration)
    {
        anim?.SetTrigger("Jump");

        Vector3 startPos = transform.position;
        Vector3 endPos = agent.currentOffMeshLinkData.endPos + Vector3.up * agent.baseOffset;
        float timer = 0f;

        while (timer < 1f)
        {
            float yOffset = jumpCurve.Evaluate(timer);
            agent.transform.position = Vector3.Lerp(startPos, endPos, timer) + yOffset * Vector3.up;
            timer += Time.deltaTime / duration;
            yield return null;
        }
    }

    public override void LookAt(Vector3 position, float aimSpeed)
    {
        turnSpeed = aimSpeed;

        Vector3 tempPos = position;
        tempPos.y = transform.position.y;
        Vector3 direction = tempPos - transform.position;

        if(direction != Vector3.zero)
            finalRotation = Quaternion.LookRotation(direction.normalized);
    }

    public override void ChangeSpeedPercent(int percentChange, float duration)
    {
        float timer = 0;
        float change = 1f + (float)percentChange / 100f;
        moveSpeed *= change;
        agent.speed = moveSpeed;

        StartCoroutine(effectDuration());
        IEnumerator effectDuration()
        {
            while (timer < duration)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            moveSpeed = defaultMoveSpeed;
            agent.speed = moveSpeed;
        }
    }
}
