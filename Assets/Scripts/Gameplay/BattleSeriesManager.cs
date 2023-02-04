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
    private System.Action endBattle;
    private int indexcommand;
    private void Start()
    {
        state = BattleState.idle;
    }
    private void Update()
    {
    }
    
    public void AwakeBattleSeries(List<InputSerie> inputSeries,int index,int baseDamage,bool isDefend, System.Action endBattle)
    {
        this.indexcommand = index;
        inputManager.ResetDamage();
        spawnManager.ResetSpawn();
        System.Action awakeAction = () =>
        {
            state = BattleState.awake;
            inputManager.StartManager(baseDamage, isDefend);

            spawnManager.CreateSeriesGameplay(inputSeries, () => { StartBattleSeries(); });
            this.endBattle = endBattle;
        };
        if (GameManager.instance.tutorialManager.IsFirstTime)
        {
            GameManager.instance.tutorialManager.OnTutorialInputSeries(awakeAction);
        }
        else
        {
            awakeAction.Invoke();
        }
    }

    public void StartBattleSeries()
    {
        state = BattleState.started;
    }


    public void EndBattleSeries()
    {
        Timers.TimersManager.SetTimer(this, 1f, () =>
        {
            GameManager.instance.animationManager.ChooseAnimation(this.indexcommand, ()=> 
            {
                state = BattleState.end;
                this.endBattle.Invoke();
            });
            
        });

    }
    public BattleState BattleState { get { return state; } }
}
