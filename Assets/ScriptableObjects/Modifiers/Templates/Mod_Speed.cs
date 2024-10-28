using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSpeed", menuName = "Scriptable Objects/Modifiers/PlayerSpeed")]
public class Mod_Speed : Mod_Base
{
    public override void PermanentModifyPlayer()
    {
        MainManager.Player.IncreasePlayerSpeed();        
    }
}
