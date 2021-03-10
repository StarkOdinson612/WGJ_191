using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    Vector2 movement;
    public float speed = 10f;
    [Range(1f,5f)]
    public float scale = 2f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(rb.velocity.magnitude) > 0.1f)
        {
            anim.SetBool("isMoving", true);

            if (Mathf.Abs(movement.x) > 0.5f)
            {
                anim.SetBool("isMovingRight", true);

                if (movement.x > 0.5f)
                {
                    transform.localScale = new Vector3(scale, scale, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-scale, scale, 1);
                }
            }
            else
            {
                anim.SetBool("isMovingRight", false);
            }

            if (Mathf.Abs(movement.y) > 0.5f)
            {
                anim.SetBool("isMovingUp", true);

                if (movement.y > 0.5f)
                {
                    transform.localScale = new Vector3(scale, scale, 1);
                }
                else
                {
                    transform.localScale = new Vector3(scale, -scale, 1);
                }
            }
            else
            {
                anim.SetBool("isMovingUp", false);
            }
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(movement.x, movement.y, 0) * speed;
    }
}
