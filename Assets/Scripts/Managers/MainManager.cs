using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static Manager_Pooling Pooling;
    public static Manager_Player Player;
    public static Manager_Effects Effects;
    public static Manager_Shooting Shooting;

    private void Start()
    {
        Pooling = GetComponent<Manager_Pooling>();
        Player = GetComponent<Manager_Player>();
        Effects = GetComponent<Manager_Effects>();
        Shooting = GetComponent<Manager_Shooting>();

        Pooling?.SetupValues();
        Player?.SetupValues();
        Effects?.SetupValues();
        Shooting?.SetupValues();
    }
}

public enum ModType
{
    weapon,
    player,
    both
}

public enum ModWeaponType
{
    onPickup,
    onShoot,
    onUpdate,
    onColission
}

public enum ModPlayerType
{
    health,
    speed,
    jump
}