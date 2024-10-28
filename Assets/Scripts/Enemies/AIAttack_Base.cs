using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack_Base : MonoBehaviour
{
    public GunTemplate gun;
    public Transform gunMuzzle;
    public ParticleSystem particleMuzzle;
    public float attackWindUpTime;

    [HideInInspector]public bool bulletInChamber;
    [HideInInspector]public AIThink_Base scriptMain;
    [HideInInspector] public Animator anim;

    public virtual void SetupValues(AIThink_Base scr, Animator baseAnimator)
    {
        scriptMain = scr;
        anim = baseAnimator;
    }

    public virtual void AimAt(Transform target)
    {
        RaycastHit hit;
        if (Physics.Raycast(gunMuzzle.position, gunMuzzle.forward, out hit, 100, gun.proximityCollisionMask, QueryTriggerInteraction.Ignore))
            if(hit.collider.transform == target)
                Shoot();
    }

    public virtual void Shoot()
    {
        if (!bulletInChamber)
            return;

        StartCoroutine(AttackWindUp());
    }

    private IEnumerator AttackWindUp()
    {
        bulletInChamber = false;
        anim?.SetTrigger("Shoot");

        yield return new WaitForSeconds(attackWindUpTime);

        foreach (Mod_Base mod in gun.ModifiersShoot)
        {
            mod.ModifyWeaponShoot(gunMuzzle, transform.gameObject);
        }

        PlayEffects();
        CheckProximity(gunMuzzle.position, gunMuzzle);

        if (gameObject.activeInHierarchy)
            StartCoroutine(FireRate());
    }

    public virtual void PlayEffects()
    {
        scriptMain.PlaySound(gun.soundShooting[Random.Range(0, gun.soundShooting.Length)]);
        particleMuzzle.Play();
    }

    public virtual void CheckProximity(Vector3 spawnPos, Transform spawnSource)
    {
        Vector3 spreadDirection = spawnSource.forward + spawnSource.up * Random.Range(-gun.spread, gun.spread) + spawnSource.right * Random.Range(-gun.spread, gun.spread);
        RaycastHit Hit;

        if (Physics.Raycast(spawnPos, spreadDirection, out Hit, gun.proximityRadius, gun.proximityCollisionMask, QueryTriggerInteraction.Ignore))
        {
            ShootHitPoint(Hit);
        }
        else
        {
            ShootRigidbody(spawnPos, spawnSource, spreadDirection);
        }
    }

    public virtual void ShootRigidbody(Vector3 spawnPos, Transform spawnSource, Vector3 spawnDirection)
    {
        Quaternion rot = Quaternion.LookRotation(spawnDirection);

        Transform bullet = MainManager.Pooling.TakeBullet(gun.isPlayerGun);
        bullet?.GetComponent<Bullet>().StartBullet(spawnPos + spawnSource.forward * gun.bulletSpawnDistance, rot, gun, 0);
    }

    public virtual void ShootHitPoint(RaycastHit Hit)
    {
        Hit.collider.GetComponent<Object_Base>()?.Damage(gun.damage, Hit.point, Hit.normal, gun.size, gun.isPlayerGun);

        foreach (Mod_Base mod in gun.ModifiersColission)
        {
            mod.ModifyWeaponColission(Hit.collider.gameObject, Hit.normal, Hit.point, gun, 0);
        }
    }

    private IEnumerator FireRate()
    {
        yield return new WaitForSeconds(60f / gun.rateOfFireRPM);
        bulletInChamber = true;
    }

    private void OnEnable()
    {
        bulletInChamber = true;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
