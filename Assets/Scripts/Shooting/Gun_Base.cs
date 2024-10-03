using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Base : MonoBehaviour
{
    public GunTemplate gun;

    [HideInInspector]public int bulletsInMagazine;
    bool bulletInChamber = true;
    bool isReloading;

    AudioSource gunAudio;

    private void Start()
    {
        bulletsInMagazine = gun.magazineSize;
        gunAudio = GetComponent<AudioSource>();
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
            if (MainManager.Shooting.ammo[gun] < 1 && !gunAudio.isPlaying)
                PlaySoundEffect(gun.soundEmpty);

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
        MainManager.Effects.ShootEffects(gun.recoilStrength);
        MainManager.Effects.CameraShake(gun.recoilStrength, 0.5f);
        PlaySoundEffect(gun.soundShooting[Random.Range(0, gun.soundShooting.Length)]);

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
        Transform bullet = MainManager.Pooling.TakePlayerBullet();
        bullet?.GetComponent<Bullet>().StartBullet(spawnPos + spawnSource.forward * gun.bulletSpawnDistance, spawnSource.rotation, gun);
    }

    public virtual void ShootHitPoint(RaycastHit Hit)
    {
        Hit.collider.GetComponent<Object_Base>()?.Damage(gun.damage, Hit.point, Hit.normal, gun.size);

        foreach (Mod_Base mod in gun.ModifiersColission)
        {
            mod.ModifyWeaponColission(Hit.collider.gameObject, Hit.normal, Hit.point);
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
        if (MainManager.Shooting.ammo[gun] == 0)
            return;

        PlaySoundEffect(gun.soundReload);
        StartCoroutine(WaitReload());
    }

    public virtual void PlaySoundEffect(AudioClip clip)
    {
        gunAudio.pitch = Random.Range(0.97f, 1.03f);
        gunAudio.PlayOneShot(clip);
    }
}