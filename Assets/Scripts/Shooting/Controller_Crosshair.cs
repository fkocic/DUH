using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Controller_Crosshair : MonoBehaviour
{
    [SerializeField] Image up, down, right, left;
    float startUp, startDown, startRight, startLeft;
    float value;
    bool isShooting;
    [SerializeField] int maxTravel;
    [SerializeField] float lingerTime, inBetweenTime, returnTime;

    [SerializeField] Image hitMarker;
    Color hitMarkerStart;
    [SerializeField] float markerFadeTime;

    CanvasGroup crosshairGroup;

    private void Start()
    {
        startUp = up.rectTransform.anchoredPosition.y;
        startDown = down.rectTransform.anchoredPosition.y;
        startRight = right.rectTransform.anchoredPosition.x;
        startLeft = left.rectTransform.anchoredPosition.x;

        hitMarkerStart = hitMarker.color;
        Color invCol = hitMarkerStart;
        invCol.a = 0f;
        hitMarker.color = invCol;

        crosshairGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        up.rectTransform.anchoredPosition = new Vector2(0, startUp + value);
        down.rectTransform.anchoredPosition = new Vector2(0, startDown - value);
        right.rectTransform.anchoredPosition = new Vector2(startRight + value, 0);
        left.rectTransform.anchoredPosition = new Vector2(startLeft - value, 0);
    }

    public void ToggleShooting(int intensity)
    {
        value += intensity*2;
        value = Mathf.Clamp(value, 0, maxTravel);

        if (!isShooting)
        {
            isShooting = true;
            DOTween.Kill("Cross");
            StartCoroutine(lingerReturn());
        }
    }

    private IEnumerator lingerReturn()
    {
        yield return new WaitForSeconds(inBetweenTime);
        isShooting = false;
        yield return new WaitForSeconds(inBetweenTime);

        if (!isShooting)
        {
            yield return new WaitForSeconds(lingerTime);
            DOTween.To(() => value, x => value = x, 0, returnTime).SetId("Cross");
        }
    }

    public void ToggleHitmarker()
    {
        DOTween.Kill("Marker");
        hitMarker.color = hitMarkerStart;
        StartCoroutine(fadeMarker());
    }

    private IEnumerator fadeMarker()
    {
        yield return new WaitForSeconds(lingerTime);
        hitMarker.DOFade(0f, markerFadeTime).SetId("Marker");
    }

    public void ToggleCrosshair(float alpha)
    {
        crosshairGroup.alpha = alpha;
    }
}
