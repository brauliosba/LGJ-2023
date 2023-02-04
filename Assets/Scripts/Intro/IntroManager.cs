using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class IntroManager : MonoBehaviour
{
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Image foregroundImage;
    [SerializeField]
    private TMP_Text mainText;

    [SerializeField]
    private List<Sprite> backgounds;
    [SerializeField]
    private List<string> dialogueLines;

    private int backgroundIndex = 0;
    private bool isTextAnimated = false;

    private void Start()
    {
        ChangeBackground(backgounds[backgroundIndex]);
    }

    private IEnumerator _TextAnimation(string line, TMP_Text text)
    {
        isTextAnimated = true;
        text.text = "";
        
        for (int i = 0; i < line.Length; i++)
        {
            text.text = line.Substring(0, i) + "<color=#0000>" + line.Substring(i) + "</color>";

            yield return new WaitForSeconds(0.2f);
        }
        isTextAnimated = false;
    }

    private void ChangeBackground(Sprite newBackground)
    {
        if (backgroundImage.sprite != null)
        {
            foregroundImage.color = Color.black;
        }
        else
        {
            foregroundImage.sprite = backgroundImage.sprite;
            foregroundImage.color = Color.white;
        }

        foregroundImage.gameObject.SetActive(true);

        backgroundImage.sprite = newBackground;

        backgroundImage.color = Color.white;
        backgroundImage.gameObject.SetActive(true);

        foregroundImage.DOFade(0f, 1f).From(1f).OnComplete(() =>
        {
            foregroundImage.gameObject.SetActive(false);
        });

        StartCoroutine(_TextAnimation(dialogueLines[backgroundIndex], mainText));
    }

    public void OnNextButtonClicked()
    {
        if (!isTextAnimated)
        {
            backgroundIndex++;
            ChangeBackground(backgounds[backgroundIndex]);
        }
    }
}