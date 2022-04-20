using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Bruh");


        if (collision.gameObject.tag == "ball")
        {
            Ball b = collision.gameObject.GetComponent<Ball>();
            GameManager.instance.pot(b);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
