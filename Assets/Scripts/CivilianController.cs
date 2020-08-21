using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianController : MonoBehaviour
{
    public int health = 100;
    public bool zombie;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!zombie && health <= 100)
        {
            StartCoroutine("Death");
        }
    }

    IEnumerator Death()
    {
        //TODO: trigger death animation
        yield return new WaitForSeconds(4f);

        zombie = Random.Range(0.0f, 100.0f) <= GameController.zombieProbability;
        if (zombie)
        {
            health = GameController.zombieHealth;
            //TODO:  trigger effect 
            //TODO:  change skin change skin
        }


    }

}

