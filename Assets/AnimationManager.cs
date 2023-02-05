using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    int standxpos = -544;
    int playerxpos = -548;
    int enemyxpos = -400;
    public void PrepAnimation()
    {
        Animator enemyAnimator = enemy.GetComponent<Animator>();
        enemyAnimator.Play("prep");
    }
    public void ChooseAnimation(int index, System.Action endAttack)
    {
        RectTransform standRectTransform = stand.GetComponent<RectTransform>();
        RectTransform playerRectTransform = player.GetComponent<RectTransform>();
        RectTransform enemyRectTransform = enemy.GetComponent<RectTransform>();

        Animator enemyAnimator = enemy.GetComponent<Animator>();
        Animator standAnimator = stand.GetComponent<Animator>();
        Animator playerAnimator = player.GetComponent<Animator>();
        //enemyAnimator.Play("attack");

        Image enemyImage = enemy.GetComponent<Image>();
        Image playerImage = player.GetComponent<Image>();
        Image standImage = stand.GetComponent<Image>();
        float timemove = 0.5f;
        switch (index)
        {
            case 0:
                print("weak anim");
                standAnimator.Play("weakattack");
                //playerAnimator.Play("weakattack");


                Timers.TimersManager.SetTimer(this, timemove, () =>
                {
                    AudioManager.instance.PlaySFX("punch");
                    enemyRectTransform.DOAnchorPosX(enemyxpos + 100, timemove);
                    DOTween.Sequence().AppendInterval(0.2f).Append(enemyImage.DOFade(0.3f, 0.3f).SetLoops(4, LoopType.Yoyo));
                });
                
                break;
            case 1:
                print("hard anim");

                standRectTransform.DOAnchorPosX(standxpos + 200, timemove).OnComplete(() =>
                {
                    AudioManager.instance.PlaySFX("punch");
                });
                playerRectTransform.DOAnchorPosX(playerxpos + 200, timemove).OnComplete(() =>
                {
                    AudioManager.instance.PlaySFX("knife");
                });
                Timers.TimersManager.SetTimer(this, timemove, () =>
                {
                    standAnimator.Play("strongattack");
                    playerAnimator.Play("strongattack");
                    Timers.TimersManager.SetTimer(this, 0.15f, () =>
                    {
                        enemyRectTransform.DOAnchorPosX(enemyxpos + 100, timemove);
                        DOTween.Sequence().AppendInterval(0.2f).Append(enemyImage.DOFade(0.3f, 0.3f).SetLoops(4, LoopType.Yoyo));
                    });
                });
                break;
            case 2:
                print("heal animation");
                standRectTransform.DOAnchorPosY(-20f,0.25f).OnComplete(() => standRectTransform.DOAnchorPosY(8f, 0.25f));
                AudioManager.instance.PlaySFX("heal");
                break;
            case 3:
                print("def animation");

                enemyRectTransform.DOAnchorPosX(enemyxpos - 200, timemove);
                Timers.TimersManager.SetTimer(this, timemove, () =>
                {
                    enemyAnimator.Play("attack");
                    Timers.TimersManager.SetTimer(this, timemove, () =>
                    {
                        standRectTransform.DOAnchorPosX(standxpos - 100, timemove);
                        playerRectTransform.DOAnchorPosX(playerxpos - 100, timemove);
                        DOTween.Sequence().AppendInterval(0.2f).AppendCallback(() =>
                        {
                            playerImage.DOFade(0.3f, 0.3f).SetLoops(4, LoopType.Yoyo);
                            standImage.DOFade(0.3f, 0.3f).SetLoops(4, LoopType.Yoyo);
                        });
                    });
                });
                
                break;

            default:
                break;
        }

        Timers.TimersManager.SetTimer(this, timemove*3, () =>
        {
            enemyAnimator.Play("idle");
            standRectTransform.DOAnchorPosX(standxpos, timemove);
            playerRectTransform.DOAnchorPosX(playerxpos, timemove);
            enemyRectTransform.DOAnchorPosX(enemyxpos, timemove);
            if (endAttack != null)
                endAttack.Invoke();
        });
    }
}


