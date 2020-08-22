using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public bool upAnimation;
    public float upPosition = 8.0f;
    public float moveUpSpeed = 5.0f;
    private Canvas canvas;

    private void Start()
    {
        canvas = gameObject.GetComponent<Canvas>();
    }

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;

        if(upAnimation)
        {
            canvas.enabled = true;
            StartCoroutine("MoveUp");
        }

    }

    IEnumerator MoveUp()
    {
        Vector3 pos = gameObject.transform.position;
        for (int i = 0; i < 100; i++)
        {
            pos.y += 0.05f;
            gameObject.transform.position = pos;
            yield return new WaitForSeconds(0.01f);
        }

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, 5, gameObject.transform.position.z);
        upAnimation = false;
        canvas.enabled = false;
    }

}


