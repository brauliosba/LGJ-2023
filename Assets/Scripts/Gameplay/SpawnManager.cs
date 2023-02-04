using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timers;
public class SpawnManager : MonoBehaviour
{
    [Header("Input series")]
    [SerializeField] private List<InputSerie> seriesData;
    [SerializeField] private InputSerie seriesGameplay;
    [SerializeField] private InputArrowPrefab inputArrowPrefab;
    [Header("spawn objects scene")]
    [SerializeField] private GameObject spawnGO;
    [SerializeField] private GameObject goalGO;
    [Header("spawn objects variables")]
    [SerializeField] private float velocity;

    [SerializeField] private float timeRateInputPrefabSpawn;
    public InputArrowPrefab currentInputArrowPrefab = null;
    private int currentInputInstatiate = 0;

    public void CreateSeriesGameplay(System.Action startAction)
    {

        int x = Random.Range(0, seriesData.Count);
        seriesData[x].Print();
        seriesGameplay = Instantiate(seriesData[x]);

        TimersManager.SetTimer(this, 2, () =>
        {
            Debug.Log("====Begin battle====");

            if (startAction != null)
                startAction.Invoke();

            LoopSeriesGamePlay();
        });
    }

    public void ResetSpawn()
    {
        currentInputArrowPrefab = null;
       // seriesData.Clear();
        currentInputInstatiate = 0;
    }
    public void CheckIsCurrentPrefabIsTheLastOne(InputArrowPrefab prefab)
    {
        if (prefab.lastone)
        {
            int td = GameManager.instance.battleSeriesManager.inputManager.totalDamage;
            Debug.Log("Finish total damage " + td);
            GameManager.instance.battleSeriesManager.EndBattleSeries();
            
        } 
    }
    private void LoopSeriesGamePlay()
    {
        Debug.Log("spawn " + currentInputInstatiate);
        bool islastOne = currentInputInstatiate > seriesGameplay.length-2;
        Spawn(seriesGameplay.GetInputGame(currentInputInstatiate),islastOne);
        currentInputInstatiate++;
        if (currentInputInstatiate > seriesGameplay.length - 1)
        {
            
            return;
        }
        Timer timer = new Timer(timeRateInputPrefabSpawn, () =>
        {
            LoopSeriesGamePlay();
        });
        TimersManager.SetTimer(this, timer);
    }
    private void Spawn(InputGame inputGame,bool lastone)
    {
        InputArrowPrefab prefab = Instantiate(inputArrowPrefab, spawnGO.transform);
        prefab.InitPrefab(inputGame, velocity, lastone);
    }
    public GameObject GoalGO { get { return goalGO; } }
}

