﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsController : MonoBehaviour
{
    public List<int> prices;
    public Text cursedHeartsShop;

    private Text maxedText;
    private Button buyButton;
    private Slider sliderStat;
    private Text buyAmount;
    private int statPrice;

    // Start is called before the first frame update
    void Start()
    {
        buyButton = gameObject.GetComponentInChildren<Button>();
        sliderStat = gameObject.GetComponentInChildren<Slider>();
        maxedText = gameObject.transform.Find("MaxedStat").GetComponent<Text>();
        sliderStat.maxValue = prices.Count;
        buyAmount = gameObject.transform.Find("Amount").GetComponent<Text>();
        buyAmount.text = prices[0].ToString();
        cursedHeartsShop = gameObject.transform.parent.Find("CurrencyAmount").GetComponent<Text>();
        statPrice = prices[(int)sliderStat.value];
    }

    public void IncreaseStat()
    {
        sliderStat.value = sliderStat.value + 1;
        GameController.cursedHeartsObtained = GameController.cursedHeartsObtained - statPrice;

        if (sliderStat.value == sliderStat.maxValue)
        {
            buyAmount.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
            maxedText.enabled = true;
            return;
        }
        statPrice = prices[(int)sliderStat.value];
        buyAmount.text = statPrice.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.cursedHeartsObtained < statPrice)
        {
            buyButton.enabled = false;
            buyButton.image.color = Color.grey;
        } 
        else
        {
            buyButton.enabled = true;
            buyButton.image.color = Color.white;
        }
    }
}
