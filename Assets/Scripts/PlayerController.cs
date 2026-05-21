using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{

    [Header("ダブルジャンプ設定")]
    [SerializeField]
    private int maxJumpCount = 2;

    private int currentJumpCount = 0;

    [Header("踏みつけ設定")]
    [SerializeField]

    private float stompBounceForce = 8f;

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

    // 状態
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameManager.GameState.Playing)
        {
            return;
        }

        CheckGround();

        HandleMovement();

        HandleJump();

        CheckFall();
    }


    private void CheckGround()
    {

        bool wasGrounded = isGrounded;

        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            isGrounded = Physics2D.OverlapCircle(transform.position + Vector3.down * 0.5f, groundCheckRadius, groundLayer);
        }

        if (!wasGrounded && isGrounded)
        {
            currentJumpCount = 0;
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
        if (Keyboard.current != null && Keyboard.current.upArrowKey.wasPressedThisFrame && currentJumpCount < maxJumpCount)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySE("jump");
            }
            currentJumpCount++;
        }
    }


    private void CheckFall()
    {
        if (transform.position.y < fallThreshold)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float playerBottom = transform.position.y - GetComponent<BoxCollider2D>().bounds.extents.y;
            float enemyCenter = collision.transform.position.y;
            if (playerBottom > enemyCenter)
            {
                EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.OnStomped();
                    // 
                    SoundManager.Instance.PlaySE("stomp");
                }

                rb.linearVelocity = new Vector2(rb.linearVelocity.x, stompBounceForce);
            }
            else
            {

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.GameOver();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CollectItem();
            }
            Destroy(other.gameObject);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 checkPos = groundCheck != null ? groundCheck.position : transform.position + Vector3.down * 0.5f;
        Gizmos.DrawWireSphere(checkPos, groundCheckRadius);
    }


    // private void CheckGround()
    // {
    //     bool wasGrounded = isGrounded;

    //     if (groundCheck != null)
    //     {
    //         isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    //     }
    //     else
    //     {
    //         isGrounded = Physics2D.OverlapCircle(transform.position + Vector3.down * 0.5f, groundCheckRadius, groundLayer);
    //     }

    //     if (!wasGrounded && isGrounded)
    //     {
    //         currentJumpCount = 0;
    //     }

    // }

}
