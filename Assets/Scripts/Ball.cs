using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rigbod;
    public Collider2D col;
    public int number;
    public bool potted;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        rigbod = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
