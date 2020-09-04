using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Manager")]
    public int maxScore = 5;

    [Space]
    [Header("References")]
    public PlayerControl p1;
    public PlayerControl p2;
    public Ball ball;
    public Trajectory trajectory;

    [Space]
    [Header("Events")]
    public UnityEvent onP1Win;
    public UnityEvent onP2Win;
    public UnityEvent onDebug;
    public UnityEvent onDebugClose;
    public UnityEvent onRestart;

    [Space]
    [Header("UI")]
    public Text scoreP1;
    public Text scoreP2;
    public Text debugText;
    public Text power1;
    public Text power2;

    float ballMass;
    Vector2 ballVelocity;
    float ballSpeed;
    Vector2 ballMomentum;
    float ballFriction;

    float impulsePlayer1X;
    float impulsePlayer1Y;
    float impulsePlayer2X;
    float impulsePlayer2Y;

    private bool isDebugWindowShown = false;

    private void Update()
    {
        CheckScore();
        UpdateUI();
        Debug();
    }

    private void Start()
    {
        power1.text = "Press " + p1.powerUp.ToString() + " To Power-up";
        power2.text = "Press " + p2.powerUp.ToString() + " To Power-up";
    }

    private void CheckScore()
    {
        if(p1.Score == maxScore)
        {
            onP1Win.Invoke();
        }

        if(p2.Score == maxScore)
        {
            onP2Win.Invoke();
        }
    }

    public void RestartGame()
    {
        p1.ResetScore();
        p2.ResetScore();
        onRestart.Invoke();
        ball.SendMessage("RestartGame", 0.5f, SendMessageOptions.RequireReceiver);
    }

    public void UpdateUI()
    {
        scoreP1.text = p1.Score.ToString();
        scoreP2.text = p2.Score.ToString();
    }

    private void Debug()
    {
        ballMass = ball.rb.mass;
        ballVelocity = ball.rb.velocity;
        ballSpeed = ball.rb.velocity.magnitude;
        ballMomentum = ballMass * ballVelocity;
        ballFriction = ball.GetComponent<CircleCollider2D>().friction;

        impulsePlayer1X = p1.LastContactPoint.normalImpulse;
        impulsePlayer1Y = p1.LastContactPoint.tangentImpulse;
        impulsePlayer2X = p2.LastContactPoint.normalImpulse;
        impulsePlayer2Y = p2.LastContactPoint.tangentImpulse;

        debugText.text = "Ball mass = " + ballMass + "\n" +
                "Ball velocity = " + ballVelocity + "\n" +
                "Ball speed = " + ballSpeed + "\n" +
                "Ball momentum = " + ballMomentum + "\n" +
                "Ball friction = " + ballFriction + "\n" +
                "Last impulse from player 1 = (" + impulsePlayer1X + ", " + impulsePlayer1Y + ")\n" +
                "Last impulse from player 2 = (" + impulsePlayer2X + ", " + impulsePlayer2Y + ")\n";
    }

    public void DebugPanel()
    {
        if (!isDebugWindowShown)
        {
            onDebug.Invoke();
            trajectory.enabled = true;
            trajectory.ballAtCollision.SetActive(true);
            isDebugWindowShown = true;
        }
        else
        {
            onDebugClose.Invoke();
            trajectory.enabled = false;
            trajectory.ballAtCollision.SetActive(false);
            isDebugWindowShown = false;
        }
        
    }
}
