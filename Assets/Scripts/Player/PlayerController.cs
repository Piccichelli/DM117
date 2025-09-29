using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    Vector2 input;

    public float speed = 3f;
    public float jumpForce = 5f;


    public bool isGrounded = false;
    public bool canJump = false;
    public bool canDash = true;

    public float dashSpeed = 10f;
    private bool isDashing = false;
    private float dashTime = 0.2f;
    private float lastDirection = 1f; //1 direita, -1 esquerda
    public float dashCooldown = 1f; // tempo de espera no chão

    public float maxFallSpeed = -10f;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        //andando
        rb2d.linearVelocityX = input.x * speed;

        //controlando o dash
        if (isDashing)
        {
            rb2d.linearVelocity = new Vector2(lastDirection * dashSpeed, rb2d.linearVelocityY);

            dashTime -= Time.deltaTime;
            if (dashTime <= 0f)
            {
                isDashing = false;
                dashTime = 0.2f;
            }
        }
    }

    void FixedUpdate()
    {
        // Limita a velocidade de queda
        if (rb2d.linearVelocityY < maxFallSpeed)
        {
            rb2d.linearVelocityY = maxFallSpeed;
        }
    }

    public void OnMove(InputValue value)
    {
        if(!isDashing)
        {
            input = value.Get<Vector2>();

            if(input.x != 0f)
            {
                lastDirection = Mathf.Sign(input.x);
            }
        }
    }

    public void OnJump(InputValue value)
    {
        if(isGrounded || canJump)
        {
            rb2d.linearVelocityY = jumpForce;
            isGrounded = false;
            canJump = false;
        }
    }

    public void OnDash(InputValue value)
    {
        if(canDash)
        {
            isDashing = true;
            if (isGrounded)
            {
                canDash = false;
                StartCoroutine(DashCooldown());
            }
            else
            {
                isDashing = true;
                canDash = false;
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ResetJump"))
        {
            canJump = true;
            StartCoroutine(TemporarilyDisable(collision.gameObject, 3f));
        }
        if (collision.CompareTag("ResetDash"))
        {
            canDash = true;
            StartCoroutine(TemporarilyDisable(collision.gameObject, 3f));
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && IsGrounded(collision))
        {
            canDash = true;
            isGrounded = true;
        }
    }

    private bool IsGrounded(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Vector2.Angle(contact.normal, Vector2.up) < 5f)
            {
                return true; // encontrou chão
            }
        }
        return false; // nenhum contato aponta para cima
    }

    // Coroutine para esperar o cooldown no chão
    private System.Collections.IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // Coroutine que desativa e reativa depois
    private System.Collections.IEnumerator TemporarilyDisable(GameObject obj, float delay)
    {
        obj.SetActive(false);       // desativa o objeto
        yield return new WaitForSeconds(delay); // espera 3 segundos
        obj.SetActive(true);        // reativa o objeto
    }
}
