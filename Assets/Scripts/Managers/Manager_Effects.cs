using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manager_Effects : MonoBehaviour
{
    [SerializeField] CameraMovement scriptCamera;
    [SerializeField] Controller_Crosshair scriptCrosshair;
    [SerializeField] Controller_DamageIndicator scriptIndicator;
    [SerializeField] AudioSource hitmarkerAudio;

    [SerializeField] float pickupFadeTime;
    [SerializeField] CanvasGroup pickupGroup;
    Quaternion pickpGroupRotation;
    [SerializeField] TMP_Text pickupText;

    [SerializeField] ParticleSystem[] grenadeSmokes;

    public void SetupValues()
    {
        pickpGroupRotation = pickupGroup.GetComponent<RectTransform>().localRotation;
    }

    public void ShootEffects(float intensity)
    {
        scriptCrosshair.ToggleShooting((int)intensity);
    }

    public void ShowHitMarker()
    {
        scriptCrosshair.ToggleHitmarker();

        if(!hitmarkerAudio.isPlaying)
            hitmarkerAudio.Play();
    }

    public void ToggleCrosshair(float alpha)
    {
        scriptCrosshair.ToggleCrosshair(alpha);
    }

    public void CameraShake(float intensity, float duration)
    {
        scriptCamera.AddRecoil(intensity, duration);
    }

    public void PlayerDamageIndicator(Transform player, Vector3 bulletPos)
    {
        Vector3 dmgDir;
        Vector3 leftRight = bulletPos;
        leftRight.y = player.position.y;

        dmgDir = leftRight - player.position;
        float angleLeftRight = Vector3.Angle(dmgDir, player.forward);

        Vector3 upDown = bulletPos;
        upDown.x = player.position.x;
        dmgDir = upDown - player.position;
        float angleUpDown = Vector3.Angle(dmgDir, player.forward);

        if(angleLeftRight > angleUpDown)
        {
            Vector3 localLeftRight = player.InverseTransformDirection(leftRight);
            if (localLeftRight.x < 0)
                scriptIndicator?.ShowIndicator(0);
            else
                scriptIndicator?.ShowIndicator(1);
        }
        else
        {
            scriptIndicator?.ShowIndicator(2);
        }
    }

    public void UIDisplayPickup(string pickupName)
    {
        DOTween.Kill("Fade");

        pickupText.text = "Picked up " + pickupName;
        pickupGroup.GetComponent<RectTransform>().localRotation = pickpGroupRotation;
        pickupGroup.alpha = 1;
        pickupGroup.GetComponent<RectTransform>().DOShakeRotation(0.5f, 30f).SetId("Fade");

        StartCoroutine(waitFade());

        IEnumerator waitFade()
        {
            yield return new WaitForSeconds(1);
            pickupGroup.DOFade(0, pickupFadeTime).SetId("Fade");
        }
    }

    public void PlayParticleSmoke(int num)
    {
        grenadeSmokes[num].Play();
    }
}
