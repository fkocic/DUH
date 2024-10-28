using UnityEngine;

public abstract class Mod_Base : ScriptableObject
{
    public string modName;

    public ModType modType;
    public ModPlayerType playerStage;
    public ModWeaponType weaponStage;
    public GunTemplate modForGun;

    public virtual void PermanentModifyWeapon()
    {
        //
    }

    public virtual void ModifyWeaponShoot(Transform spawnPos, GameObject gunShooting)
    {
        //
    }

    public virtual void ModifyWeaponColission(GameObject hitObject, Vector3 normal, Vector3 impactPoint, GunTemplate firedFromGun, int bounceCounter)
    {
        //
    }

    public virtual void ModifyWeaponFixedUpdate(Transform callObject)
    {
        //
    }

    public virtual void PermanentModifyPlayer()
    {
        //
    }

    public virtual void ModifyPlayer(Transform callObject) 
    {
        //
    }

    public void AttachModifier()
    {
        if(modType == ModType.weapon)
        {
            modForGun.AttachModifier(this, weaponStage);
        }
        else if(modType == ModType.player) 
        {
            MainManager.Player.AttachModifier(this);
        }
        else
        {
            //player
            modForGun.AttachModifier(this, weaponStage);
        }
    }
}
