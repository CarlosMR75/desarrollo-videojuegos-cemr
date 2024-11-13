using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorMovimiento : MonoBehaviour
{
    public float moveSpeed = 5f; 
    private Rigidbody2D rb; 
    private Vector2 movement;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // movement.x = Input.GetAxisRaw("Horizontal");
        // movement.y = Input.GetAxisRaw("Vertical");

        movement = Vector2.zero;

        if (Input.GetKey(KeyCode.Y))
        {
            movement.y = 1;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            movement.y = -1;
        }
        
        if (Input.GetKey(KeyCode.L))
        {
            movement.x = 1;
        }
        else if (Input.GetKey(KeyCode.J))
        {
            movement.x = -1;
        }

        animator.SetFloat("MovimientoX", movement.x);
        animator.SetFloat("MovimientoY", movement.y);
        animator.SetBool("isWalking", movement.magnitude > 0);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
