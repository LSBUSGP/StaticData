using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TMP_Text text;

    void Update()
    {
        GameData.instance.UpdateTimeText(text);
    }
}
