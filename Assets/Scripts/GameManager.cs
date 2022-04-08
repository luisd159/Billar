using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CueBall ball;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ball")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

    }
        private void OnMouseDown()
    {
        Vector2 mauspos =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 bolapos = ball.GetComponent<Transform>().position;

        Vector2 diffpos =-1 * (mauspos - bolapos);
        ball.GetComponent<Rigidbody2D>().AddForce(diffpos, ForceMode2D.Impulse);
        Debug.Log(100*diffpos +" "+mauspos+" "+bolapos);
    }
}
