using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ball ball;
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
    }
    void OnMouseDrag()
    {
        mauspos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bolapos = ball.GetComponent<Transform>().position;
        Vector2 distance =mauspos - bolapos;
        float Angle = Vector2.SignedAngle(Vector2.right, distance) * Mathf.PI/ 180;
        float sx = Mathf.Cos(Angle) * 1f;
        float sy = Mathf.Sin(Angle) * 1f;
        Vector2 stick = new Vector2(sx, sy);

        Debug.Log((stick*10-stick).magnitude);
        line.SetPosition(0, bolapos + (stick*10) + (distance * 0.2f));
        line.SetPosition(1, bolapos + stick + (distance * 0.2f));
        line.startWidth=0.2f;
        line.endWidth=0.1f;
        Color stickColor = Color.Lerp(new Color(0,0.2f,1), Color.red, distance.magnitude / 34);
        line.endColor = stickColor;
    }
    private void OnMouseUp()
    {
        mauspos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bolapos = ball.GetComponent<Transform>().position;
        Vector2 launchv = bolapos - mauspos;
        ball.GetComponent<Rigidbody2D>().AddForce(launchv*1.5f, ForceMode2D.Impulse);
        line.startWidth = 0f;
        line.endWidth = 0f;

    }


}
