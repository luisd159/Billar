using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CueBall ball;
    LineRenderer line;
    public Vector2 mauspos;
    public Vector2 bolapos;
    // Start is called before the first frame update
    void Start()
    {
        line = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame

    

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ball")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

    }



    private void OnMouseDown()
    {
        line.startWidth=0.1f;
        line.endWidth = 0.1f;
    }
    void OnMouseDrag()
    {
        mauspos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bolapos = ball.GetComponent<Transform>().position;
        Vector2 stickv = 0.1f*(bolapos - mauspos);
        line.SetPosition(0, mauspos);
        line.SetPosition(1, bolapos-stickv);
    }
    private void OnMouseUp()
    {
        mauspos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bolapos = ball.GetComponent<Transform>().position;
        Vector2 launchv = bolapos - mauspos;
        ball.GetComponent<Rigidbody2D>().AddForce(launchv, ForceMode2D.Impulse);
        Debug.Log("bruh");
        line.startWidth = 0;
        line.endWidth = 0;
    }


}
