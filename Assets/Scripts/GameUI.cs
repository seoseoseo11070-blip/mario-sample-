using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]

    private TextMeshProUGUI timerText;

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
        if (GameManager.Instance == null) return;
        if (itemCountText != null)
        {
            itemCountText.text = "ITEMS: " +
            GameManager.Instance.GetItemCount() + "/" +
            GameManager.Instance.GetRequiredItemCount();
        }

        if (scoreText != null)
        {
            scoreText.text = "SCORE: " +
            GameManager.Instance.GetScore();
        }
        if (timerText != null)
        {
            int timeInt = Mathf.CeilToInt(GameManager.Instance.GetRemainingTime());
            timerText.text = "TIME: " + timeInt;
            if (timeInt <= 10)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = Color.white;
            }
        }
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
