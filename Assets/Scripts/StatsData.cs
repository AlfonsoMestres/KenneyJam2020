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
        public Stat statType;
        public float[] values;
    }

    public sliderData[] sliders;
}
