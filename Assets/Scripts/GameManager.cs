using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public Ball ball;
    LineRenderer line;
    public LineRenderer shotLine;
    public Vector2 mauspos;
    public Vector2 bolapos;
    public PlayerState player;
    public PlayerState lastPlayer;
    public bool firstPot;
    public bool[,] pB = new bool[2,2];
    private bool potted;
    // Start is called before the first frame update
    void Start()
    {
        firstPot = false;
        player = PlayerState.player1;
        lastPlayer = PlayerState.player1;
        instance = this;
        line = this.GetComponent<LineRenderer>();
        line.startWidth = 0f;
        line.endWidth = 0f;
    }

    // Update is called once per frame




    public void afterShot()
    {
        if (potted)
        {

        }
    }

    void OnMouseDrag()
    {
        if (player != PlayerState.pause)
        {
            mauspos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bolapos = ball.GetComponent<Transform>().position;
            Vector2 distance = mauspos - bolapos;
            float Angle = Vector2.SignedAngle(Vector2.right, distance) * Mathf.PI / 180;
            float sx = Mathf.Cos(Angle) * 1f;
            float sy = Mathf.Sin(Angle) * 1f;
            Vector2 stick = new Vector2(sx, sy);
            line.SetPosition(0, bolapos + (stick * 10) + (distance * 0.2f));
            line.SetPosition(1, bolapos + stick + (distance * 0.2f));
            line.startWidth = 0.2f;
            line.endWidth = 0.1f;
            shotLine.startWidth = 0.075f;
            shotLine.endWidth = 0.075f;
            shotLine.SetPosition(0, bolapos);
            shotLine.SetPosition(1, bolapos - distance);
            Color stickColor = Color.black;
            if (player==PlayerState.player1)
            {
                stickColor = Color.Lerp(Color.black, Color.blue, distance.magnitude / 34);
            }
            else
            {
                stickColor = Color.Lerp(Color.black, Color.red, distance.magnitude / 34);
            }
            line.endColor = stickColor;
        }
    }


    public void pot(Ball ball)
    {
        potted = true;
        int[] lisas = new int[] { 1, 2, 3, 4, 5, 6, 7 };
        int[] rayadas = new int[] { 9, 10, 11, 12, 13, 14, 15 };
        if (!firstPot)
        {   
            firstPot = true;
            if (lastPlayer==PlayerState.player1)
            {
                if (pB[0,0] && Array.Exists(lisas,x => x==ball.number))
                {

                }
                else
                {

                }
            }
            else
            {

            }

            

        }
        Destroy(ball);
    }
    private void OnMouseUp()
    {
        if (player != PlayerState.pause)
        {
            player = PlayerState.pause;
            mauspos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bolapos = ball.GetComponent<Transform>().position;
            Vector2 launchv = bolapos - mauspos;
            ball.GetComponent<Rigidbody2D>().AddForce(launchv * 1.5f, ForceMode2D.Impulse);
            line.startWidth = 0f;
            line.endWidth = 0f;
            shotLine.startWidth = 0;
            shotLine.endWidth = 0;
        }
        
    }


    public enum PlayerState
    {
        player1,
        player2,
        pause
    }

}
