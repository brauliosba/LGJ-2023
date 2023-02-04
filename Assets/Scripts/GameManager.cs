using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public BattleSeriesManager battleSeriesManager;
    private void Awake()
    {
        instance = this;
    }


}
