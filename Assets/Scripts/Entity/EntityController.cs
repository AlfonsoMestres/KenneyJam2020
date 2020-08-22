using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class EntityController : MonoBehaviour
{
    public Color entityHPColor;
    public Slider healthBar;
    public int health = 100;

    // TODO: Update health bar when health value is affected

    protected bool deathIdle; // This will avoid multiple calls to the coroutine
    protected NavMeshAgent navMeshAgent;
    protected Animator characterAnimator;

    protected GameController gameController = null;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController")?.GetComponent<GameController>();
        gameController.OnGameCreated.Subscribe(OnGameCreated);

        characterAnimator = GameObject.FindObjectOfType<Animator>();

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected virtual void OnGameCreated()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        healthBar.value = health;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        healthBar.value = health;
    }

    // How is this called? I cannot trigger the death animation if this is not called
    IEnumerator Death()
    {
        characterAnimator.SetBool("Dead", true);

        yield return new WaitForSeconds(GameController.transformTime);

        DeathBehaviour();
        deathIdle = false;
    }

    protected abstract void DeathBehaviour();
}