using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsController : MonoBehaviour
{

    private Slider sliderStat;
    private int statPrice;

    // Start is called before the first frame update
    void Start()
    {
        sliderStat = gameObject.GetComponentInChildren<Slider>();
        //statPrice = int.Parse(gameObject.transform.Find("Amount").GetComponent<Text>().text);
    }

    public void IncreaseStat()
    {
        sliderStat.value = sliderStat.value + 1;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Disable the button if the amount is not enough
        if (GameController.cursedHeartsObtained >= statPrice)
        {

        }
    }
}
