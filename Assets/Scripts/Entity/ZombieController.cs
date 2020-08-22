using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : EntityController
{
    private float timeZombieBetweenChecks = 0.5f;
    private float zombieCheckTimer;

    protected override void OnGameCreated()
    {
        base.OnGameCreated();
        gameController.AddZombie(this);
        navMeshAgent.speed = GameController.zombieSpeed;
    }

    protected override void Update()
    {
        base.Update();
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
                characterAnimator.SetBool("Attacking", true);
                zombieCheckTimer = -2.0f; //this way they remain in place
                targetController.TakeDamage(GameController.zombieAttackDamage);
                navMeshAgent.ResetPath();
            } 
            else
            {
                characterAnimator.SetBool("Walking", true);
                characterAnimator.SetBool("Attacking", false);
            }
        } 
        else
        {
            characterAnimator.SetBool("Walking", false);
            characterAnimator.SetBool("Attacking", false);
        }
    }

    protected override void DeathBehaviour()
    {
        //this has to happen every 1 sec or so better than in any frame
    }
}