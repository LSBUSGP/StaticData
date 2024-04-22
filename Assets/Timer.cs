using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public GameData gameData;
    public TMP_Text text;

    void Update()
    {
        gameData.UpdateTimeText(text);
    }
}
