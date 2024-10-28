using UnityEngine;

[CreateAssetMenu(fileName = "BulletFreeze", menuName = "Scriptable Objects/Modifiers/BulletFreeze")]
public class Mod_BulletFreeze : Mod_Base
{
    public int percentChange;
    public float effectDuration;

    public override void ModifyWeaponColission(GameObject hitObject, Vector3 normal, Vector3 impactPoint, GunTemplate firedFromGun, int bounceCounter)
    {
        hitObject.GetComponentInParent<AIMove_Base>()?.ChangeSpeedPercent(percentChange, effectDuration);
    }
}
