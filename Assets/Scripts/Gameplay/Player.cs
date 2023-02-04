using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int health;

    [SerializeField]
    private List<SkillDataScriptable> skills;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public int Health { get { return health; } }
    public List<SkillDataScriptable> Skills { get { return skills; } }
}
