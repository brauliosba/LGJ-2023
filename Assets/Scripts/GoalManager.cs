using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GoalState
{
    good,
    perfect,
    miss,
}
public class GoalManager : MonoBehaviour
{
    public GoalState state;
    private void Start()
    {
        state = GoalState.miss;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Good"))
        {
            state = GoalState.good;
        }
        if (collision.name.Contains("Perfect"))
        {
            state = GoalState.perfect;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name.Contains("Back"))
        {
            state = GoalState.miss;
        }
    }

   
}
