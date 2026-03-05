using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform tareget;

    private float smoothSpeed = 5f;

    private Vector3 offset = new Vector3(0, -10);

    private bool useBounds = true;

    private float minx = 0f;

    private float maxX = 100f;

    private float minY = 0f;

    private float maxY = 10f;

    void Start()
    {
        if (tareget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                tareget = player.transform;
            }
        }
    }

    void LateUpdate()
    {
        if (tareget == null) return;

        Vector3 desiredPosition = tareget.position + offset;

        if (useBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minx, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
    private void OawGizmosSelected()
    {
        if (!useBounds) return;
        Gizmos.color = Color.cyan;
        Vector3 center = new Vector3((minx + maxX) / 2, (minY + maxY) / 2, 0);
        Vector3 size = new Vector3(maxX - minx, maxY - minY, 1);
        Gizmos.DrawWireCube(center, size);
    }
}
