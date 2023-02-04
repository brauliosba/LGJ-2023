using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Sound", menuName = "ScriptableObjects/Data/Sound", order = 3)]
public class SoundScriptable : ScriptableObject
{
    public string Name;
    public AudioClip AudioClip;
    [Range(0.0f, 1.0f)]
    public float Volume;
}
