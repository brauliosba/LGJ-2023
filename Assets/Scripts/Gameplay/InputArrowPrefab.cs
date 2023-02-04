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
  //  public int index;
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

    private void Update()
    {
        if (isMoving)
        {
            Move();
        }

    }

    public void Stop()
    {
        isMoving = false;
    }
    private void Move()
    {
        Vector2 position = rectTransform.anchoredPosition;

        position.x -= velocity * 10f * Time.deltaTime;
        rectTransform.anchoredPosition = position;
    }
}
