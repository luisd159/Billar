using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public Ball cueball;
    LineRenderer line;
    public LineRenderer shotLine;
    public Vector2 mauspos;
    public Vector2 bolapos;
    public PlayerState player; //los players y pause, que es cuando un tiro está en juego en el momento
    public PlayerState lastPlayer; //de quién es el tiro que se acaba de realizar
    public bool firstPot; //si ya embocaron alguna bola
    public List<Ball> balls; //lista con las bolas, que se van sacando a medida que se pottean
    public bool[,] pB = new bool[2, 2]; //player Balls, a quién le pertenecen qué bolas: fila 0 es el jugador, fila 2 es el tipo. una columna es el not de la otra.
    public Vector2[] scorePos = new Vector2[16];
    private bool potted; //si en ese turno embocaron.
    public Text turn;
    public int[] score = new int[] { 0, 0 };
    public bool pottedWrong;
    public Text[] playText = new Text[2];
    public bool canPlace = false;
    public Collider2D tablecol;

    // Start is called before the first frame update
    void Start()
    {
        scorePos = new Vector2[] {new Vector2(-10,0), new Vector2(-22.78f,7), new Vector2(-22.78f,5), new Vector2(-22.78f,3), new Vector2(-22.78f,1), new Vector2(-22.78f,-1), new Vector2(-22.78f,-3),
         new Vector2(-22.78f,-5), new Vector2(0,-12), new Vector2(22.78f,7), new Vector2(22.78f,5), new Vector2(22.78f,3), new Vector2(22.78f,1), new Vector2(22.78f,-1), new Vector2(22.78f,-3),  new Vector2(22.78f,-5)};


        firstPot = false;
        player = PlayerState.player1;
        lastPlayer = PlayerState.player1;
        instance = this;
        line = this.GetComponent<LineRenderer>();
        line.startWidth = 0f;
        line.endWidth = 0f;
        turn.text = "Player 1's Turn";
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
                //cuando ya todas están frenadas, afterShot es pa saber qué pasó ese turno
                afterShot();
            }
        }
    }

    private void OnMouseDown()
    {
        if (player == PlayerState.dragball)
        {
            canPlace = true;
            mauspos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 m1 = mauspos + new Vector2(1, 1);
            Vector2 m2 = mauspos + new Vector2(-1, 1);
            Vector2 m3 = mauspos + new Vector2(-1, -1);
            Vector2 m4 = mauspos + new Vector2(1, -1);
            for (int i = 1; i < 16; i++)
            {

                if (balls[i].col.bounds.Contains(m1) || balls[i].col.bounds.Contains(m2) || balls[i].col.bounds.Contains(m3) || balls[i].col.bounds.Contains(m4) || !tablecol.bounds.Contains(mauspos))
                {
                    canPlace = false;
                }
            }
            Debug.Log("can place? "+ tablecol.bounds.extents);

            if (canPlace)
            {
                cueball.col.enabled = true;
                player = lastPlayer;
            }
        }
        
    }



    void OnMouseDrag()
    {
        if (player != PlayerState.pause && player !=PlayerState.dragball)
        {
            mauspos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bolapos = cueball.GetComponent<Transform>().position;
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

    private void OnMouseUp()
    {
        if (player != PlayerState.pause && player!=PlayerState.dragball)
        {
            player = PlayerState.pause;
            mauspos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bolapos = cueball.GetComponent<Transform>().position;
            Vector2 launchv = bolapos - mauspos;
            cueball.rigbod.AddForce(launchv * 1.5f, ForceMode2D.Impulse);
            line.startWidth = 0f;
            line.endWidth = 0f;
            shotLine.startWidth = 0;
            shotLine.endWidth = 0;
            turn.text = "Wait...";
        }

    }
    public void afterShot()
    {

        if (pottedWrong)
        {
            player = PlayerState.dragball;
            cueball.col.enabled = false;
            if (lastPlayer == PlayerState.player1)
            {
                turn.text = "Player 2 Drop Ball";
            }
            else
            {
                turn.text = "Player 1 Drop Ball";
            }
        }
        else if(!potted)
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
        else
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
        potted = false;
        pottedWrong = false;
    }


    public void pot(Ball ball)
    {
        potted = true;
        int type=0;
        switch (ball.number) //se define el tipo de bola según su número
        {
            case 0:
                type = 0;
                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
                type = 1;
                break;
            case 8:
                type = 3;
                break;
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
            case 14:
            case 15:
                type = 2;
                break;
            default:
                break;
        }
        
        //se define quién emboca qué, según el type
        if (!firstPot && type> 0)
        {
            firstPot = true;
            switch (type)
            {
                case 0:
                    break;
                case 1:
                    if (lastPlayer==PlayerState.player1)
                    {
                        pB[0, 0] = true;
                        pB[1, 1] = true;
                        playText[0].text = "Player 1";
                        playText[1].text = "Player 2";
                    }
                    else
                    {
                        pB[0, 1] = true;
                        pB[1, 0] = true;
                        playText[0].text = "Player 2";
                        playText[1].text = "Player 1";
                    }
                    break;
                case 2:
                    if (lastPlayer == PlayerState.player1)
                    {
                        pB[0, 1] = true;
                        pB[1, 0] = true;
                        playText[0].text = "Player 2";
                        playText[1].text = "Player 1";
                    }
                    else
                    {
                        pB[0, 0] = true;
                        pB[1, 1] = true;
                        playText[0].text = "Player 1";
                        playText[1].text = "Player 2";
                    }
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }

        switch (type)//se mira qué se hace con la bola
        {
            case 0:
                ball.GetComponent<Transform>().position = scorePos[ball.number];
                ball.rigbod.velocity = new Vector2(0, 0);
                pottedWrong = true;
                break;
            case 1:
                if (lastPlayer==PlayerState.player1)
                {
                    if (pB[0,0])
                    {
                        score[0]++;
                    }
                    else
                    {
                        pottedWrong = true;
                        score[1]++;
                    }
                    

                }
                else
                {
                    if (pB[1,0])
                    {
                        score[1]++;
                    }
                    else
                    {
                        pottedWrong = true;
                        score[0]++;
                    }
                }
                break;
            case 2:
                if (lastPlayer == PlayerState.player1)
                {
                    if (pB[0, 1])
                    {
                        score[0]++;
                    }
                    else
                    {
                        pottedWrong = true;
                        score[1]++;
                    }


                }
                else
                {
                    if (pB[1, 1])
                    {
                        score[1]++;
                    }
                    else
                    {
                        pottedWrong = true;
                        score[0]++;
                    }
                }
                break;
            case 3://win condition
                /*1: jugador 1 gana bien
                 *2: jugador 2 gana bien 
                 *3: jugador 1 gana pq el 2 es manco
                 *4: jugador 2 gana pq el 1 es manco
                 */
                if (lastPlayer==PlayerState.player1)
                {
                    if (score[0]<7)
                    {
                        PlayerPrefs.SetInt("Result", 4);
                    }
                    else
                    {
                        PlayerPrefs.SetInt("Result", 1);
                    }
                }
                else
                {
                    if (score[1] < 7)
                    {
                        PlayerPrefs.SetInt("Result", 3);
                    }
                    else
                    {
                        PlayerPrefs.SetInt("Result", 2);
                    }
                }
                SceneManager.LoadScene("End");
                break;


            default:
                break;
        }


        if (ball.number>0)
        {
            ball.rigbod.constraints = RigidbodyConstraints2D.FreezePosition;
            ball.GetComponent<Transform>().position = scorePos[ball.number];
            ball.col.enabled = false;
        }
        
    }
    


    public enum PlayerState
    {
        player1,
        player2,
        pause,
        dragball
    }

}
