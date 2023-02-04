using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private string enemyName;
    [SerializeField]
    private int damage;
    [SerializeField]
    private int health;

    private RectTransform rectTransform;
    private Vector2 naturalPosition;

    private void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();

        rectTransform.DOAnchorPosX(-400f, 1.5f).OnComplete(() => naturalPosition = rectTransform.position);
    }

    public int Health { get { return health; } }

    public int Damage { get { return damage; } }
}
