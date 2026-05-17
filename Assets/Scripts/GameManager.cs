using System.Security.Permissions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体を管理するマネージャークラス
/// EmptyObjectにアタッチして使用
/// </summary>
public class GameManager : MonoBehaviour
{

    [Header("スコア設定")]
    [SerializeField]
    private int scorrPerItem = 100;
    [SerializeField]
    private int scorePerStomp = 200;
    [SerializeField]
    private int timeBonus = 10;
    [Header("タイマー設定")]
    [SerializeField]
    private float timeLimit = 60f;

    private int score = 0;
    private float remainingTime;

    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        Title,
        Playing,
        GameOver,
        GameClear
    }

    // 現在のゲーム状態
    public GameState CurrentState { get; private set; }

    // アイテム取得数
    private int itemCount = 0;

    // クリアに必要なアイテム数
    [SerializeField]
    private int requiredItemCount = 3;

    void Awake()
    {
        // シングルトンパターン
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 現在のシーン名から状態を設定
        UpdateStateFromScene();
    }

    void Update()
    {
        if (CurrentState == GameState.Playing)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                GameOver();
                return;
            }
        }
        // タイトル、ゲームオーバー、ゲームクリア画面でスペースキー入力を処理
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            switch (CurrentState)
            {
                case GameState.Title:
                    StartGame();
                    break;
                case GameState.GameOver:
                case GameState.GameClear:
                    ReturnToTitle();
                    break;
            }
        }
    }

    /// <summary>
    /// 現在のシーン名からゲーム状態を更新
    /// </summary>
    private void UpdateStateFromScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "TitleScene":
                CurrentState = GameState.Title;
                break;
            case "GameScene":
                CurrentState = GameState.Playing;
                break;
            case "GameOverScene":
                CurrentState = GameState.GameOver;
                break;
            case "GameClearScene":
                CurrentState = GameState.GameClear;
                break;
        }
        if (sceneName == "TitleScene" && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM("title");
        }
    }

    /// <summary>
    /// ゲームを開始する
    /// </summary>
    public void StartGame()
    {
        itemCount = 0;
        score = 0;
        remainingTime = timeLimit;
        CurrentState = GameState.Playing;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM("game");
        }
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// タイトル画面に戻る
    /// </summary>
    public void ReturnToTitle()
    {
        itemCount = 0;
        CurrentState = GameState.Title;
        SceneManager.LoadScene("TitleScene");
    }

    /// <summary>
    /// ゲームオーバーにする
    /// </summary>
    public void GameOver()
    {
        CurrentState = GameState.GameOver;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlaySE("gameover");
        }
        SceneManager.LoadScene("GameOverScene");
    }

    /// <summary>
    /// ゲームクリアにする
    /// </summary>
    public void GameClear()
    {
        CurrentState = GameState.GameClear;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlaySE("clear");
        }
        SceneManager.LoadScene("GameClearScene");
    }

    /// <summary>
    /// アイテムを取得した時に呼ばれる
    /// </summary>
    public void CollectItem()
    {
        itemCount++;
        score += scorrPerItem;
        Debug.Log("スコア: " + score + "アイテム取得: " + itemCount + " / " + requiredItemCount);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE("item");
        }
        // クリア条件を達成したらゲームクリア
        if (itemCount >= requiredItemCount)
        {
            int bonus = Mathf.CeilToInt(remainingTime) * timeBonus;
            score += bonus;
            GameClear();
        }
    }

    /// <summary>
    /// 現在のアイテム数を取得
    /// </summary>
    public int GetItemCount()
    {
        return itemCount;
    }

    /// <summary>
    /// 必要なアイテム数を取得
    /// </summary>
    public int GetRequiredItemCount()
    {
        return requiredItemCount;
    }
    public void AddScore(int points)
    {
        score += points;
    }
    public int GetScore()
    {
        return score;
    }
    public float GetRemainingTime()
    {
        return remainingTime;
    }
}
