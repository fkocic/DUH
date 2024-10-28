using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealth", menuName = "Scriptable Objects/Modifiers/PlayerHealth")]
public class Mod_Health : Mod_Base
{
    public int healthIncrement = 20;

    public override void PermanentModifyPlayer()
    {
        MainManager.Player.maxHealth += healthIncrement;
        MainManager.Player.UIHealth();
    }
}
