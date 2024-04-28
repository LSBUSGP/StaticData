using System;
using TMPro;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;
    [SerializeField] float time = 0.0f;

    void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateTimeText(TMP_Text text)
    {
        time += Time.deltaTime;
        TimeSpan span = TimeSpan.FromSeconds(time);
        text.text = $"TIME: {span.Minutes:D2}:{span.Seconds:D2}.{span.Milliseconds/10:D2}";
    }

    public void ResetTimer()
    {
        time = 0.0f;
    }
}
