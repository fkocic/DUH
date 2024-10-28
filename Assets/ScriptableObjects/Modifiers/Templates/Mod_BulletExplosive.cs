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

    public override void ModifyWeaponColission(GameObject hitObject, Vector3 normal, Vector3 impactPoint, GunTemplate firedFromGun, int bounceCounter)
    {
        Collider[] colliders = Physics.OverlapSphere(impactPoint, explosiveRadius);
        foreach (Collider hit in colliders)
        {
            hit.GetComponent<Object_Base>()?.Damage(modForGun.damage, impactPoint, Vector3.zero, modForGun.size, modForGun.isPlayerGun);
            MainManager.Pooling.PlaceParticle(particleType.explosion, impactPoint, new Vector3(explosiveRadius, explosiveRadius, explosiveRadius));
        }
    }
}
