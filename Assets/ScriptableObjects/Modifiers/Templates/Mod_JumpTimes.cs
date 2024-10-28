using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerJumpTimes", menuName = "Scriptable Objects/Modifiers/PlayerJumpTimes")]
public class Mod_JumpTimes : Mod_Base
{
    public override void PermanentModifyPlayer()
    {
        MainManager.Player.IncreasePlayerJumpTimes();
    }
}
