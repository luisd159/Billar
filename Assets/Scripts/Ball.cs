using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rigbod;
    public int number;
    // Start is called before the first frame update
    void Start()
    {
        rigbod = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
