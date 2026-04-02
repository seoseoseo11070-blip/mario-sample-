using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class ltemController : MonoBehaviour
{
    [Header("アニメーション設定")]
    [SerializeField]
    private bool useFloatAnimation = true;

    [SerializeField]
    private float floatSpeed = 2f;

    [SerializeField]
    private float floatAmount = 0.3f;

    [Header("回転設定")]
    [SerializeField]
    private bool useRotation = false;

    [SerializeField]
    private float rotationSpeed = 90f;

    private Vector3 startPosition;

    private float elapsedTime = 0f;


    void Start()
    {

        startPosition = transform.position;
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }

        if (string.IsNullOrEmpty(gameObject.tag) || gameObject.tag == "Untagged")
        {
            gameObject.tag = "Item";
        }
    }
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameManager.GameState.Playing)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        if (useFloatAnimation)
        {
            FloatAnimation();
        }
        if (useRotation)
        {
            RotateAnimation();
        }
    }

    private void FloatAnimation()
    {
        float newY = startPosition.y + Mathf.Sin(elapsedTime * floatSpeed) * floatAmount;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
    private void RotateAnimation()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    private void OiggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CollectItem();
            }
            Destroy(gameObject);
        }
    }
}
