using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Stat
{
    health,
    speed,
    attack,
    cursePower,
    initialTouches
}

public class StatsController : MonoBehaviour
{
    public List<int> prices;
    public Text cursedHeartsShop;
    public Stat statType;
    public GameController gameController;
    public StatsData statsData;

    private Text maxedText;
    private Button buyButton;
    private Slider sliderStat;
    private Text buyAmount;
    private int statPrice;
    private float maxSliderValue;
    private float[] levelUpData;
    public int currentIndex;



    public void SetSlider()
    {
        foreach (var slider in statsData.sliders)
        {
            if (slider.statType == statType)
            {
                sliderStat.maxValue = slider.values.Length;
                break;
            }
        }
        sliderStat.value = currentIndex;
        statPrice = prices[currentIndex];
    }

    // Start is called before the first frame update
    void Awake()
    {
        buyButton = gameObject.GetComponentInChildren<Button>();
        sliderStat = gameObject.GetComponentInChildren<Slider>();
        maxedText = gameObject.transform.Find("MaxedStat").GetComponent<Text>();
        sliderStat.maxValue = prices.Count;
        buyAmount = gameObject.transform.Find("Amount").GetComponent<Text>();
        buyAmount.text = prices[0].ToString();
        cursedHeartsShop = gameObject.transform.parent.Find("CurrencyAmount").GetComponent<Text>();

        SetSlider();
    }

    public void IncreaseStat()
    {
        ++currentIndex;
        sliderStat.value = currentIndex;
        GameController.cursedHeartsObtained = GameController.cursedHeartsObtained - statPrice;

        if (sliderStat.value == sliderStat.maxValue)
        {
            buyAmount.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
            maxedText.enabled = true;
            return;
        }
        statPrice = prices[currentIndex];
        buyAmount.text = statPrice.ToString();
        gameController.SavePrefs();
    }

    public float GetCurrentStatValue()
    {
        foreach(var slider in statsData.sliders)
        {
            if(slider.statType == statType)
            {
                return slider.values[currentIndex];
            }
        }

        return 0f;
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
