using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform intro;
    public Transform win;
    public Transform lose;
    public Timer timer;
    public int level;
    public int totalSheep;
    int sheepCount;
    public Sheep sheep;
    public float range;
    public string next;

    void Start()
    {
        if (timer == null)
        {
            Timer.Reset();
        }
        if (intro != null)
        {
            StartCoroutine(ShowIntro());
        }
        else
        {
            Timer.Reset();
        }
        List<Vector2> positions = new List<Vector2>();
        for (int i = 0; i < totalSheep; i++)
        {
            Vector2 position;
            bool collision;
            // make sure we don't collide with existing sheep
            do
            {
                collision = false;
                position = Random.insideUnitCircle * range;
                foreach (Vector2 p in positions)
                {
                    if (Vector2.Distance(position, p) < 1.0f)
                    {
                        collision = true;
                        break;
                    }
                }
            }
            while (collision);
            positions.Add(position);
            Sheep sheep = Instantiate(this.sheep, new Vector3(position.x, 0.5f, position.y), Quaternion.identity);
            sheep.level = this;
        }
    }

    IEnumerator ShowIntro()
    {
        intro.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        intro.gameObject.SetActive(false);
        timer.gameObject.SetActive(true);
    }

    public void CountSheep()
    {
        sheepCount++;
        if (totalSheep == sheepCount)
        {
            StartCoroutine(YouWin());
        }
    }

    IEnumerator YouWin()
    {
        timer.enabled = false;
        win.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Load(next);
    }

    IEnumerator YouLose()
    {
        timer.enabled = false;
        lose.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Load("Start");
    }

    public void DoubleCount()
    {
        StartCoroutine(YouLose());
    }

    public void Load(string level)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(level);
    }
}
