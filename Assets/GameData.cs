using TMPro;
using UnityEngine;

[CreateAssetMenu]
public class GameData : ScriptableObject
{
    [SerializeField] float time;

    public void ResetTimer()
    {
        time = 0.0f;
    }

    public void UpdateTimeText(TMP_Text text)
    {
        time += Time.deltaTime;
        System.TimeSpan span = System.TimeSpan.FromSeconds(time);
        text.text = $"TIME: {span.Minutes:D2}:{span.Seconds:D2}.{span.Milliseconds / 10:D2}";
    }
}
