using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float jumpForce = 10f;
    [Header("接地判定")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundCheckRadius = 0.2f;
    [SerializeField]
    private LayerMask groundLayer;
    [Header("落下判定")]
    [SerializeField]
    private float fallThreshold = -10f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;


    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();


        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {

        CheckGround();


        HandleMovement();


        HandleJump();


        CheckFall();
    }


    private void CheckGround()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {

            isGrounded = Physics2D.OverlapCircle(
                transform.position + Vector3.down * 0.5f,
                groundCheckRadius,
                groundLayer
            );
        }
    }


    private void HandleMovement()
    {
        float horizontal = 0f;


        if (Keyboard.current != null)
        {

            if (Keyboard.current.leftArrowKey.isPressed)
            {
                horizontal = -1f;
            }
            else if (Keyboard.current.rightArrowKey.isPressed)
            {
                horizontal = 1f;
            }
        }

        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);

        if (horizontal != 0 && spriteRenderer != null)
        {
            spriteRenderer.flipX = horizontal < 0;
        }
    }


    private void HandleJump()
    {

        if (Keyboard.current != null && Keyboard.current.upArrowKey.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }


    private void CheckFall()
    {
        if (transform.position.y < fallThreshold)
        {
            Debug.Log("プレイヤーが落下した");
            transform.position = new Vector3(0, 1, 0);
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 checkPos = groundCheck != null ? groundCheck.position : transform.position + Vector3.down * 0.5f;
        Gizmos.DrawWireSphere(checkPos, groundCheckRadius);
    }
}