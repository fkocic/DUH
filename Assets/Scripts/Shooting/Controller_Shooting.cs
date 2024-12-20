using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Controller_Shooting : MonoBehaviour
{
    private bool isPlayerLocked;

    private void Update()
    {
        if (isPlayerLocked)
            return;

        if (Input.GetButton("Fire1"))
        {
            MainManager.Shooting.activeGun?.Shoot(transform);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            MainManager.Shooting.activeGun?.Reload();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MainManager.Shooting.SwitchGunDirect(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MainManager.Shooting.SwitchGunDirect(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MainManager.Shooting.SwitchGunDirect(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            MainManager.Shooting.SwitchGunDirect(3);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            MainManager.Game.PauseGame();
        }
    }

    public void LockPlayer()
    {
        isPlayerLocked = true;
    }
}
