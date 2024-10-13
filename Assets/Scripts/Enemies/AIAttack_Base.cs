using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class AIAttack_Base : MonoBehaviour
{
    public GunTemplate gun;
    public Transform gunMuzzle;

    bool bulletInChamber = true;

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

        bulletInChamber = false;

        foreach (Mod_Base mod in gun.ModifiersShoot)
        {
            mod.ModifyWeaponShoot(gunMuzzle, transform.gameObject);
        }

        //play audio
        //play muzzle flash

        CheckProximity(gunMuzzle.position, gunMuzzle);
        StartCoroutine(FireRate());
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
}