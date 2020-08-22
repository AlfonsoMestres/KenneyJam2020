using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsController : MonoBehaviour
{
    public List<int> prices;

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
    }

    public void IncreaseStat()
    {
        sliderStat.value = sliderStat.value + 1;
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
        } 
        else
        {
            buyButton.enabled = true;
        }
    }
}
