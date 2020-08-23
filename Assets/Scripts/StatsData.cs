using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsData", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class StatsData : ScriptableObject
{
    [Serializable]
    public class sliderData
    {
        [Serializable]
        public class PairPriceValue
        {
            public float value;
            public float price;
        }

        public Stat statType;
        public PairPriceValue[] pairs;
    }

    public sliderData[] sliders;
}
