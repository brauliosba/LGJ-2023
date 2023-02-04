using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[System.Serializable]
public class DialogueIndex
{
    public List<int> indices;
}

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
    [TextArea(3,4)]
    [SerializeField]
    private List<string> dialogues;
    [SerializeField]
    private List<DialogueIndex> dialogueIndices;

    private int backgroundIndex = 0;
    private bool isTextAnimated = false;
    private int indice = 0;
    private int currentIndex = 0;
    private int currentLineIndex = 0;
    private int currentBackgroundIndex = 0;

    private void Start()
    {
        mainText.text = "";
        ChangeBackground();
    }

    public void NextLine()
    {
        if (!isTextAnimated)
        {
            if (currentIndex < dialogueIndices[currentLineIndex].indices.Count)
            {
                string line = dialogues[currentLineIndex];
                StartCoroutine(_TextAnimation(indice, dialogueIndices[currentLineIndex].indices[currentIndex] + indice, line, mainText));
                indice += dialogueIndices[currentLineIndex].indices[currentIndex];
                currentIndex++;
            }
            else
            {
                isTextAnimated = true;
                currentIndex = 0;
                indice = 0;
                currentLineIndex++;
                currentBackgroundIndex++;
                ChangeBackground();
            }
        }
    }

    private IEnumerator _TextAnimation(int previousIndex, int lastIndex, string line, TMP_Text text)
    {
        isTextAnimated = true;
        text.text = "";
        
        for (int i = previousIndex; i < lastIndex; i++)
        {
            text.text = line.Substring(0, i) + "<color=#0000>" + line.Substring(i) + "</color>";

            yield return new WaitForSeconds(0.05f);
        }
        isTextAnimated = false;
    }

    private void ChangeBackground()
    {
        if (currentBackgroundIndex < backgounds.Count)
        {
            isTextAnimated = true;
            Sprite newBackground = backgounds[currentBackgroundIndex];

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

            isTextAnimated = false;
            NextLine();
        }
        else
        {
            isTextAnimated = true;
            SceneManager.LoadScene("LevelTesting -dev-Y");
        }
    }
}