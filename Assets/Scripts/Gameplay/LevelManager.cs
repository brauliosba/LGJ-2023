using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject enemyContainer;
    [SerializeField]
    private GameObject optionsContianer;
    [SerializeField]
    private Slider playerHealthBar;
    [SerializeField]
    private Slider enemyHealthBar;

    [SerializeField]
    private List<SkillOption> skillOptions;

    [Header("Data")]
    [SerializeField]
    private LevelDataScriptable currentLevel;

    private List<GameObject> currentLevelEnemies;
    private Enemy currentEnemy;
    private int currentEnemyIndex = 0;
    private bool isEnemyAlive = false;
    private List<int> currentCooldowns = new List<int>();
    private int playerHealth;
    private int enemyHealth;

    private void Start()
    {
        currentLevelEnemies = currentLevel.Enemies;

        for (int i = 0; i < player.Skills.Count; i++)
        {
            currentCooldowns.Add(0);
        }

        playerHealth = player.Health;

        playerHealthBar.gameObject.SetActive(false);
        playerHealthBar.value = 0;
        DOTween.To(() => playerHealthBar.value, x => playerHealthBar.value = x, 1f, 1f);

        enemyHealthBar.gameObject.SetActive(false);
        optionsContianer.SetActive(false);
        InstantiateEnemy();
    }

    private void InstantiateEnemy()
    {
        if (!isEnemyAlive)
        {
            GameObject go = Instantiate(currentLevelEnemies[currentEnemyIndex], enemyContainer.transform);
            currentEnemy = go.GetComponent<Enemy>();
            currentEnemy.StartAnimation(() => {
                isEnemyAlive = true;
                enemyHealth = currentEnemy.Health;

                playerHealthBar.value = 0;
                playerHealthBar.gameObject.SetActive(true);
                DOTween.To(() => playerHealthBar.value, x => playerHealthBar.value = x, 1f, 1f);

                enemyHealthBar.value = 0;
                enemyHealthBar.gameObject.SetActive(true);
                DOTween.To(() => enemyHealthBar.value, x => enemyHealthBar.value = x, 1f, 1f).OnComplete(() => 
                {
                    PlayerTurn();
                });
            });
        }
    }

    private void EnemyTurn()
    {
        //player.deffend
        //enemy.attack

        playerHealth -= currentEnemy.Damage;
        float newHealth = (float)playerHealth / player.Health;
        DOTween.To(() => playerHealthBar.value, x => playerHealthBar.value = x, newHealth, 1.5f).SetEase(Ease.InQuad).OnComplete(() => PostEnemyAction());

    }

    private void PostEnemyAction()
    {
        if (playerHealth <= 0)
        {
            playerHealthBar.gameObject.SetActive(false);
            player.gameObject.SetActive(false);
        }
        else
        {
            PlayerTurn();
        }
    }

    private void PlayerTurn()
    {
        OpenActionMenu();
    }

    private void OpenActionMenu()
    {
        for (int i = 0; i < skillOptions.Count; i++)
        {
            if (currentCooldowns[i] != 0)
                currentCooldowns[i] -= 1;

            SkillOption skillOption = skillOptions[i];
            skillOption.SkillName.text = player.Skills[i].SkillName;
            if (currentCooldowns[i] != 0)
            {
                skillOption.SkillButton.interactable = false;
                skillOption.SkillCooldown.text = currentCooldowns[i].ToString();
                skillOption.SkillCooldown.gameObject.SetActive(true);
            }
            else
            {
                skillOption.SkillButton.interactable = true;
                skillOption.SkillCooldown.gameObject.SetActive(false);
            }
        }

        optionsContianer.SetActive(true);
    }

    private void DoAction(int index)
    {
        //player.attack
        //player.attackAnim

        if (index != 2)
        {
            enemyHealth -= player.Skills[index].Damage;
            float newHealth = (float)enemyHealth / currentEnemy.Health;
            DOTween.To(() => enemyHealthBar.value, x => enemyHealthBar.value = x, newHealth, 1.5f).SetEase(Ease.InQuad).OnComplete(() => PostPlayerAction());        
        }
        else
        {
            playerHealth += player.Skills[index].Damage;
            float newHealth = (float)playerHealth / player.Health;
            DOTween.To(() => playerHealthBar.value, x => playerHealthBar.value = x, newHealth, 1.5f).SetEase(Ease.InQuad).OnComplete(() => PostPlayerAction());
        }
    }

    private void PostPlayerAction()
    {
        if (enemyHealth <= 0)
        {
            //enemy.dieAnim
            enemyHealthBar.gameObject.SetActive(false);
            currentEnemy.gameObject.SetActive(false);

            Destroy(currentEnemy.gameObject);
            currentEnemyIndex++;
            isEnemyAlive = false;
            InstantiateEnemy();
        }
        else
        {
            EnemyTurn();
        }
    }

    public void OnActionButtonClicked(int index)
    {
        if (currentCooldowns[index] == 0)
        {
            currentCooldowns[index] = player.Skills[index].Cooldown;
            DoAction(index);
            optionsContianer.SetActive(false);
        }
    }
}
