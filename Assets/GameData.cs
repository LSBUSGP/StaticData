using System;
using TMPro;
using UnityEngine;

public class GameData : MonoBehaviour
{
    static GameData instance;
    float time = 0.0f;

    void Start()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public void UpdateTimeText(TMP_Text text)
    {
        time += Time.deltaTime;
        TimeSpan span = TimeSpan.FromSeconds(time);
        text.text = $"TIME: {span.Minutes:D2}:{span.Seconds:D2}.{span.Milliseconds/10:D2}";
    }
}
