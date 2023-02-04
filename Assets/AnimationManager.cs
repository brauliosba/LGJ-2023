using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AnimationManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject stand;
    [SerializeField] private GameObject enemy;

    public void SetEnemy(GameObject prefab)
    {
        enemy = prefab;
    }
    int standxpos = -540;
    int playerxpos = -400;
    int enemyxpos = -400;
    public void ChooseAnimation(int index, System.Action endAttack)
    {
        RectTransform standRectTransform = stand.GetComponent<RectTransform>();
        RectTransform playerRectTransform = player.GetComponent<RectTransform>();
        RectTransform enemyRectTransform = enemy.GetComponent<RectTransform>();
        float timemove = 0.5f;
        switch (index)
        {
            case 0:
                print("weak anim");
                standRectTransform.DOAnchorPosX(standxpos+200, timemove);
                Timers.TimersManager.SetTimer(this, timemove, () =>
                {
                    enemyRectTransform.DOAnchorPosX(enemyxpos+200, timemove);
                });
                
                break;
            case 1:
                print("hard anim");
                standRectTransform.DOAnchorPosX(standxpos + 200, timemove);
                playerRectTransform.DOAnchorPosX(playerxpos + 200, timemove);
                Timers.TimersManager.SetTimer(this, timemove, () =>
                {
                    enemyRectTransform.DOAnchorPosX(enemyxpos + 200, timemove);
                });
                break;
            case 2:
                print("heal animation");
                break;
            case 3:
                print("def animation");
                enemyRectTransform.DOAnchorPosX(enemyxpos - 200, timemove);
                Timers.TimersManager.SetTimer(this, timemove, () =>
                {
                    standRectTransform.DOAnchorPosX(standxpos - 200, timemove);
                    playerRectTransform.DOAnchorPosX(playerxpos - 200, timemove);
                });
                break;

            default:
                break;
        }

        Timers.TimersManager.SetTimer(this, timemove*3, () =>
        {
            standRectTransform.DOAnchorPosX(standxpos, timemove);
            playerRectTransform.DOAnchorPosX(playerxpos, timemove);
            enemyRectTransform.DOAnchorPosX(enemyxpos, timemove);
            if (endAttack != null)
                endAttack.Invoke();
        });
    }
}


