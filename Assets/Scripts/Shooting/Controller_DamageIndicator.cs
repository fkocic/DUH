using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Controller_DamageIndicator : MonoBehaviour
{
    [SerializeField] CanvasGroup groupDamageEffect;
    [SerializeField] CanvasGroup[] groupDamageDirection;
    Quaternion[] startRots = new Quaternion[3];

    [SerializeField] float damageEffectDuration, damageEffectFadeSpeed, damageEffectAlpha;
    [SerializeField] float directionEffectDuration, directionEffectFadeSpeed, directionEffectAlpha, directionShakeStrength;

    private void Start()
    {
        /*
        for(int i = 0; i < startRots.Length; i++) 
        {
            startRots[i] = groupDamageDirection[i].GetComponent<RectTransform>().rotation;
        }
        */
    }

    public void ShowIndicator(int dir)
    {
        //ShowGroupDirection(dir);
        ShowGroupDamage();
    }

    private void ShowGroupDamage()
    {
        DOTween.Kill("Group");
        groupDamageEffect.DOFade(damageEffectAlpha, damageEffectFadeSpeed).SetId("Group");

        StartCoroutine(waitFade());
        IEnumerator waitFade()
        {
            yield return new WaitForSeconds(damageEffectDuration);
            groupDamageEffect.DOFade(0, damageEffectFadeSpeed).SetId("Group");
        }
    }

    private void ShowGroupDirection(int pos)
    {
        DOTween.Kill("Dir" + pos.ToString());
        groupDamageDirection[pos].alpha = 0;
        groupDamageDirection[pos].GetComponent<RectTransform>().rotation = startRots[pos];

        groupDamageDirection[pos].DOFade(directionEffectAlpha, directionEffectFadeSpeed).SetId("Dir" + pos.ToString());
        groupDamageDirection[pos].GetComponent<RectTransform>().DOShakeRotation(0.3f, directionShakeStrength).SetId("Dir" + pos.ToString());

        StartCoroutine(waitFade());
        IEnumerator waitFade()
        {
            yield return new WaitForSeconds(directionEffectDuration);
            groupDamageDirection[pos].DOFade(0, directionEffectFadeSpeed).SetId("Dir" + pos.ToString());
        }
    }
}
