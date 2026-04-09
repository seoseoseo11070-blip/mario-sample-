using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameClearUI : MonoBehaviour
{
    [Header("UI要素")]
    [SerializeField]
    private TextMeshProUGUI gameClearText;
    [SerializeField]
    private TextMeshProUGUI pressSpaceText;

    void Start()
    {
        if (gameClearText == null)
        {
            gameClearText = GameObject.Find("GameClearText")?.GetComponent<TextMeshProUGUI>();
        }

        if (pressSpaceText == null)
        {
            pressSpaceText = GameObject.Find("PressSpaceText")?.GetComponent<TextMeshProUGUI>();
        }

        if (gameClearText != null)
        {
            gameClearText.text = "GAME CLEAR";
        }

        if (pressSpaceText != null)
        {
            pressSpaceText.text = "PRESS SPACE TO TITLE";
        }
    }
    void Update()
    {
        if (pressSpaceText != null)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * 2f));
            Color color = pressSpaceText.color;
            color.a = alpha;
            pressSpaceText.color = color;
        }

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ReturnToTitle();
            }
        }
    }
}
