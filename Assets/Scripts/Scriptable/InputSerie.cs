using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Input Serie")]
public class InputSerie : ScriptableObject
{
    public string nameSerie;
    public List<InputGame> inputGame;

    public void Print()
    {
        Debug.Log("====Name series:" + nameSerie+"====");
        for (int i = 0; i < inputGame.Count; i++)
        {
            Debug.Log(inputGame[i].key);
        }
    }
    public int length
    {
        get { return inputGame.Count; }
    }
    public InputGame GetInputGame(int pos)
        { return inputGame[pos]; }
}