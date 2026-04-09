using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleUI : MonoBehaviour
{
    [Header("UI要素")]
    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private TextMeshProUGUI pressSpaceTetxt;
    void Start()
    {
        if (titleText == null)
        {
            titleText = GameObject.Find("TitleText")?.GetComponent<TextMeshProUGUI>();
        }

        if (pressSpaceTetxt == null)
        {
            pressSpaceTetxt = GameObject.Find("pressSpaceText")?.GetComponent<TextMeshProUGUI>();
        }
        if (titleText != null)
        {
            titleText.text = "MARIO SAMPLE";
        }
        if (pressSpaceTetxt != null)
        {
            pressSpaceTetxt.text = "PRESS SPACE TO START";
        }
    }
    void Update()
    {
        if (pressSpaceTetxt != null)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * 2f));
            Color color = pressSpaceTetxt.color;
            color.a = alpha;
            pressSpaceTetxt.color = color;
        }
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGame();
            }
        }
    }
}
