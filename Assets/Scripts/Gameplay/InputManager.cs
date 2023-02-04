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
    private int perfectCount = 0;
    public int GetTotalDamage()
    {
        Debug.Log("Finish total damage " + totalDamage);
        Debug.Log("good count " + goodCount);
        Debug.Log("perfect count " + perfectCount);
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
    public void CheckInput()
    {
        GoalState state = GameManager.instance.battleSeriesManager.goalManager.state;
        InputArrowPrefab currentPrefab = GameManager.instance.battleSeriesManager.spawnManager.currentArrowPrefab();
        int baseDamage = GameManager.instance.battleSeriesManager.BaseDamage;
        int totalSeries = GameManager.instance.battleSeriesManager.spawnManager.TotalSeries;
        int damage = baseDamage / totalSeries;
        if (currentPrefab == null)
            return;

        if (Input.GetKeyDown(currentPrefab.inputGame.key) /*&& !currentPrefab.wasPresed*/)
        {
            currentPrefab.wasPresed = true;
            if (state == GoalState.good)
            {
                totalDamage += Mathf.FloorToInt(damage * 0.5f);
                goodCount++;
                FeedbackInput("Good!!");
            }
            else if (state == GoalState.perfect)
            {
                totalDamage += Mathf.FloorToInt(damage);
                perfectCount++;
                FeedbackInput("PERFECT!!");
            }
            else
            {
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
        prefab.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        prefab.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
        //animacion para arriba DOTWEEN
        Destroy(prefab, 0.5f);
    }
}


