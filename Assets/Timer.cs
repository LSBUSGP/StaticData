using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float time = 0.0f;
    public TMP_Text text;

    void Update()
    {
        time += Time.deltaTime;
        TimeSpan span = TimeSpan.FromSeconds(time);
        text.text = $"TIME: {span.Minutes:D2}:{span.Seconds:D2}.{span.Milliseconds/10:D2}";
    }
}
