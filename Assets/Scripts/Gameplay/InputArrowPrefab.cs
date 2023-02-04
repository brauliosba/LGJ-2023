using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputArrowPrefab : MonoBehaviour
{
    [SerializeField] Image circle;
    [SerializeField] Image arrow;
    public KeyCode keyCode;
    public InputGame inputGame;
    public bool wasPresed;
    public bool isMoving;
    public bool isCurrent;
    private float velocity;
    private RectTransform rectTransform;
    public bool lastone = false;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void InitPrefab(InputGame inputGame, float velocity, bool lastone)
    {
        this.lastone = lastone;
        this.wasPresed = false;
        this.inputGame = inputGame;
        this.velocity = velocity;
        arrow.GetComponent<RectTransform>().rotation = Quaternion.Euler(0,0,inputGame.rotate);
        circle.color = inputGame.color;
        keyCode = inputGame.key;
    }
    public void Destroy()
    {
        this.gameObject.SetActive(false);
        isMoving = false;
        isCurrent = false;
        Vector2 position = rectTransform.anchoredPosition;
        position.x = -2000;
        rectTransform.anchoredPosition = position;
    }
    private void Update()
    {
        if (isMoving)
        {
            Move();
        }

    }

    private void Move()
    {
        Vector2 position = rectTransform.anchoredPosition;

        position.x -= velocity * 10f * Time.deltaTime;
        rectTransform.anchoredPosition = position;
    }
}
