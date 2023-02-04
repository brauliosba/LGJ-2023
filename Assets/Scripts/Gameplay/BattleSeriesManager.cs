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
    int baseDamage;
    
    public void AwakeBattleSeries(List<InputSerie> inputSeries,int baseDamage, System.Action endBattle)
    {
        inputManager.ResetDamage();
        spawnManager.ResetSpawn();

        state = BattleState.awake;
        this.baseDamage = baseDamage;
        
        spawnManager.CreateSeriesGameplay(inputSeries, ()=> { StartBattleSeries(); });
        this.endBattle = endBattle;
    }

    public void StartBattleSeries()
    {
        state = BattleState.started;
    }


    public void EndBattleSeries()
    {
        Timers.TimersManager.SetTimer(this, 1f, () =>
        {
            state = BattleState.end;
            this.endBattle.Invoke();
        });

    }
    public BattleState BattleState { get { return state; } }
    public int BaseDamage { get { return baseDamage; } }
}
