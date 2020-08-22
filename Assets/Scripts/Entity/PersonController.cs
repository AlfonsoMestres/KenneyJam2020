using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonController : EntityController
{
    public float timePersonBetweenChecks = 1.5f;
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

        if (!gameController.hasGameStarted) return;

        if (timePersonBetweenChecks < personCheckTimer)
        {
            WatchOutForZombies();
            personCheckTimer = Random.Range(0.0f, timePersonBetweenChecks * 0.2f);
        }
        else
        {
            personCheckTimer += Time.deltaTime;
            if (!navMeshAgent.hasPath)
            {
                characterAnimator.SetBool("Walking", false);
            }
            else
            {
                characterAnimator.SetBool("Walking", true);
            }
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

        float xRand = Random.Range(-1f, 1f);
        float zRand = Random.Range(-1f, 1f);
        Vector3 randomDirection = Vector3.Normalize(new Vector3(xRand, 0f, zRand));

        // TODO: Se tiene que añadir una variable mas que sea "pensando" para meterle el idle.. porque hay veces que se para para hacer tiempo y sigue haciendo el moonwalk
        if (closestZombie != null && minDistance < GameController.peopleFearDistance)
        {
            Vector3 direction = Vector3.Normalize((transform.position - closestZombie.gameObject.transform.position) + randomDirection * 3.0f);
            navMeshAgent.SetDestination(transform.position + direction * 20.0f);
        }
        else
        {
            navMeshAgent.SetDestination(transform.position + randomDirection * 5.0f);
        }

    }


    public void Die()
    {
        StartCoroutine("Death");
    }

    protected override void DeathBehaviour()
    {
        if (Random.Range(0.0f, 100.0f) <= GameController.zombieProbability)
        {
            gameController.PersonConverted(this);
        }
    }


}
