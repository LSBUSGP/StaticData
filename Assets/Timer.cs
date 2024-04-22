using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    GameData gameData;
    public TMP_Text text;

    void Start()
    {
        gameData = FindObjectOfType<GameData>();
    }

    void Update()
    {
        gameData.UpdateTimeText(text);
    }
}
