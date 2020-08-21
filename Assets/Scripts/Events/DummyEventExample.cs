using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    GameController controller = null;
    // Start is called before the first frame update
    void Start()
    {
        controller.OnGameStarted.Subscribe(OnGameStart);
        controller.OnGameStarted.Unsubscribe(OnGameStart);
    }

    private void OnGameStart()
    {

    }
}
