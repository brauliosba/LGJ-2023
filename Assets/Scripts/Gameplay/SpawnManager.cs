using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timers;
using System.Linq;
public class SpawnManager : MonoBehaviour
{
    [Header("Input series")]
   // [SerializeField] private List<InputSerie> seriesData;
    [SerializeField] private InputSerie seriesGameplay;
    [SerializeField] private InputArrowPrefab inputArrowPrefab;
    [Header("spawn objects scene")]
    [SerializeField] private GameObject spawnGO;
    [SerializeField] private GameObject goalGO;
    [Header("spawn objects variables")]
    [SerializeField] private float velocity;

    [SerializeField] private float timeRateInputPrefabSpawn;
    private int currentInputInstatiate = 0;
    int totalSeries = 0;

    public InputArrowPrefab currentArrowPrefab()
    {
        return spawnGO.GetComponentsInChildren<InputArrowPrefab>().ToList().Find(p => p.isCurrent);
    }
    public void UpdateCurrentArrow(InputArrowPrefab inputArrowPrefab)
    {
        foreach (var item in spawnGO.GetComponentsInChildren<InputArrowPrefab>())
        {
            item.isCurrent = false;
        }
        inputArrowPrefab.isCurrent = true;
    }
    public void CreateSeriesGameplay(List<InputSerie> seriesData, System.Action startAction)
    {

        int x = Random.Range(0, seriesData.Count);
        seriesData[x].Print();
        seriesGameplay = Instantiate(seriesData[x]);
        totalSeries = seriesGameplay.length;
        TimersManager.SetTimer(this, 2, () =>
        {
            Debug.Log("====Begin battle====");

            if (startAction != null)
                startAction.Invoke();

            LoopSeriesGamePlay();
        });
    }
    public int TotalSeries { get { return totalSeries; } }
    public void ResetSpawn()
    {
        //currentInputArrowPrefab = null;
       // seriesData.Clear();
        currentInputInstatiate = 0;
    }
    public void CheckIsCurrentPrefabIsTheLastOne(InputArrowPrefab prefab)
    {
        if (prefab.lastone)
        {
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

