using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public Ball ball;
    LineRenderer line;
    public LineRenderer shotLine;
    public Vector2 mauspos;
    public Vector2 bolapos;
    public PlayerState player; //los players y pause, que es cuando un tiro está en juego en el momento
    public PlayerState lastPlayer; //de quién es el tiro que se acaba de realizar
    public bool firstPot; //si ya embocaron alguna bola
    public List<Ball> balls; //lista con las bolas, que se van sacando a medida que se pottean
    public bool[,] pB = new bool[2,2]; //player Balls, a quién le pertenecen qué bolas: fila 0 es el jugador, fila 2 es el tipo. una columna es el not de la otra.
    private bool potted; //si en ese turno embocaron.
    public Text turn;

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


    private void Update()
    {
        //si está en pleno turno chequea pa sacarlo del turno y que tire el otro
        if (player == PlayerState.pause)
        {
            bool endTurn = true;
            for (int i = 0; i < balls.Count; i++)
            {
                if (balls[i].rigbod.velocity.magnitude != 0)
                {
                    endTurn = false;
                }
            }
            if (endTurn)
            {
                Debug.Log("Turn ended");
                //cuando ya todas están frenadas, afterShot es pa saber qué pasó ese turno
                afterShot();
            }
        }
    }

    public void afterShot()
    {

        if (potted)
        {
            if (lastPlayer == PlayerState.player1)
            {
                player = PlayerState.player1;
                lastPlayer = PlayerState.player1;
                turn.text = "Player 1's Turn";
            }
            else
            {
                
                player = PlayerState.player2;
                lastPlayer = PlayerState.player2;
                turn.text = "Player 2's Turn";
            }
        }
        else
        {
            if (lastPlayer == PlayerState.player1)
            {
                player = PlayerState.player2;
                lastPlayer = PlayerState.player2;
                turn.text = "Player 2's Turn";
            }
            else
            {
                player = PlayerState.player1;
                lastPlayer = PlayerState.player1;
                turn.text = "Player 1's Turn";
            }
        }
    }

    void OnMouseDrag()
    {


        //human made horrors beyond guaji comprehension
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
        int[] solids = new int[] { 1, 2, 3, 4, 5, 6, 7 };
        int[] striped = new int[] { 9, 10, 11, 12, 13, 14, 15 };

        //se define quién emboca qué
        if (!firstPot)
        {   
            firstPot = true;
            if (lastPlayer==PlayerState.player1)
            {
                if (Array.Exists(solids,x => x==ball.number))
                {
                    pB[0, 0] = true;
                    pB[1, 1] = true;
                }
                else if(Array.Exists(striped, x => x == ball.number))
                {
                    pB[0, 1] = true;
                    pB[1, 0] = true;
                }
            }
            else
            {
                if (Array.Exists(solids, x => x == ball.number))
                {
                    pB[0, 1] = true;
                    pB[1, 0] = true;
                }
                else if (Array.Exists(striped, x => x == ball.number))
                {
                    pB[0, 0] = true;
                    pB[1, 1] = true;
                }
            }



        }

        //se saca de la lista antes de destroy pq si no ñaca
        balls.Remove(ball);
        Destroy(ball);
    }
    private void OnMouseUp()
    {

        //pause es pa no spammear el palo
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
            turn.text = "Wait...";
        }
        
    }


    public enum PlayerState
    {
        player1,
        player2,
        pause
    }

}
