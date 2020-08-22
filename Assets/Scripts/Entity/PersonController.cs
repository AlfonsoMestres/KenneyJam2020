﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonController : EntityController
{
    private float timePersonBetweenChecks = 3.0f;
    private float personCheckTimer;

    protected override void OnGameCreated()
    {
        base.OnGameCreated();

        gameController.AddPerson(this);
        navMeshAgent.speed = GameController.peopleSpeed;
        personCheckTimer = Random.Range(0.0f, timePersonBetweenChecks);
    }

    protected override void Update()
    {
        base.Update();
        if (timePersonBetweenChecks < personCheckTimer)
        {
            WatchOutForZombies();
            personCheckTimer = Random.Range(0.0f, timePersonBetweenChecks * 0.2f);
        }
        else
        {
            personCheckTimer += Time.deltaTime;
        }

        //Will this be used for both people and zombies?
        if (!deathIdle && health <= 0)
        {
            //deathIdle = true;
            //StartCoroutine("Death");
        }
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

    protected override void DeathBehaviour()
    {
        if (Random.Range(0.0f, 100.0f) <= GameController.zombieProbability)
        {
            //GameController.zombiesAlive = GameController.zombiesAlive + 1;
            healthBar.value = GameController.zombieHealth;
            healthBar.transform.Find("Fill Area").GetComponentInChildren<Image>().color = entityHPColor;
            // TODO: trigger effect 
            // TODO: change skin change skin
        }
        else
        {
            // TODO: remove life bar
        }
    }
}