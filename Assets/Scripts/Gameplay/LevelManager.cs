using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private TMPro.TextMeshProUGUI endgameTxt;
    [SerializeField]
    private GameObject endgameContainer;
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
    public bool isTesting;
    private void Update()
    {
        
        if (isTesting && Input.GetKeyDown(KeyCode.Space))
        {
            GameOver(true);
        }
        
    }
    private void Start()
    {
        AudioManager.instance.PlayMusicFade("battle", true);
        currentLevelEnemies = currentLevel.Enemies;

        for (int i = 0; i < player.Skills.Count; i++)
        {
            currentCooldowns.Add(0);
        }

        playerHealth = player.Health;

        playerHealthBar.gameObject.SetActive(false);


        endgameContainer.SetActive(false);
        enemyHealthBar.gameObject.SetActive(false);
        presicionContianer.SetActive(false);
        optionsContianer.SetActive(false);
        InstantiateEnemy();
    }

    private void InstantiateEnemy()
    {
        if (currentEnemyIndex >= currentLevelEnemies.Count)
        {
            GameOver(true);
            //print("current index" + currentEnemyIndex);
        }

        if (!isEnemyAlive)
        {
            GameObject go = Instantiate(currentLevelEnemies[currentEnemyIndex], enemyContainer.transform);
            GameManager.instance.animationManager.SetEnemy(go);
            currentEnemy = go.GetComponent<Enemy>();
            currentEnemy.StartAnimation(() => {
                isEnemyAlive = true;
                enemyHealth = currentEnemy.Health;

                playerHealthBar.gameObject.SetActive(true);
                playerHealthBar.HealthBarAnimation(playerHealth, 1, player.Health, 1, null);

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
        MovePresicionContainer(new Vector2(740, 0));
        int c = currentEnemyIndex == 2 ? 3 : currentEnemyIndex;
        int baseDamage = player.Skills[c].Damage;
        
        //index es la dificultad del enemigo
        GameManager.instance.battleSeriesManager.AwakeBattleSeries(
            player.Skills[c].InputSeries,
            3,
            baseDamage,
            true,
            () => {
            int td = GameManager.instance.battleSeriesManager.inputManager.GetTotalDamage();
            //Debug.Log("Finish total damage " + td);
            presicionContianer.SetActive(false);
            playerHealth = playerHealth - (currentEnemy.Damage - td) < 0 ? 0 : playerHealth - (currentEnemy.Damage - td);
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
            GameOver(false);
        }
        else
        {
            PlayerTurn();
        }
    }

    private void PlayerTurn()
    {
        OpenActionMenu();
        if (GameManager.instance.tutorialManager.IsFirstTime)
        {
            GameManager.instance.tutorialManager.OnTutorialCommand(null);
        }
        
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
    private void MovePresicionContainer(Vector2 toPosition)
    {
        presicionContianer.GetComponent<RectTransform>().anchoredPosition = toPosition;
    }
    private void DoAction(int index)
    {
        MovePresicionContainer(Vector2.zero);
        List<InputSerie> inputSeries = player.Skills[index].InputSeries;
        int baseDamage = player.Skills[index].Damage;
        GameManager.instance.battleSeriesManager.AwakeBattleSeries(
            inputSeries,
            index,
            baseDamage,
            false, 
            () => {
            presicionContianer.SetActive(false);
            if (index != 2)
            {
                int td = GameManager.instance.battleSeriesManager.inputManager.GetTotalDamage();
                
                enemyHealth = (enemyHealth - td) < 0 ? 0 : enemyHealth - td;

                float newHealth = (float)enemyHealth / currentEnemy.Health;
                enemyHealthBar.HealthBarAnimation(enemyHealth, newHealth, currentEnemy.Health, 1.5f, PostPlayerAction);
            }
            else
            {
               int td = GameManager.instance.battleSeriesManager.inputManager.GetTotalDamage();

                playerHealth = (playerHealth + td) > player.Health ? player.Health : playerHealth + td;
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
    public void GameOver(bool playerWin)
    {
        endgameContainer.SetActive(true);
        if (playerWin)
        {
            endgameTxt.text = "Ganaste !!";
        }
        else
        {
            endgameTxt.text = "Perdiste";
        }
        /*
        Timers.TimersManager.SetTimer(this, 1f, () => 
        {
            SceneManager.LoadScene("Credits");
        });
        */
    }

    public void GoCredits()
    {
        AudioManager.instance.StopMusic();
        SceneManager.LoadScene("Credits");
    }

    public void Restart()
    {
        AudioManager.instance.StopMusic();
        SceneManager.LoadScene("LevelTesting -dev-Y");
    }
}
