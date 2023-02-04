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
    private GameObject presicionContianer;
    [SerializeField]
    private HealthBar playerHealthBar;
    [SerializeField]
    private HealthBar enemyHealthBar;

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

        enemyHealthBar.gameObject.SetActive(false);
        presicionContianer.SetActive(false);
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

                playerHealthBar.gameObject.SetActive(true);
                playerHealthBar.HealthBarAnimation(playerHealth, 1, playerHealth, 1, null);

                enemyHealthBar.gameObject.SetActive(true);
                enemyHealthBar.HealthBarAnimation(enemyHealth, 1, enemyHealth, 1, PlayerTurn);
            });
        }
    }

    private void EnemyTurn()
    {
        //player.deffend
        //enemy.attack

        presicionContianer.SetActive(true);
        int baseDamage = player.Skills[0].Damage;
        //index es la dificultad del enemigo
        GameManager.instance.battleSeriesManager.AwakeBattleSeries(player.Skills[0].InputSeries,baseDamage, () => {
            presicionContianer.SetActive(false);

            playerHealth -= currentEnemy.Damage;
            float newHealth = (float)playerHealth / player.Health;
            playerHealthBar.HealthBarAnimation(playerHealth, newHealth, player.Health, 1.5f, PostEnemyAction);

        });

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
        List<InputSerie> inputSeries = player.Skills[index].InputSeries;
        int baseDamage = player.Skills[index].Damage;
        GameManager.instance.battleSeriesManager.AwakeBattleSeries(inputSeries,baseDamage, () => {
            presicionContianer.SetActive(false);
            if (index != 2)
            {
                int td = GameManager.instance.battleSeriesManager.inputManager.totalDamage;
                Debug.Log("Finish total damage " + td);
                enemyHealth -= td;
                float newHealth = (float)enemyHealth / currentEnemy.Health;
                enemyHealthBar.HealthBarAnimation(enemyHealth, newHealth, currentEnemy.Health, 1.5f, PostPlayerAction);
            }
            else
            {
                playerHealth += player.Skills[index].Damage;
                float newHealth = (float)playerHealth / player.Health;
                playerHealthBar.HealthBarAnimation(playerHealth, newHealth, player.Health, 1.5f, PostPlayerAction);
            }
        });

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
            presicionContianer.SetActive(true);
        }
    }
}
