using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;

public class Sheep : MonoBehaviour, IPointerClickHandler
{
    public Gaussian graze;
    public Gaussian search;
    public float speed = 1f;
    public Rigidbody body;
    bool move = false;
    Vector3 target;
    bool counted = false;
    public Level level;

    void Reset()
    {
        body = GetComponent<Rigidbody>();
    }

    void Start()
    {
        StartCoroutine(Graze());
    }

    IEnumerator Graze()
    {
        yield return new WaitForSeconds(Random.Range(0.0f, graze.mean));
        while (true)
        {
            Vector2 offset = Random.insideUnitSphere * search.GetRandomNumber();
            target = body.position + new Vector3(offset.x, 0.0f, offset.y);
            move = true;
            yield return new WaitWhile(() => move && Vector3.Distance(body.position, target) > 0.01f);
            move = false;
            yield return new WaitForSeconds(graze.GetRandomNumber());
        }
    }

    void FixedUpdate()
    {
        if (move)
        {
            float d = Vector3.Distance(body.position, target);
            float u = body.velocity.magnitude;
            float v = Mathf.Min(d, speed);
            float a = v - u;
            body.AddForce((target - body.position).normalized * body.mass * a);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        move = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (counted)
        {
            level.DoubleCount();
        }
        else
        {
            counted = true;
            StartCoroutine(Pulse());
            level.CountSheep();
        }
    }

    IEnumerator Pulse()
    {
        Vector3 scale = transform.localScale;
        transform.localScale = scale * 1.5f;
        yield return new WaitForSeconds(0.1f);
        transform.localScale = scale;
    }
}
