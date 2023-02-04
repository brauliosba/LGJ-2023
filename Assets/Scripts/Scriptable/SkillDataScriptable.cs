using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Skill", menuName = "ScriptableObjects/Data/Skill", order = 2)]
public class SkillDataScriptable : ScriptableObject
{
    [SerializeField]
    private string skillName;
    [SerializeField]
    private int cooldown;
    [SerializeField]
    private int damage;

    public string SkillName { get { return skillName; } }
    public int Cooldown { get { return cooldown; } }
    public int Damage { get { return damage; } }
}
