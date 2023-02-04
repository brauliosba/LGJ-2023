using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI totalDamageFeedback;
    [SerializeField] private TMPro.TextMeshProUGUI goodFeedback;
    [SerializeField] private TMPro.TextMeshProUGUI perfectFeedback;
    public int totalDamage = 0;
    public int goodCount = 0;
    public int perfectCount = 0;
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
        InputArrowPrefab currentPrefab = GameManager.instance.battleSeriesManager.spawnManager.currentInputArrowPrefab;
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
            }
            else if (state == GoalState.perfect)
            {
                totalDamage += Mathf.FloorToInt(damage);
                perfectCount++;
            }
            else
            {
                print("miss");
            }
            GameManager.instance.battleSeriesManager.spawnManager.CheckIsCurrentPrefabIsTheLastOne(currentPrefab);
            Destroy(currentPrefab.gameObject);
        }
    }
    public void ResetDamage()
    {
        totalDamage = 0;
    }
}

