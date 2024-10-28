using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack_Kamikaze : AIAttack_Base
{
    public float explosiveRadius;
    public float explosiveDamage;

    public override void AimAt(Transform target)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosiveRadius);
        foreach (Collider hit in colliders)
        {
            hit.GetComponent<Object_Base>()?.Damage(explosiveDamage, transform.position, Vector3.zero, 0.1f, false);
            MainManager.Pooling.PlaceParticle(particleType.explosion, transform.position, new Vector3(explosiveRadius, explosiveRadius, explosiveRadius));
        }

        gameObject.SetActive(false);
    }
}
