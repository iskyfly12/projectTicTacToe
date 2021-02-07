using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
    [Header("First Player")]
    [SerializeField] private Color firstPlayerColor;
    [SerializeField] private TMP_ColorGradient firstPlayerTextColor;

    [Header("Second Player")]
    [SerializeField] private Color secondPlayerColor;
    [SerializeField] private TMP_ColorGradient secondPlayerTextColor;

    [Header("Neutral Game")]
    [SerializeField] private Color tieGameColor;
    [SerializeField] private TMP_ColorGradient tieGameTextColor;

    [Header("UI Game")]
    [SerializeField] private TextMeshProUGUI textResult;
    [SerializeField] private TextMeshProUGUI textPlayerTurn;
    [SerializeField] private Image[] imageToChangeColor;

    [Header("UI Panels")]
    [SerializeField] private CanvasGroup canvasTopInfo;
    [SerializeField] private CanvasGroup canvasResultInfo;
    [SerializeField] private CanvasGroup canvasMenu;

    public void UpdateInfoPlayer(PeaceType player)
    {
        canvasTopInfo.gameObject.SetActive(true);
        FadePanel(canvasTopInfo, 0, 1, .5f);
        SetUIColor(player);
    }

    public void ShowResults(PeaceType player)
    {
        canvasResultInfo.interactable = true;
        canvasResultInfo.blocksRaycasts = true;
        canvasResultInfo.gameObject.SetActive(true);
        canvasTopInfo.gameObject.SetActive(false);
        FadePanel(canvasResultInfo, 0, 1, .5f);
        SetUIColor(player);
    }

    public void ShowMainScreen()
    {
        PostProcessingSystem.SetDepthField(5, .5f, false);
    }

    public void HideResults()
    {
        canvasResultInfo.gameObject.SetActive(false);
    }

    private void SetUIColor(PeaceType player)
    {
        switch (player)
        {
            case PeaceType.None:
                SetImagesColor(tieGameColor);
                textResult.text = "Tie Game";
                //textResult.colorGradientPreset = tieGameTextColor;
                break;
            case PeaceType.Cross:
                SetImagesColor(firstPlayerColor);
                textPlayerTurn.text = "Blue Player";
                textResult.text = "Blue Player Wins!";
                //textResult.colorGradientPreset = firstPlayerTextColor;
                break;
            case PeaceType.Nought:
                SetImagesColor(secondPlayerColor);
                textPlayerTurn.text = "Red Player";
                textResult.text = "Red Player Wins!";
                //textResult.colorGradientPreset = secondPlayerTextColor;
                break;
        }
    }

    private void SetImagesColor(Color newColor)
    {
        for (int i = 0; i < imageToChangeColor.Length; i++)
            imageToChangeColor[i].color = newColor;
    }

    private void FadePanel(CanvasGroup canvas, float alphaStart, float alphaEnd, float time)
    {
        DOVirtual.Float(alphaStart, alphaEnd, time, x => canvas.alpha = x);
    }
}
