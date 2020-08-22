using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static float transformTime = 3.5f; // Time a person wait to be transform or killed
    public static int zombieProbability = 100;
    public static int zombieHealth = 40;
    public static int zombieAttackDamage = 40;

    public static float zombieSpeed = 5f;
    public static float peopleSpeed = 5f;
    public static float zombieAttackDistance = 2.0f;
    public static float peopleFearDistance = 8.0f;

    public static int curseAmount = 1;

    public List<EntityController> zombies = new List<EntityController>();
    public List<EntityController> people = new List<EntityController>();

    private Camera mainCamera;

    public Transform zombiePrefab;
    public Text winLoseText;
    public Texture2D[] mouseSprites;

    private bool hasGameStarted;

    private Text curseTouchAmountText;
    private Text civiliansAliveAmountText;

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
    public IEventSubscribe OnGameFinished { get { return onGameFinished; } }
    public IEventSubscribe<EntityController> OnPersonConverted { get { return onPersonConverted; } }
    public IEventSubscribe<EntityController> OnZombieDeath { get { return onZombieDeath; } }

    private Event onGameCreated = new Event();
    private Event onGameStarted = new Event();
    private Event onGameFinished = new Event();

    private Event<EntityController> onPersonConverted = new Event<EntityController>();
    private Event<EntityController> onZombieDeath = new Event<EntityController>();

    #endregion
    private void Start()
    {
        CreateGame();
        Cursor.SetCursor(mouseSprites[0], Vector2.zero, CursorMode.Auto);
        StartGame();
    }


    public void CreateGame()
    {
        onGameCreated.Invoke();
    }

    private void Update()
    {
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
                        curseAmount = curseAmount - 1;
                        curseTouchAmountText.text = curseAmount.ToString();
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
        hasGameStarted = true;
        curseTouchAmountText.text = curseAmount.ToString();
    }

    public void EndGame(bool win)
    {
        //End game logic
        SavePrefs();
        gameHasEnded = true;
        playerWon = win;
        Invoke("ChangeEndGameText", 3.0f);

    }

    private void SavePrefs()
    {
        PlayerPrefs.SetInt("Currency", cursedHeartsObtained);
    }

    private void LoadPrefs()
    {
        cursedHeartsObtained = PlayerPrefs.GetInt("Currency", 10);
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

        onGameFinished.Invoke();
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

    public void PersonConverted(EntityController person)
    {
        Transform personTransform = person.gameObject.transform;
        RemovePerson(person);
        onPersonConverted.Invoke(person);
        Destroy(person.gameObject);
        ++cursedHeartsObtained;
        Instantiate(zombiePrefab, personTransform.position, personTransform.rotation);

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

    }

    public void OnPlayAgainClicked()
    {
        SceneManager.LoadScene("WhiteBox");
    }

    #endregion

}