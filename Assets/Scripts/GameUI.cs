using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [Header("UI要素")]
    [SerializeField]
    private TextMeshProUGUI itemCountText;
    void Start()
    {
        if (itemCountText == null)
        {
            itemCountText = GameObject.Find("ItemCountText")?.GetComponent<TextMeshProUGUI>();
        }
        UpdateUI();
    }
    void Update()
    {
        UpdateUI();
    }


    private void UpdateUI()
    {
        if (itemCountText != null && GameManager.Instance != null)
        {
            int current = GameManager.Instance.GetItemCount();
            int required = GameManager.Instance.GetRequiredItemCount();
            itemCountText.text = "ITEMS: " + current + " / " + required;
        }
    }
}
