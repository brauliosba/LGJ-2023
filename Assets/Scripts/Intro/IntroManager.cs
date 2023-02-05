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
    private Button mainButton;

    [SerializeField]
    private Sprite posionXImage;

    [SerializeField]
    private List<Sprite> backgrounds;
    [TextArea(3,4)]
    [SerializeField]
    private List<string> dialogues;

    private int currentLineIndex = 0;
    private int currentBackgroundIndex = 0;
    private Coroutine textCoroutine;

    private void Start()
    {
        mainText.text = "";
        ChangeBackground();
        mainButton.onClick.RemoveAllListeners();
        mainButton.onClick.AddListener(OnNextImageButtonClicked);
    }

    public void OnNextImageButtonClicked()
    {
        if (textCoroutine != null)
        {
            StopCoroutine(textCoroutine);
            textCoroutine = null;
            mainText.text = dialogues[currentLineIndex];
        }
        else 
        { 
            if (currentBackgroundIndex < backgrounds.Count)
            {
                currentLineIndex++;
                currentBackgroundIndex++;
                ChangeBackground();
            }
            else
            {
                AudioManager.instance.StopMusic();
                SceneManager.LoadScene("LevelTesting -dev-Y");
            }
        }
    }

    private IEnumerator _TextAnimation(string line, TMP_Text text)
    {
        text.text = "";
        
        for (int i = 0; i < line.Length; i++)
        {
            text.text = line.Substring(0, i) + "<color=#0000>" + line.Substring(i) + "</color>";

            yield return new WaitForSeconds(0.05f);
        }
    }

    private void PosionX()
    {
        foregroundImage.sprite = backgroundImage.sprite;
        foregroundImage.color = Color.white;

        foregroundImage.gameObject.SetActive(true);

        backgroundImage.sprite = posionXImage;

        backgroundImage.color = Color.white;
        backgroundImage.gameObject.SetActive(true);

        foregroundImage.DOFade(0f, 1f).From(1f).OnComplete(() =>
        {
            foregroundImage.gameObject.SetActive(false);
        });

        mainButton.onClick.RemoveAllListeners();
        mainButton.onClick.AddListener(OnNextImageButtonClicked);
    }

    private void ChangeBackground()
    {
        if (currentBackgroundIndex < backgrounds.Count)
        {
            Sprite newBackground = backgrounds[currentBackgroundIndex];

            if (currentBackgroundIndex == 1)
            {
                mainButton.onClick.RemoveAllListeners();
                mainButton.onClick.AddListener(PosionX);
            }

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

            string line = dialogues[currentLineIndex];
            textCoroutine = StartCoroutine(_TextAnimation(line, mainText));
        }
    }
}