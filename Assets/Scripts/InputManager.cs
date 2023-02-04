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
        if (currentPrefab == null)
            return;

        if (Input.GetKeyDown(currentPrefab.inputGame.key) /*&& !currentPrefab.wasPresed*/)
        {
            currentPrefab.wasPresed = true;
            if (state == GoalState.good)
            {
                totalDamage += 10;
                goodCount++;
            }
            else if (state == GoalState.perfect)
            {
                totalDamage += 20;
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

