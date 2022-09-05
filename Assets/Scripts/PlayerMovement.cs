using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float gravityScale;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float climbSpeed;
    [SerializeField] Vector2 deathKick;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    CapsuleCollider2D capsuleCollider2D; // Body Collider
    BoxCollider2D boxCollider2D;         // Feet
    Animator animator;
    Vector2 moveInput;
    Rigidbody2D rb2d;

    bool isAlive = true;

    void Start()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        gravityScale = rb2d.gravityScale;
    }

    void Update()
    {
        if (!isAlive) return;
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) return;
        moveInput = value.Get<Vector2>();
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rb2d.velocity.y);
        rb2d.velocity = playerVelocity;

        // Animación de correr
        bool isRunning = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", isRunning);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) return;
        if (!boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        if (value.isPressed)
        {
            // Parte de 25 (ejemplo, jumpSpeed = 25f) entonces baja a 0 (por la gravedad)
            // Dando la impresión que su velocidad disminuye pero como es positivo se eleva y luego cae
            // Más rápido porque de 0 baja a negativos su velocidad hasta tocar suelo
            rb2d.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) return;
        Instantiate(bullet, gun.position, transform.rotation);
    }

    // Voltear Sprite
    void FlipSprite()
    {
        // Mathf.Epsilon es el valor flotante más diminuto cercano a 0
        // Más seguro y preciso que usar 0
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (!boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            rb2d.gravityScale = gravityScale;
            return;
        }

        Vector2 climbVelocity = new Vector2(rb2d.velocity.x, moveInput.y * climbSpeed);
        rb2d.velocity = climbVelocity;
        rb2d.gravityScale = 0f;

        bool isClimbing = Mathf.Abs(rb2d.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", isClimbing);
    }

    void Die()
    {
        if (capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            rb2d.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
