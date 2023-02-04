using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Level", menuName = "ScriptableObjects/Data/Level", order = 1)]
public class LevelDataScriptable : ScriptableObject
{
    [SerializeField]
    private List<GameObject> enemies;

    public List<GameObject> Enemies { get { return enemies; } }
}
