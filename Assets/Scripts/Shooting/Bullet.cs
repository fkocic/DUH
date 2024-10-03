using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] LayerMask predictLayer;
    [SerializeField] bool isPlayerBullet;

    Rigidbody rb;
    GunTemplate firedFromGun;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        rb.isKinematic = true;
        StopAllCoroutines();

        if(isPlayerBullet)
            MainManager.Pooling.ReturnPlayerBullet(transform);
        else
            MainManager.Pooling.ReturnEnemyBullet(transform);
    }

    public void StartBullet(Vector3 spawnPos, Quaternion spawnRot, GunTemplate gun)
    {
        firedFromGun = gun;
        transform.position = spawnPos;
        transform.rotation = spawnRot;
        transform.localScale = new Vector3(gun.size, gun.size, gun.size);
        GetComponent<TrailRenderer>().colorGradient = gun.bulletTrailColor;
        GetComponent<TrailRenderer>().time = gun.bulletTrailDuration;

        gameObject.SetActive(true);
        rb.isKinematic = false;
        rb.AddForce(transform.forward * gun.speed, ForceMode.Impulse);

        StartCoroutine(DurationEnd(gun.duration));
    }

    private IEnumerator DurationEnd(float dur)
    {
        yield return new WaitForSeconds(dur);
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        foreach (Mod_Base mod in firedFromGun.ModifiersFixedUpdate)
        {
            mod.ModifyWeaponFixedUpdate(transform);
        }

        ApplyForceOverLifetime();
        PredictCollision();
    }

    private void ApplyForceOverLifetime()
    {
        rb.AddForce(firedFromGun.forceOverLifetime);
    }

    private void PredictCollision()
    {
        Vector3 prediction = transform.position + rb.velocity * Time.fixedDeltaTime;
        RaycastHit hit;

        if(Physics.Linecast(transform.position, prediction, out hit, predictLayer))
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            transform.position = hit.point;
            RealCollision(hit.collider.gameObject, hit.normal);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        RealCollision(collision.gameObject, collision.GetContact(0).normal);
    }

    private void RealCollision(GameObject hitObject, Vector3 normal)
    {
        foreach(Mod_Base mod in firedFromGun.ModifiersColission)
        {
            mod.ModifyWeaponColission(hitObject, normal, transform.position);
        }

        hitObject.GetComponent<Object_Base>()?.Damage(firedFromGun.damage, transform.position, normal, firedFromGun.size);
        gameObject.SetActive(false);
    }
}