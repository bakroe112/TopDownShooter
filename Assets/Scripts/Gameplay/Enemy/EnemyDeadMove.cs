//using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadMove : MonoBehaviour
{
    public float deadSpeed;
    public float minSpeed;
    public float slowDown;
    private Rigidbody2D rb;
    void OnEnable()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        //Destroy(GetComponent<AILerp>());
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        float angle = Random.Range(0, 2 * Mathf.PI);
        rb.linearVelocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * deadSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity *= slowDown;
        if (rb.linearVelocity.magnitude < minSpeed)
        {
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(this);
        }
    }
}
