using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static Manager_Pooling Pooling;
    public static Manager_Player Player;
    public static Manager_Effects Effects;
    public static Manager_Shooting Shooting;
<<<<<<< HEAD
    public static Manager_Audio Audio;
=======
    public static Manager_Spawning Spawning;
    public static Manager_Game Game;
>>>>>>> dev-level-generation

    private void Start()
    {
        Pooling = GetComponent<Manager_Pooling>();
        Player = GetComponent<Manager_Player>();
        Effects = GetComponent<Manager_Effects>();
        Shooting = GetComponent<Manager_Shooting>();
<<<<<<< HEAD
        Audio = GetComponent<Manager_Audio>();
=======
        Spawning = GetComponent<Manager_Spawning>();
        Game = GetComponent<Manager_Game>();
>>>>>>> dev-level-generation

        Pooling?.SetupValues();
        Player?.SetupValues();
        Effects?.SetupValues();
        Shooting?.SetupValues();
        Audio?.SetupValues();
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