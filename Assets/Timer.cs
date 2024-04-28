using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    static float time = 0.0f;
    public TMP_Text text;

    void Update()
    {
        time += Time.deltaTime;
        TimeSpan span = TimeSpan.FromSeconds(time);
        text.text = $"TIME: {span.Minutes:D2}:{span.Seconds:D2}.{span.Milliseconds/10:D2}";
    }

    public static void Reset()
    {
        time = 0.0f;
    }
}
