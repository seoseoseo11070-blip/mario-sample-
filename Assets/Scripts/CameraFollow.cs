using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("追跡対象")]
    [SerializeField] private Transform target;

    [Header("追跡設定")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);

    [Header("カメラ制限")]
    [SerializeField] private bool useBounds = true;
    [SerializeField] private float minX = 0f;
    [SerializeField] private float maxX = 100f;
    [SerializeField] private float minY = 0f;
    [SerializeField] private float maxY = 10f;

    private void Start()
    {
        FindPlayer();
    }

    private void FindPlayer()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }
    }

    private void LateUpdate()
    {
        FindPlayer();
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        desiredPosition.z = transform.position.z;

        if (useBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    private void OnDrawGizmosSelected()
    {
        if (!useBounds) return;

        Gizmos.color = Color.cyan;
        Vector3 center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, transform.position.z);
        Vector3 size = new Vector3(maxX - minX, maxY - minY, 1);
        Gizmos.DrawWireCube(center, size);
    }
}