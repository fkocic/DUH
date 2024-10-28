using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Player : Object_Base
{
    public override void Damage(float dmg, Vector3 impactPoint, Vector3 faceNormal, float projectileScale, bool isPlayer)
    {
        MainManager.Effects.PlayerDamageIndicator(transform, impactPoint);
        MainManager.Player.ChangeHealth(-dmg);
    }
}
