using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    Animator animator;
    GameObject player;
    Rigidbody2D rb;
    public float hor;
    public float speed = 1;
    internal bool isMoving = false;
    Jumping jumpScript;
    [HideInInspector] public bool canMove = true;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponentInChildren<Rigidbody2D>();
        jumpScript = GetComponent<Jumping>();
    }
    private void FixedUpdate()
    {
        Move();
        animator.SetFloat("Speed", rb.velocity.magnitude); 
    }
    public void Move()
    {
        if (canMove)
        {
            hor = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(hor * speed, rb.velocity.y);
            if (hor > 0)
            {
                player.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (hor < 0)
            {
                player.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
        }
    }
    public void stopMove()
    {
        canMove = false;
    }
}
