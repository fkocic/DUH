using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Base : MonoBehaviour
{
    public GunTemplate gun;

    [HideInInspector]public int bulletsInMagazine;
    bool bulletInChamber = true;
    bool isReloading;

    [Header("Animations")]
    public float realReloadTime;
    public Animator gunAnimator;
    public ParticleSystem muzzleParticles;

    private void Start()
    {
        bulletsInMagazine = gun.magazineSize;
    }

    private IEnumerator FireRate()
    {
        yield return new WaitForSeconds(60f/gun.rateOfFireRPM);
        bulletInChamber = true;
    }

    private IEnumerator WaitReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(gun.reloadSpeed);

        int bulletsNeeded = gun.magazineSize - bulletsInMagazine;
        bulletsNeeded = Mathf.Clamp(bulletsNeeded, 0, MainManager.Shooting.ammo[gun]);

        bulletsInMagazine += bulletsNeeded;
        MainManager.Shooting.ChangeAmmo(gun, -bulletsNeeded);
        bulletInChamber = true;

        isReloading = false;
    }

    public virtual void Shoot(Transform spawnPos)
    {
        if (!bulletInChamber || isReloading)
        {
            if (MainManager.Shooting.ammo[gun] < 1 && !MainManager.Shooting.gunAudio.isPlaying)
            {
                MainManager.Shooting.PlayAudio(gun.soundEmpty);
                gunAnimator?.SetTrigger("ShootEmpty");
            }

            return;
        }

        bulletInChamber = false;
        bulletsInMagazine--;
        MainManager.Shooting.UIAmmo();

        foreach (Mod_Base mod in gun.ModifiersShoot)
        {
            mod.ModifyWeaponShoot(spawnPos, transform.gameObject);
        }

        CheckProximity(spawnPos.position, spawnPos);

        //EFFECTS
        MainManager.Effects.ShootEffects(gun.cameraShakeIntensity);
        MainManager.Effects.CameraShake(gun.cameraShakeIntensity, gun.cameraShakeDuration);
        MainManager.Shooting.PlayAudio(gun.soundShooting[Random.Range(0, gun.soundShooting.Length)]);
        gunAnimator?.SetTrigger("Shoot");
        muzzleParticles?.Play();

        //CHECK AMMO
        CheckAmmoState();
    }

    public virtual void CheckProximity(Vector3 spawnPos, Transform spawnSource)
    {
        RaycastHit Hit;

        if (Physics.Raycast(spawnPos, spawnSource.forward, out Hit, gun.proximityRadius, gun.proximityCollisionMask, QueryTriggerInteraction.Ignore))
        {
            ShootHitPoint(Hit);
        }
        else
        {
            ShootRigidbody(spawnPos, spawnSource);
        }
    }

    public virtual void ShootRigidbody(Vector3 spawnPos, Transform spawnSource)
    {
        Transform bullet = MainManager.Pooling.TakeBullet(gun.isPlayerGun);
        bullet?.GetComponent<Bullet>().StartBullet(spawnPos + spawnSource.forward * gun.bulletSpawnDistance, spawnSource.rotation, gun, 0);
    }

    public virtual void ShootHitPoint(RaycastHit Hit)
    {
        Hit.collider.GetComponent<Object_Base>()?.Damage(gun.damage, Hit.point, Hit.normal, gun.size, gun.isPlayerGun);

        foreach (Mod_Base mod in gun.ModifiersColission)
        {
            mod.ModifyWeaponColission(Hit.collider.gameObject, Hit.normal, Hit.point, gun, 0);
        }
    }

    public void CheckAmmoState()
    {
        if (bulletsInMagazine > 0)
        {
            StartCoroutine(FireRate());
        }
        else if (MainManager.Shooting.ammo[gun] > 0)
        {
            Reload();
        }
    }

    public virtual void Reload()
    {
        if (MainManager.Shooting.ammo[gun] == 0 || isReloading || bulletsInMagazine == gun.magazineSize)
            return;

        gunAnimator?.SetTrigger("Reload");

        StartCoroutine(WaitReload());
    }

    public virtual void SetAnimationFloat(string state, float value)
    {
        gunAnimator?.SetFloat(state, value);
    }

    public virtual void UpdateAnimationReload()
    {
        float newReloadSpeed = realReloadTime / gun.reloadSpeed;
        gunAnimator?.SetFloat("reloadSpeed", newReloadSpeed);
    }

    private void OnEnable()
    {
        isReloading = false;
        gunAnimator = GetComponentInChildren<Animator>();
        UpdateAnimationReload();
        //gunAnimator?.Play("Idle", 0, 0f);

        if (bulletInChamber == false)
        {
            CheckAmmoState();
        }
    }
}