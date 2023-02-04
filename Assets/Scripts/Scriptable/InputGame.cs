using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "Input Game")]
public class InputGame : ScriptableObject
{
    public KeyCode key;
    public int rotate;
    public Color color;
}
