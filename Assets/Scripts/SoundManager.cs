using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("BGM")]
    [SerializeField]
    private AudioClip bgmTitle;
}
