using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private bool isFirstTime = true;
    [SerializeField] private GameObject tutorialContainer;
    [SerializeField] private TMPro.TextMeshProUGUI textTutorial;
    [SerializeField] private UnityEngine.UI.Image icon;
    [SerializeField] private List<Sprite> iconList;

    System.Action endAction;
    void Start()
    {
        tutorialContainer.SetActive(false);
    }

    public void OnTutorialCommand(System.Action endAction)
    {

        Debug.Log("tutorial command");
        string text = " - Elige tu movimiento con el mouse \n" +
           " - Algunos movimientos tienen cooldown. ¡Úsalos con cuidado!";
        textTutorial.text = text;
        icon.sprite = iconList[0];
        
        if (endAction != null)
            this.endAction = endAction;
        tutorialContainer.SetActive(true);
    }

    public void OnTutorialInputSeries(System.Action endAction)
    {

        Debug.Log("tutorial input series");
        string text = " - Usa las teclas direccionales para defenderte.\n" +
            "- Mientras mejor lo hagas, menos daño te harán.";
        textTutorial.text = text;
        icon.sprite = iconList[1];
        if (endAction != null)
            this.endAction = endAction;
        tutorialContainer.SetActive(true);
        SetFirstTime();
    }
    public void EndTutorial()
    {
        tutorialContainer.SetActive(false);
        if (endAction != null)
        {
            endAction.Invoke();
        }
    }
    public bool IsFirstTime { get { return isFirstTime; } }
    public void SetFirstTime()
    {
        isFirstTime = false;
    }
}
