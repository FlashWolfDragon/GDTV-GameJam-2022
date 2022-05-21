using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float playerSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;
    // [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    // float gravityScaleAtStart;

    Rigidbody2D rb2d;
    SpriteRenderer rend;
    Animator anim;
    [SerializeField] Collider2D bodyCollider;
    [SerializeField] Collider2D feetCollider;
    bool isAlive = true;
    PlayerInput controls;
    InputAction jumpAction;
    Vector2 moveInput;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        // anim = GetComponent<Animator>();
        // bodyCollider = GetComponent<CapsuleCollider2D>();
        // feetCollider = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        controls = new PlayerInput();
        jumpAction = controls.Player.Jump;
        controls.Enable();
    }

    void Update()
    {
        if (!isAlive) return;

        Run();
        JumpGravity();
        FlipSprite();
        // Die();
    }

    void JumpGravity()
    {
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb2d.velocity.y >= 0 && !jumpAction.IsPressed())
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.velocity.x), 1f);
        }
    }

    void Run()
    {
        float playerXVelocity = moveInput.x * playerSpeed;
        Vector2 playerVelocity = new Vector2(playerXVelocity, rb2d.velocity.y);
        rb2d.velocity = playerVelocity;

        bool isPlayerMoving = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        // anim.SetBool("isRunning", isPlayerMoving);

    }

    // void Die()
    // {
    //     if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
    //     {
    //         isAlive = false;
    //         anim.SetTrigger("Dying");
    //         rb2d.velocity = new Vector2(-Mathf.Sign(rb2d.velocity.x) * deathKick.x, deathKick.y);
    //         rend.color = new Color32(255, 0, 0, 255);

    //         FindObjectOfType<GameSession>().ProcessPlayerDeath();
    //     }
    // }

    void OnMove(InputValue value)
    {
        if (!isAlive) return;

        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) return;

        var isGrounded = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (!isGrounded) return;

        if (value.isPressed)
        {
            rb2d.velocity += new Vector2(0f, jumpSpeed);
        }
    }
}
