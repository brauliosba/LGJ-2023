using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class InputManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI totalDamageFeedback;
    [SerializeField] private TMPro.TextMeshProUGUI goodFeedback;
    [SerializeField] private TMPro.TextMeshProUGUI perfectFeedback;
    [SerializeField] private Transform feedbackInput;
    private int totalDamage = 0;
    private int goodCount = 0;
    private int missCount = 0;
    private int perfectCount = 0;

    int baseDamage;
    bool isDefend;
    public int GetTotalDamage()
    {
      //  Debug.Log("Finish total damage " + totalDamage);
      //  Debug.Log("miss count " + missCount);
      //  Debug.Log("good count " + goodCount);
      //  Debug.Log("perfect count " + perfectCount);
        return totalDamage;
    }
    void Update()
    {
        if (totalDamageFeedback != null)
            totalDamageFeedback.text = "Total Damage " + totalDamage;
        if (goodFeedback != null)
            goodFeedback.text = "Good " + goodCount;
        if (perfectFeedback != null)
            perfectFeedback.text = "Perfect " + perfectCount;
        if (GameManager.instance.battleSeriesManager.BattleState == BattleState.idle)
            return;
        CheckInput();
    }
    public void StartManager(int baseDamage, bool isDefend)
    {
        this.baseDamage = baseDamage;
        this.isDefend = isDefend;
    }
    public void CheckInput()
    {
        GoalState state = GameManager.instance.battleSeriesManager.goalManager.state;
        InputArrowPrefab currentPrefab = GameManager.instance.battleSeriesManager.spawnManager.currentArrowPrefab();

        int totalSeries = GameManager.instance.battleSeriesManager.spawnManager.TotalSeries;
        int damage = baseDamage / totalSeries;
        if (currentPrefab == null)
            return;

        if (Input.GetKeyDown(currentPrefab.inputGame.key) /*&& !currentPrefab.wasPresed*/)
        {
            currentPrefab.wasPresed = true;
            if (state == GoalState.good)
            {
                if (!isDefend)
                {
                    totalDamage += Mathf.FloorToInt(damage * 0.5f);
                }
                else
                {
                    totalDamage += Mathf.FloorToInt(damage * 0.33f);
                }
                    
                goodCount++;
                FeedbackInput("Good!!");
                AudioManager.instance.PlaySFX("good");
            }
            else if (state == GoalState.perfect)
            {
                if (!isDefend)
                {
                    totalDamage += Mathf.FloorToInt(damage);
                }
                else
                {
                    totalDamage += Mathf.FloorToInt(damage * 0.66f);
                }
                                
                perfectCount++;
                FeedbackInput("PERFECT!!");
                AudioManager.instance.PlaySFX("perfect");
            }
            else
            {
                missCount++;
                AudioManager.instance.PlaySFX("miss");
                print("Miss");
            }
            GameManager.instance.battleSeriesManager.spawnManager.CheckIsCurrentPrefabIsTheLastOne(currentPrefab);
            currentPrefab.Destroy();
           // Destroy(currentPrefab.gameObject);
        }
    }
    public void ResetDamage()
    {
        totalDamage = 0;
    }
    [SerializeField]private GameObject feedbackPrefab;
    public void FeedbackInput(string text)
    {
        GameObject prefab = Instantiate(feedbackPrefab, feedbackInput);
        RectTransform rectTransform = prefab.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;
        prefab.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
        rectTransform.DOAnchorPosY(120f, 0.5f);
        //animacion para arriba DOTWEEN
        Destroy(prefab, 0.5f);
    }
}


