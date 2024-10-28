using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manager_Shooting : MonoBehaviour
{
    [SerializeField] List<GunTemplate> allGuns = new List<GunTemplate>();
    [SerializeField] List<Transform> allGunsTransforms = new List<Transform>();
    [SerializeField] List<Transform> pickedGuns = new List<Transform>();
    public Dictionary<GunTemplate, Transform> guns = new Dictionary<GunTemplate, Transform>();
    public Dictionary<GunTemplate, int> ammo = new Dictionary<GunTemplate, int>();
    public Gun_Base activeGun;

    [Header("Gun Switching")]
    int previousGun = -1, currentGun = -1;
    [SerializeField] Vector3 activePosition, inactivePosition;
    [SerializeField] float switchSpeed;
    [SerializeField] float swayStrength, swaySpeed;

    [Header("UI")]
    public TMP_Text txtAmmo;
    public TMP_Text txtSelectedWeapon;

    [Header("Animation Info")]
    CharacterController playerController;
    PlayerMovement scriptPlayer;

    [Header("Audio")]
    public AudioSource gunAudio;
    public ParticleSystem particleSteam;

    public void SetupValues()
    {
        for (int i = 0; i < allGuns.Count; i++)
        {
            ammo.Add(allGuns[i], allGuns[i].startAmmoInInventory);
            guns.Add(allGuns[i], allGunsTransforms[i]);
            allGuns[i].ResetValues();
        }

        playerController = MainManager.Player.player.GetComponent<CharacterController>();
        scriptPlayer = MainManager.Player.player.GetComponent<PlayerMovement>();

        UIAmmo();
    }

    public void ChangeAmmo(GunTemplate gun, int amount)
    {
        ammo[gun] += amount;
        ammo[gun] = Mathf.Clamp(ammo[gun], 0, gun.maxAmmo);

        UIAmmo();
    }

    public void PickupWeapon(GunTemplate gun)
    {
        if (pickedGuns.Contains(guns[gun]))
        {
            ChangeAmmo(gun, gun.magazineSize);
        }
        else
        {
            pickedGuns.Add(guns[gun]);
            SwitchGunDirect(pickedGuns.Count - 1);

            UIAmmo();
        }
    }

    #region Animation

    public void SwayWeapon(Vector2 mouseInput)
    {
        if (!activeGun)
            return;

        Quaternion rotY = Quaternion.AngleAxis(mouseInput.x * swayStrength, pickedGuns[currentGun].forward);
        pickedGuns[currentGun].localRotation = Quaternion.Slerp(pickedGuns[currentGun].localRotation, rotY, swaySpeed * Time.deltaTime);
    }

    private void Update()
    {
        if (!activeGun)
            return;

        if(!scriptPlayer.grounded || playerController.velocity.magnitude < 0.1f)
            activeGun.SetAnimationFloat("moveSpeed", 0f);
        else
            activeGun.SetAnimationFloat("moveSpeed", scriptPlayer.speed);
    }

    #endregion

    #region GunSwitch
    public void SwitchGunDirect(int num)
    {
        if (num > pickedGuns.Count - 1)
            return;

        DOTween.Kill("GunUp");
        DOTween.Kill("GunDown");

        activeGun = null;
        previousGun = currentGun;
        currentGun = num;

        PutGunDown();
    }

    public void PutGunDown()
    {
        if (previousGun == -1)
        {
            ActivateGun();
            return;
        }

        pickedGuns[previousGun].DOLocalMove(inactivePosition, switchSpeed / 2).SetId("GunDown").OnComplete(DeactivateGun);
    }

    private void PutGunUp()
    {
        pickedGuns[currentGun].DOLocalMove(activePosition, switchSpeed / 2).SetId("GunUp").OnComplete(SetNewGun);
    }

    private void ActivateGun()
    {
        pickedGuns[currentGun].gameObject.SetActive(true);
        PutGunUp();
    }

    private void DeactivateGun()
    {
        //pickedGuns[previousGun].gameObject.SetActive(false);
        foreach(Transform t in pickedGuns)
        { t.gameObject.SetActive(false); }
        ActivateGun();
    }

    private void SetNewGun()
    {
        activeGun = pickedGuns[currentGun].GetComponent<Gun_Base>();
        UIAmmo();
    }
    #endregion

    #region UI

    public void UIAmmo()
    {
        if (!activeGun)
            return;

        txtAmmo.text = activeGun.bulletsInMagazine.ToString() + "/" + ammo[activeGun.gun].ToString();
        txtSelectedWeapon.text = activeGun.gun.gunName;
    }

    #endregion

    #region Effects

    public void PlayAudio(AudioClip clip)
    {
        gunAudio.pitch = Random.Range(0.97f, 1.03f);
        gunAudio.PlayOneShot(clip);
    }

    public void PlayParticle()
    {
        particleSteam.Play();
    }

    #endregion
}
