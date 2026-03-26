using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyController : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private bool moveRight = true;

    [Header("移動範囲")]
    [SerializeField]

    private bool usePatrol = true;
    [SerializeField]
    private float patrolDistance = 3f;
    [Header("壁検知")]
    [SerializeField]

    private Transform wallCheck;
    [SerializeField]
    private float wallCheckDistance = 0.5f;
    [SerializeField]
    private LayerMask groundLayer;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private Vector3 startPosition;
    private float currentDirection = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        startPosition = transform.position;

        currentDirection = moveRight ? 1f : -1f;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (string.IsNullOrEmpty(gameObject.tag) || gameObject.tag == "Untagged")
        {
            gameObject.tag = "Enemy";
        }
    }

    void Update()
    {
        // if (GameManager.Instance != unll && GameManager.Instance.CurrentState != GameManager.GameState.Playing)
        // {
        //     rb.linearVelocity = Vector2.zero;
        //     return;
        // }
        if (usePatrol)
        {
            Patrol();
        }
        CheckWall();
        Move();
    }

    private void Patrol()
    {
        float distanceFromStart = transform.position.x - startPosition.x;

        if (distanceFromStart > patrolDistance)
        {
            currentDirection = -1f;
        }
        else if (distanceFromStart < -patrolDistance)
        {
            currentDirection = 1f;
        }
    }
    private void CheckWall()
    {
        Vector2 checkPos;
        if (wallCheck != null)
        {
            checkPos = wallCheck.position;
        }
        else
        {
            checkPos = (Vector2)transform.position + Vector2.right * currentDirection * 0.5f;
        }

        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.right * currentDirection, wallCheckDistance, groundLayer);
        if (hit.collider != null)
        {
            currentDirection *= -1f;
        }
    }
    private void Move()
    {
        rb.linearVelocity = new Vector2(currentDirection * moveSpeed, rb.linearVelocity.y);
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = currentDirection < 0;
        }
    }

    private void OGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 pos = Application.isPlaying ? startPosition : transform.position;
        Gizmos.DrawLine(pos + Vector3.left * patrolDistance, pos + Vector3.right * patrolDistance);

        Gizmos.color = Color.red;
        Vector3 wallCheckPos = wallCheck != null ? wallCheck.position : transform.position;
        float dir = Application.isPlaying ? currentDirection : (moveRight ? 1f : -1f);
        Gizmos.DrawRay(wallCheckPos, Vector3.right * dir * wallCheckDistance);
    }
}

