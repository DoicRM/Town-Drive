using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Driver : MonoBehaviour
{
    static public float steerSpeed = 250f;
    static public float moveSpeed = 15f;
    static public float slowSpeed = 15f;
    static public float boostSpeed = 25f;

    static public bool hasPackage;
    static public int scoreValue = 0;
    static public int carHP = 100;

    void Update()
    {
        float steerAmount = Input.GetAxis("Horizontal") * steerSpeed * Time.deltaTime;
        float moveAmount = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Rotate(0, 0, -steerAmount);
        transform.Translate(0, moveAmount, 0);
    }
    
    void OnCollisionEnter2D(Collision2D other) 
    {
        moveSpeed = slowSpeed;    
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Boost")
        {
            moveSpeed = boostSpeed;
        }    
    }
}
