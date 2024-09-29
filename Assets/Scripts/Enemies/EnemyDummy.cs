using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyDummy : Object_Base
{
    [SerializeField] EnemyTemplate enemy;
    [SerializeField] float health;
    [SerializeField] ParticleSystem bloodParticles;

    Material mat;

    private void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    public override void Damage(float dmg, Vector3 impactPoint, Vector3 faceNormal)
    {
        health -= dmg;
        if (health <= 0)
            Die();

        DamageEffect(impactPoint, faceNormal);
    }

    private void DamageEffect(Vector3 impactPoint, Vector3 faceNormal)
    {
        mat.SetColor("_Glow_Color", enemy.glowEffectColor);
        DOVirtual.Float(0, enemy.glowEffectStrength, enemy.glowEffectDuration, val => mat.SetFloat("_Glow_Strength", val)).SetLoops(2, LoopType.Yoyo);

        MainManager.Effects.ShowHitMarker();

        if (faceNormal != Vector3.zero)
            bloodParticles.transform.rotation = Quaternion.LookRotation(faceNormal);

        bloodParticles.transform.position = impactPoint;
        var emitParams = new ParticleSystem.EmitParams();
        bloodParticles.Emit(emitParams, 10);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
