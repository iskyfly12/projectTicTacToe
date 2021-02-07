using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public void ShowPanel(CanvasGroup canvas)
    {
        DOVirtual.Float(0, 1, .5f, x => canvas.alpha = x);
        StartCoroutine(WaitToHideOrShow(canvas, true));
    }

    public void HidePanel(CanvasGroup canvas)
    {
        DOVirtual.Float(1, 0, .5f, x => canvas.alpha = x);
        StartCoroutine(WaitToHideOrShow(canvas, false));
    }

    IEnumerator WaitToHideOrShow(CanvasGroup canvas, bool state)
    {
        canvas.interactable = state;
        canvas.blocksRaycasts = state;
        canvas.gameObject.SetActive(state);
        yield return new WaitForSeconds(.5f);
        canvas.gameObject.SetActive(state);
    }
}
