using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public const float transformTime = 3.5f; // Time a person wait to be transform or killed
    public const int zombieProbability = 10;
    public const int zombieHealth = 40;
    public const int zombieAttackDamage = 40;

    public const float zombieSpeed = 10f;
    public const float peopleSpeed = 5f;
    public const float zombieAttackDistance = 2.0f;
    public const float peopleFearDistance = 5.0f;

    public List<EntityController> zombies = new List<EntityController>();
    public List<EntityController> people = new List<EntityController>();

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
        StartGame();
    }

    public void CreateGame()
    {
        onGameCreated.Invoke();
    }

    public void StartGame()
    {
        //Start logic
        onGameStarted.Invoke();
    }

    public void EndGame()
    {
        //End game logic
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
    }

    private void RemovePerson(EntityController person)
    {
        people.Remove(person);
    }

    public void ZombieDied(EntityController zombie) //We could add information about zombie
    {
        RemoveZombie(zombie);
        onZombieDeath.Invoke(zombie);

        if (zombies.Count == 0)
        {
            //        //TODO: Lose condition
            //        //TODO: trigger a couple of seconds to see everyone alive
            //        //TODO: trigger UI LOSE and Rewards
            EndGame();
        }
    }

    public void PersonConverted(EntityController person)
    {
        RemovePerson(person);
        AddZombie(person);
        onPersonConverted.Invoke(person);

        if (people.Count == 0)
        {
            //        //TODO: Win condition
            //        //TODO: trigger a couple of seconds to see everyone dead or transformed
            //        //TODO: trigger UI WIN and Rewards
            EndGame();
        }
    }
    #endregion
}