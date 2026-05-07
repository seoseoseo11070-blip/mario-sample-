using UnityEngine;
public class MovingPlatform : MonoBehaviour
{
    // 移動方向の種類
    public enum MoveDirection
    {
        Horizontal,
        Vertical
    }

    [Header("移動設定")]
    [SerializeField]
    private MoveDirection direction = MoveDirection.Horizontal;

    [SerializeField]
    private float moveDistance = 3f;

    [SerializeField]
    private float moveSpeed = 2f;

    // 内部変数
    private Vector3 startPosition;
    private float elapsedTime = 0f;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {

        if (GameManager.Instance != null &&
            GameManager.Instance.CurrentState
                != GameManager.GameState.Playing)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        float offset = Mathf.Sin(elapsedTime * moveSpeed)
                       * moveDistance;

        if (direction == MoveDirection.Horizontal)
        {
            transform.position = new Vector3(
                startPosition.x + offset,
                startPosition.y,
                startPosition.z
            );
        }
        else
        {
            transform.position = new Vector3(
                startPosition.x,
                startPosition.y + offset,
                startPosition.z
            );
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float playerBottom = collision.transform.position.y -
                collision.collider.bounds.extents.y;
            float platformTop = transform.position.y +
                GetComponent<Collider2D>().bounds.extents.y;

            if (playerBottom >= platformTop - 0.1f)
            {
                collision.transform.SetParent(transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}

