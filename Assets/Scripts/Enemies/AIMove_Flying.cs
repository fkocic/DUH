using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove_Flying : AIMove_Base
{
    Rigidbody rb;

    public LayerMask colideLayer;
    public float flyRandomness;
    public float flyHeight;
    public float heightRandomness;  
    public int maxRecursions;

    Vector3 addedRandomness, correctedDestination;
    Quaternion finalRotation;

    public override void SetupValues(float moveSpeed, float turnSpeed, Animator baseAnimator)
    {
        rb = GetComponent<Rigidbody>();
        finalRotation = Quaternion.identity;
        base.SetupValues(moveSpeed, turnSpeed, baseAnimator);
    }

    public override void SetPosition(Vector3 position)
    {
        Vector3 addedRandomness = Random.insideUnitSphere * 2;
        addedRandomness.y = Mathf.Abs(addedRandomness.y) + flyHeight/2;
        position += addedRandomness;

        transform.position = position;
    }

    public override void MoveTo(Vector3 destination, int counter)
    {
        counter++;
        correctedDestination = destination;
        addedRandomness = Random.insideUnitSphere * flyRandomness;
        addedRandomness.y = Mathf.Abs(addedRandomness.y);
        correctedDestination += addedRandomness;

        RaycastHit hit;
        if (Physics.Raycast(correctedDestination, Vector3.down, out hit, 100, colideLayer, QueryTriggerInteraction.Ignore))
        {
            correctedDestination = hit.point + Vector3.up * (flyHeight + Random.Range(0f, heightRandomness));
        }

        if (Physics.Linecast(transform.position, correctedDestination, colideLayer) && counter < maxRecursions)
        {
            MoveTo(correctedDestination, counter);
            return;
        }

        LookAt(correctedDestination, defaultTurnSpeed);
        AddForceTowards(correctedDestination);
    }

    private void AddForceTowards(Vector3 destination)
    {
        float dist = Vector3.Distance(transform.position, destination);
        dist = Mathf.Clamp(dist, 0f, 3f);

        Vector3 direction = destination - transform.position;
        rb.AddForce(direction * moveSpeed * Time.fixedDeltaTime * dist, ForceMode.Force);
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, turnSpeed * Time.deltaTime);
        anim?.SetFloat("moveSpeed", rb.velocity.magnitude);

        LimitVelocity();
    }

    private void LimitVelocity()
    {
        if (rb.velocity.magnitude > moveSpeed)
        {
            Vector3 limitedVel = rb.velocity.normalized * moveSpeed;
            rb.velocity = limitedVel;
        }
    }

    public override void LookAt(Vector3 position, float aimSpeed)
    {
        turnSpeed = aimSpeed;

        Vector3 direction = position - transform.position;
        finalRotation = Quaternion.LookRotation(direction.normalized);
    }

    public override void ChangeSpeedPercent(int percentChange, float duration)
    {
        float timer = 0;
        float change = 1f + (float)percentChange / 100f;
        moveSpeed *= change;

        StartCoroutine(effectDuration());
        IEnumerator effectDuration()
        {
            while (timer < duration)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            moveSpeed = defaultMoveSpeed;
        }
    }
}
