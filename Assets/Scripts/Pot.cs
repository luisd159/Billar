using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("aaaaa");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Bruh");
        collision.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

        if (collision.gameObject.tag == "ball")
        {
            Destroy(collision.gameObject);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
