﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static float transformTime = 3.5f; // Time a person wait to be transform or killed

    public static int peopleAlive = 0;
    public static int zombiesAlive = 0;
    public static int zombieProbability = 10;
    public static int zombieHealth = 40;
    public static float zombieSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (peopleAlive == 0)
        {
            //TODO: Win condition
            //TODO: trigger a couple of seconds to see everyone dead or transformed
            //TODO: trigger UI WIN and Rewards
        }

        if(zombiesAlive == 0)
        {
            //TODO: Lose condition
            //TODO: trigger a couple of seconds to see everyone alive
            //TODO: trigger UI LOSE and Rewards
        }
    }


}
