using UnityEngine;

public class StageBuilder : MonoBehaviour
{
    [Header("=== ステージ設定 ===")]
    [SerializeField] private float blockSize = 1f;
    [SerializeField] private Vector2 stageOffset = Vector2.zero;

    [Header("=== プレハブ ===")]
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject itemPrefab;

    // ステージデータ（0=何もない, 1=ブロック, 2=敵, 3=アイテム）
    private readonly int[,] stageData = new int[,]
    {
       { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
       { 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0 },
       { 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
       { 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0 },
       { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
       { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
       { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
       { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
    };

    private void Start()
    {
        BuildStage();
    }

    private void BuildStage()
    {
        int height = stageData.GetLength(0);
        int width = stageData.GetLength(1);

        ClearChildren("Blocks");
        ClearChildren("Enemies");
        ClearChildren("Items");

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int cellType = stageData[y, x];
                if (cellType == 0) continue;
                float posY = (height - 1 - y) * blockSize + stageOffset.y;
                Vector3 position = new Vector3(
                    x * blockSize + stageOffset.x,
                    posY,
                    0f
                );

                switch (cellType)
                {
                    case 1:
                        SpawnObject(blockPrefab, position, "Blocks");
                        break;
                    case 2:
                        SpawnObject(enemyPrefab, position, "Enemies");
                        break;
                    case 3:
                        SpawnObject(itemPrefab, position, "Items");
                        break;
                }
            }
        }
    }

    private void SpawnObject(GameObject prefab, Vector3 position, string parentName)
    {
        if (prefab == null)
        {
            Debug.LogWarning($"プレハブが設定されていません: {parentName}");
            return;
        }

        Transform parent = GetOrCreateParent(parentName);
        GameObject obj = Instantiate(prefab, position, Quaternion.identity, parent);
        obj.name = $"{prefab.name} ({position.x:F1}, {position.y:F1})";
    }

    private Transform GetOrCreateParent(string name)
    {
        Transform parent = transform.Find(name);
        if (parent == null)
        {
            GameObject go = new GameObject(name);
            parent = go.transform;
            parent.SetParent(transform);
            parent.localPosition = Vector3.zero;
        }
        return parent;
    }

    private void ClearChildren(string parentName)
    {
        Transform parent = transform.Find(parentName);
        if (parent == null) return;

#if UNITY_EDITOR
        while (parent.childCount > 0)
            DestroyImmediate(parent.GetChild(0).gameObject);
#else
        for (int i = parent.childCount - 1; i >= 0; i--)
            Destroy(parent.GetChild(i).gameObject);
#endif
    }

    private void OnDrawGizmos()
    {
        if (stageData == null) return;

        int height = stageData.GetLength(0);
        int width = stageData.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int cellType = stageData[y, x];
                if (cellType == 0) continue;
                float posY = (height - 1 - y) * blockSize + stageOffset.y;
                Vector3 position = new Vector3(
                    x * blockSize + stageOffset.x,
                    posY,
                    0f
                );

                switch (cellType)
                {
                    case 1:
                        Gizmos.color = Color.gray;
                        Gizmos.DrawWireCube(position, Vector3.one * blockSize * 0.9f);
                        break;
                    case 2:
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireSphere(position, blockSize * 0.4f);
                        break;
                    case 3:
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawWireSphere(position, blockSize * 0.3f);
                        break;
                }
            }
        }
    }
}
