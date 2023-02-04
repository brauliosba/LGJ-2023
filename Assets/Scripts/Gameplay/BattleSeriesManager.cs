using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    idle,
    awake,
    started,
    end,
}

public class BattleSeriesManager : MonoBehaviour
{
    [SerializeField]private BattleState state;
    public SpawnManager spawnManager;
    public GoalManager goalManager;
    public InputManager inputManager;

    private void Start()
    {
        state = BattleState.idle;
    }
    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AwakeBattleSeries(1);
        }
        */
    }
    System.Action endBattle;
    public void AwakeBattleSeries(int dificulty, System.Action endBattle)
    {
        state = BattleState.awake;
        
        spawnManager.CreateSeriesGameplay(()=> { StartBattleSeries(); });
        this.endBattle = endBattle;
    }

    public void StartBattleSeries()
    {
        state = BattleState.started;
    }


    public void EndBattleSeries()
    {
        state = BattleState.end;
        inputManager.ResetDamage();
        spawnManager.ResetSpawn();
        this.endBattle.Invoke();
    }
    public BattleState BattleState { get { return state; } }
}
