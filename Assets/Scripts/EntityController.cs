﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public class EntityController : MonoBehaviour
{
    public int health = 100;
    public bool isZombie;

    private bool deathIdle; // This will avoid multiple calls to the coroutine
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isZombie)
        {
            GameController.zombies.Add(this);
        }
        else
        {
            GameController.people.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = null;
        float minDistance = 10000000; //needs to be BIG the first time
        foreach (var person in GameController.people)
        {
            var distance = Vector3.Distance(person.gameObject.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                target = person.gameObject;
            }
        }
        if (target != null)
        {
            navMeshAgent.SetDestination(target.transform.position);
        }

        if (!deathIdle && health <= 0 )
        {
            deathIdle = true;
            StartCoroutine("Death");
        }
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
                GameController.zombiesAlive = GameController.zombiesAlive + 1;
                health = GameController.zombieHealth;
                //TODO:  trigger effect 
                //TODO:  change skin change skin
            }

        }

        deathIdle = false;

    }

}

