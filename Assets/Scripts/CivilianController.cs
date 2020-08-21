using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CivilianController : MonoBehaviour
{
    public int health = 100;
    public bool zombie;

    private bool deathIdle; // This will avoid multiple calls to the coroutine

    // Start is called before the first frame update
    void Start()
    {
        GameController.peopleAlive = GameController.peopleAlive + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(!deathIdle && health <= 0 )
        {
            deathIdle = true;
            StartCoroutine("Death");
        }
    }

    IEnumerator Death()
    {
        //TODO: trigger death animation
        yield return new WaitForSeconds(GameController.transformTime);

        if (zombie)
        {
            // TODO: Disable collider and scripts.
        } 
        else
        {
            // If the guy is not a zombie, take the chance to transform it
            zombie = Random.Range(0.0f, 100.0f) <= GameController.zombieProbability;
            if (zombie)
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

