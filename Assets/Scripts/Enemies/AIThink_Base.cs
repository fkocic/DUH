using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIThink_Base : MonoBehaviour
{
    public EnemyTemplate enemyType;
    [HideInInspector]public float health;
    public ParticleSystem bloodParticles;
    public SkinnedMeshRenderer[] renderers;
    public Animator anim;

    public LayerMask colideLayer;
    RaycastHit hit;

    AIMove_Base aiMove;
    AIAttack_Base aiAttack;

    AudioSource aud;

    public void SetupValues()
    {
        aiMove = GetComponent<AIMove_Base>();
        aiAttack = GetComponent<AIAttack_Base>();

        aud = GetComponent<AudioSource>();

        aiMove.SetupValues(enemyType.moveSpeed, enemyType.turnSpeed, anim);
        aiAttack.SetupValues(this, anim);
    }

    public void StartEnemy(Vector3 pos)
    {
        health = enemyType.startHealth;
        aiMove.SetPosition(pos);

        gameObject.SetActive(true);
        InvokeRepeating(nameof(Think), enemyType.thinkFrequency, enemyType.thinkFrequency);
    }

    public virtual void Think()
    {
        float dist = Vector3.Distance(transform.position, MainManager.Player.player.position);
        if (dist > enemyType.preferredDistanceToPlayer)
        {
            aiMove.MoveTo(MainManager.Player.player.position, 0);
        }
        else
        {
            if (Physics.Raycast(aiAttack.gunMuzzle.position, MainManager.Player.player.position - aiAttack.gunMuzzle.position, out hit, enemyType.preferredDistanceToPlayer, colideLayer, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    aiMove.Stop();
                    aiMove.LookAt(MainManager.Player.player.position, enemyType.aimSpeed);
                    aiAttack.AimAt(MainManager.Player.player);
                }
                else
                {
                    aiMove.MoveTo(transform.position + Random.insideUnitSphere * 5f, 0);
                }
            }
            else
            {
                aiMove.MoveTo(transform.position + Random.insideUnitSphere * 5f, 0);
            }
        }
    }

    public void Damage(float dmg, Vector3 impactPoint, Vector3 faceNormal, bool isDamagedByPlayer)
    {
        health -= dmg;

        DamageEffect(impactPoint, faceNormal, isDamagedByPlayer);

        if (health <= 0)
        {
            Pickup_Spawner spawner = gameObject.GetComponent<Pickup_Spawner>();
            if (spawner != null)
            {
                spawner.SpawnPickup();
                Debug.Log("Drop spawned");
            }

            gameObject.SetActive(false);
        }
            
    }

    private void DamageEffect(Vector3 impactPoint, Vector3 faceNormal, bool isDamagedByPlayer)
    {
        foreach(SkinnedMeshRenderer mr in renderers)
        {
            Material mat = mr.material;
            mat.SetColor("_Glow_Color", enemyType.glowEffectColor);
            DOVirtual.Float(0, enemyType.glowEffectStrength, enemyType.glowEffectDuration, val => mat.SetFloat("_Glow_Strength", val)).SetLoops(2, LoopType.Yoyo);
        }

        if(isDamagedByPlayer)
            MainManager.Effects.ShowHitMarker();

        if (faceNormal != Vector3.zero)
            bloodParticles.transform.rotation = Quaternion.LookRotation(faceNormal);

        bloodParticles.transform.position = impactPoint;
        bloodParticles.Play();

        anim?.SetTrigger("GotHit");
    }

    public virtual void PlaySound(AudioClip clip)
    {
        aud.PlayOneShot(clip);
    }

    private void OnDisable()
    {
        CancelInvoke();

        MainManager.Pooling.PlaceParticle(particleType.enemyDie, transform.position, Vector3.one);
        MainManager.Pooling.ReturnEnemy(enemyType, transform);

        MainManager.Game.CheckIfLevelOver();
    }
}
