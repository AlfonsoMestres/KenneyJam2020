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
            personCheckTimer = Random.Range(0.0f, timePersonBetweenChecks * 0.2f);
            WatchOutForZombies();
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

        if (closestZombie != null && minDistance < GameController.peopleFearDistance)
        {
            Vector3 direction = Vector3.Normalize((transform.position - closestZombie.gameObject.transform.position) + randomDirection * 3.0f);
            navMeshAgent.SetDestination(transform.position + direction * 20.0f);
        }
        else
        {
            navMeshAgent.SetDestination(transform.position + randomDirection * 2.0f);
            personCheckTimer = -1.0f;
        }

    }

    public void Die()
    {
        lookCamera.upAnimation = true;
        StartCoroutine("Death");
    }

    protected override void DeathBehaviour()
    {
        if (Random.Range(0.0f, 100.0f) <= GameController.zombieProbability)
        {
            isZombie = true;
            gameController.PersonConverted(this);
        }
    }


}
