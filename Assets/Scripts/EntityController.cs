﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EntityController : MonoBehaviour
{
    public Color zombieHPColor;
    public Slider healthBar;
    public int health = 100;

    // TODO: Update health bar when health value is affected

    public bool isZombie;

    private bool deathIdle; // This will avoid multiple calls to the coroutine
    private NavMeshAgent navMeshAgent;

    private float timeZombieBetweenChecks = 0.5f;
    private float zombieCheckTimer;

    private float timePersonBetweenChecks = 3.0f;
    private float personCheckTimer;

    private GameController gameController = null;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController")?.GetComponent<GameController>();
        gameController.OnGameCreated.Subscribe(OnGameCreated);

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnGameCreated()
    {
        if (isZombie)
        {
            gameController.AddZombie(this);
            navMeshAgent.speed = GameController.zombieSpeed;
        }
        else
        {
            gameController.AddPerson(this);
            navMeshAgent.speed = GameController.peopleSpeed;
            personCheckTimer = Random.Range(0.0f, timePersonBetweenChecks);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        healthBar.value = health;
    }

    private void SearchForNewVictim()
    {
        GameObject target = null;
        EntityController targetController = null;
        float minDistance = 10000000; //needs to be BIG the first time
        foreach (var person in gameController.people)
        {
            var distance = Vector3.Distance(person.gameObject.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                target = person.gameObject;
                targetController = person;
            }
        }
        if (target != null)
        {
            navMeshAgent.SetDestination(target.transform.position);
            if (minDistance < GameController.zombieAttackDistance)
            {
                zombieCheckTimer = -2.0f; //this way they remain in place
                targetController.TakeDamage(GameController.zombieAttackDamage);
                navMeshAgent.ResetPath();
            }
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
    }

    private void WatchOutForZombies()
    {
        GameObject closestZombie = null;
        float minDistance = 10000000; //needs to be BIG the first time
        foreach (var zombie in gameController.zombies)
        {
            var distance = Vector3.Distance(zombie.gameObject.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestZombie = zombie.gameObject;
            }
        }
        if (closestZombie != null && minDistance < GameController.peopleFearDistance)
        {
            Vector3 direction = Vector3.Normalize(transform.position - closestZombie.gameObject.transform.position);
            navMeshAgent.SetDestination(transform.position + direction * 20.0f);

        }
        else
        {
            float xRand = Random.Range(-1f, 1f);
            float zRand = Random.Range(-1f, 1f);

            Vector3 randomDirection = Vector3.Normalize(new Vector3(xRand, 0f, zRand));

            navMeshAgent.SetDestination(transform.position + randomDirection * 5.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health;

        if (isZombie)
        {
            //Basic zombie behaviour
            if (timeZombieBetweenChecks < zombieCheckTimer)
            {
                zombieCheckTimer = 0.0f;
                SearchForNewVictim();
            }
            else
            {
                zombieCheckTimer += Time.deltaTime;
            }
        }
        else
        {
            if (timePersonBetweenChecks < personCheckTimer)
            {
                WatchOutForZombies();
                personCheckTimer = Random.Range(0.0f, timePersonBetweenChecks * 0.2f);
            }
            else
            {
                personCheckTimer += Time.deltaTime;
            }



            if (!deathIdle && health <= 0)
            {
                //deathIdle = true;
                //StartCoroutine("Death");
            }
        }



        //Will this be used for both people and zombies?

    }

    IEnumerator Death()
    {
        //TODO: trigger death animation
        yield return new WaitForSeconds(GameController.transformTime);

        if (isZombie)
        {
            //this has to happen every 1 sec or so better than in any frame
        }
        else
        {
            // If the guy is not a zombie, take the chance to transform it
            isZombie = Random.Range(0.0f, 100.0f) <= GameController.zombieProbability;
            if (isZombie)
            {
                //GameController.zombiesAlive = GameController.zombiesAlive + 1;
                healthBar.value = GameController.zombieHealth;
                healthBar.transform.Find("Fill Area").GetComponentInChildren<Image>().color = zombieHPColor;
                // TODO: trigger effect 
                // TODO: change skin change skin
            }
            else
            {
                // TODO: remove life bar
            }

        }

        deathIdle = false;

    }

}