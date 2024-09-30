using UnityEngine;

[CreateAssetMenu(fileName = "BulletExplosive", menuName = "Scriptable Objects/Modifiers/BulletExplosive")]
public class Mod_BulletExplosive : Mod_Base
{
    public float damageIncrease;
    public float explosiveRadius;

    public override void PermanentModifyWeapon()
    {
        modForGun.damage += damageIncrease;
    }

    public override void ModifyWeaponColission(GameObject hitObject, Vector3 normal, Vector3 impactPoint)
    {
        Collider[] colliders = Physics.OverlapSphere(impactPoint, explosiveRadius);
        foreach (Collider hit in colliders)
        {
            hit.GetComponent<Object_Base>()?.Damage(modForGun.damage, impactPoint, Vector3.zero, modForGun.size);
            MainManager.Pooling.PlaceExplosion(impactPoint, new Vector3(modForGun.size, modForGun.size, modForGun.size));

            //Rigidbody rb = hit.GetComponent<Rigidbody>();

            //if (rb != null)
            //    rb.AddExplosionForce(200, callObject.position, explosiveRadius, 3.0F);
        }
    }
}
