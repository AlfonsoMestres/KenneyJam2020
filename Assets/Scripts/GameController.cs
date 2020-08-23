using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    public Text cursedHeartsShop;
    public Text cursedHeartsGame;

    public static float transformTime = 3.5f; // Time a person wait to be transform or killed
    public static float zombieProbability = 100;
    public static int zombieHealth = 40;
    public static float zombieHealthDecay = 10f;
    public static float zombieAttackDamage = 40f;

    public static float zombieSpeed = 5f;
    public static float peopleSpeed = 5f;
    public static float zombieAttackDistance = 3.0f;
    public static float peopleFearDistance = 8.0f;
    public bool hasGameStarted;

    public static float curseAmount = 1;

    public List<EntityController> zombies = new List<EntityController>();
    public List<EntityController> people = new List<EntityController>();

    private Camera mainCamera;

    public Transform zombiePrefab;
    public Text winLoseText;
    public Texture2D[] mouseSprites;
    public GameObject shopCanvas; 

    private Text curseTouchAmountText;
    private Text civiliansAliveAmountText;

    public StatsController healthController;
    public StatsController attackController;
    public StatsController speedController;
    public StatsController curseController;
    public StatsController initialTouchesController;

    private bool playerWon;
    private bool gameHasEnded;

    public static int cursedHeartsObtained = 10;

    private void Awake()
    {
        mainCamera = Camera.main;
        curseTouchAmountText = GameObject.Find("CurseTouchAmount").GetComponent<Text>();
        civiliansAliveAmountText = GameObject.Find("CiviliansRemainingAmount").GetComponent<Text>();
    }

    #region Events
    public IEventSubscribe OnGameCreated { get { return onGameCreated; } }
    public IEventSubscribe OnGameStarted { get { return onGameStarted; } }
    public IEventSubscribe OnCursedTouch { get { return onCursedTouch; } }
    public IEventSubscribe<bool> OnGameFinished { get { return onGameFinished; } }
    public IEventSubscribe<bool> OnGameFinishedText { get { return onGameFinishedText; } }
    public IEventSubscribe<EntityController> OnPersonConverted { get { return onPersonConverted; } }
    public IEventSubscribe<EntityController> OnZombieDeath { get { return onZombieDeath; } }

    private Event onGameCreated = new Event();
    private Event onGameStarted = new Event();
    private Event onCursedTouch = new Event();
    private Event<bool> onGameFinished = new Event<bool>();
    private Event<bool> onGameFinishedText = new Event<bool>();

    private Event<EntityController> onPersonConverted = new Event<EntityController>();
    private Event<EntityController> onZombieDeath = new Event<EntityController>();

    #endregion
    private void Start()
    {
        CreateGame();
        Cursor.SetCursor(mouseSprites[0], Vector2.zero, CursorMode.Auto);
    }


    public void CreateGame()
    {
        onGameCreated.Invoke();
    }

    private void Update()
    {
        UpdateCursedHearts();

        if (hasGameStarted && !gameHasEnded)
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var personController = hit.collider.gameObject.GetComponentInChildren<PersonController>();
                if (personController != null)
                {
                    Cursor.SetCursor(mouseSprites[1], Vector2.zero, CursorMode.Auto);
                    if (Input.GetMouseButtonDown(0) && curseAmount > 0)
                    {
                        personController.Die();
                        personController.diedFromTouch = true;
                        --curseAmount;
                        curseTouchAmountText.text = ((int)curseAmount).ToString();
                        onCursedTouch.Invoke();
                    }
                }
                else
                {
                    Cursor.SetCursor(mouseSprites[0], Vector2.zero, CursorMode.Auto);
                }
            }
        }
    }

    public void StartGame()
    {
        //Start logic
        onGameStarted.Invoke();
        LoadPrefs();
        SetStatsValues();
        hasGameStarted = true;
        curseTouchAmountText.text = curseAmount.ToString();
    }

    private void SetStatsValues()
    {
        zombieHealthDecay = healthController.GetCurrentStatValue();
        zombieAttackDamage = attackController.GetCurrentStatValue();
        zombieProbability = curseController.GetCurrentStatValue();
        zombieSpeed = speedController.GetCurrentStatValue();
        curseAmount = initialTouchesController.GetCurrentStatValue();
    }

    public void EndGame(bool win)
    {
        //End game logic
        SavePrefs();
        gameHasEnded = true;
        playerWon = win;
        Invoke("ChangeEndGameText", 3.0f);
        onGameFinished.Invoke(win);
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetInt("Currency", cursedHeartsObtained);
        PlayerPrefs.SetInt("healthIndex", healthController.currentIndex);
        PlayerPrefs.SetInt("attackIndex", attackController.currentIndex);
        PlayerPrefs.SetInt("speedIndex", speedController.currentIndex);
        PlayerPrefs.SetInt("curseIndex", curseController.currentIndex);
        PlayerPrefs.SetInt("touchesIndex", initialTouchesController.currentIndex);
    }

    private void LoadPrefs()
    {
        cursedHeartsObtained = PlayerPrefs.GetInt("Currency", 0);
        healthController.currentIndex = PlayerPrefs.GetInt("healthIndex", 0);
        attackController.currentIndex = PlayerPrefs.GetInt("attackIndex", 0);
        speedController.currentIndex = PlayerPrefs.GetInt("speedIndex", 0);
        curseController.currentIndex = PlayerPrefs.GetInt("curseIndex", 0);
        initialTouchesController.currentIndex = PlayerPrefs.GetInt("touchesIndex", 0);

        //WHY IS THIS HERE
        cursedHeartsShop.text = cursedHeartsObtained.ToString();
        cursedHeartsGame.text = cursedHeartsObtained.ToString();
    }

    public void UpdateCursedHearts()
    {
        cursedHeartsGame.text = cursedHeartsObtained.ToString();
        cursedHeartsShop.text = cursedHeartsObtained.ToString();
    }

    private void ChangeEndGameText()
    {

        winLoseText.gameObject.SetActive(true);
        if (playerWon)
        {
            winLoseText.text = "YOU WIN";
        }
        else
        {
            winLoseText.text = "YOU LOSE";
        }

        onGameFinishedText.Invoke(playerWon);
    }

    #region People & Zombie logic
    public void AddZombie(EntityController zombie)
    {
        zombies.Add(zombie);
    }

    private void RemoveZombie(EntityController zombie)
    {
        zombies.Remove(zombie);
    }

    public void AddPerson(EntityController person)
    {
        people.Add(person);
        civiliansAliveAmountText.text = people.Count.ToString();
    }

    private void RemovePerson(EntityController person)
    {
        people.Remove(person);
        civiliansAliveAmountText.text = people.Count.ToString();
    }

    public void ZombieDied(EntityController zombie) //We could add information about zombie
    {
        RemoveZombie(zombie);
        onZombieDeath.Invoke(zombie);

        if (zombies.Count == 0 && !gameHasEnded)
        {
            //        //TODO: Lose condition
            //        //TODO: trigger a couple of seconds to see everyone alive
            //        //TODO: trigger UI LOSE and Rewards
            EndGame(false);
        }
    }

    public void PersonConverted(EntityController person, bool fromTouch = false)
    {
        Transform personTransform = person.gameObject.transform;
        RemovePerson(person);
        onPersonConverted.Invoke(person);
        Destroy(person.gameObject);
        ++cursedHeartsObtained;

        if (fromTouch || Random.Range(0.0f, 100.0f) <= zombieProbability)
        {
            Instantiate(zombiePrefab, personTransform.position, personTransform.rotation);
        }

        if (people.Count == 0 && !gameHasEnded)
        {
            //        //TODO: Win condition
            //        //TODO: trigger a couple of seconds to see everyone dead or transformed
            //        //TODO: trigger UI WIN and Rewards
            EndGame(true);
        }
    }


    #endregion

    #region EndGamePopup

    public void OnGoToShopClicked()
    {
        shopCanvas.SetActive(true);
        healthController.SetSlider();
        attackController.SetSlider();
        speedController.SetSlider();
        curseController.SetSlider();
        initialTouchesController.SetSlider();
    }

    public void OnPlayAgainClicked()
    {
        SceneManager.LoadScene("WhiteBox");
    }

    #endregion

}