using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [HideInInspector]public Rigidbody2D rb;

    public float xForce;
    public float yForce;
    public float speed;

    private Vector2 trajectoryOrigin;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        RestartGame();
        trajectoryOrigin = transform.position;
    }

    void ResetBall()
    {
        this.transform.position = Vector2.zero;
        rb.velocity = Vector2.zero;
    }

    void PushBall()
    {
        float randomYForce = Random.Range(-yForce, yForce);
        float randomDirection = Random.Range(0, 2);

        if (randomDirection < 1f)
        {
            rb.AddForce(new Vector2(-xForce, randomYForce).normalized * speed);
        }
        else
        {
            rb.AddForce(new Vector2(xForce, randomYForce).normalized * speed);
        }

    }

    void RestartGame()
    {
        ResetBall();
        Invoke("PushBall", 2);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        trajectoryOrigin = transform.position;
    }

    public Vector2 TrajectoryOrigin
    {
        get { return trajectoryOrigin; }
    }
}
