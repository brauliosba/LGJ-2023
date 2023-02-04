using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public BattleSeriesManager battleSeriesManager;
    public LevelManager levelManager;
    public TutorialManager tutorialManager;
    private void Awake()
    {
        instance = this;
    }


}
