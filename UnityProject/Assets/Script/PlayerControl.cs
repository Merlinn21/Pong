using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public KeyCode moveUp;
    public KeyCode moveDown;
    public KeyCode powerUp;

    public float speed = 10f;
    public float yBoundary = 9f;
    public float powerUpAddition;
    public float powerDuration;
    public float coolDown = 2f;

    private int score;
    private bool isPowered = false;
    private Rigidbody2D rb;
    private Transform transformScale;
    private SpriteRenderer playerSprite;
    private Color currColor = Color.white;
    private Color powerColor = Color.red;
    private ContactPoint2D lastContactPoint;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        transformScale = this.gameObject.GetComponent<Transform>();
        playerSprite = this.gameObject.GetComponent<SpriteRenderer>();
        playerSprite.color = currColor;
    }

    void Update()
    {
        Move();
        PowerUp();
        CheckBoundary();
    }

    private void Move()
    {
        Vector2 velocity = rb.velocity;
        if (Input.GetKey(moveUp))
        {
            velocity.y = speed;
        }
        else if (Input.GetKey(moveDown))
        {
            velocity.y = -speed;
        }
        else
        {
            velocity.y = 0;
        }

        rb.velocity = velocity;
    }

    private void CheckBoundary()
    {
        Vector3 position = transform.position;

        if (position.y > yBoundary)
            position.y = yBoundary;
        else if (position.y < -yBoundary)
            position.y = -yBoundary;

        transform.position = position;
    }

    public void IncrementScore()
    {
        score++;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public int Score
    {
        get
        {
            return score;
        }
    }

    public ContactPoint2D LastContactPoint
    {
        get { return lastContactPoint; }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Ball"))
        {
            lastContactPoint = collision.GetContact(0);
        }
    }

    private void PowerUp()
    {
        if (Input.GetKeyDown(powerUp) && !isPowered)
        {
            transformScale.localScale += new Vector3(0, powerUpAddition, 0);
            isPowered = true;
            playerSprite.color = powerColor;
            StartCoroutine("PowerDuration");
        }
    }

    IEnumerator PowerDuration()
    {
        StartCoroutine("PowerCooldown");
        yield return new WaitForSeconds(powerDuration);
        transformScale.localScale -= new Vector3(0, powerUpAddition, 0);
    }

    IEnumerator PowerCooldown()
    {
        yield return new WaitForSeconds(powerDuration + coolDown);
        isPowered = false;
        playerSprite.color = currColor;
    }
}
