using System.Collections.Generic;
using UnityEngine;

public enum Sound
{
    SCORE,
    SPECIAL_SCORE,
    HIT_BACKBOARD,
    HIT_FLOOR,
    SHOOT,
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager Instance;
    [SerializeField] private AudioClip[] soundList;
    [SerializeField] private SoundData soundData;
    private AudioSource m_AudioSource;
    private Dictionary<Sound, float> m_SoundVolumeMap;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_SoundVolumeMap = new Dictionary<Sound, float>
        {
            { Sound.SCORE, soundData.scoreSoundVolume },
            { Sound.SPECIAL_SCORE, soundData.perfectScoreSoundVolume },
            { Sound.HIT_BACKBOARD, soundData.hitBackboardSoundVolume },
            { Sound.HIT_FLOOR, soundData.hitFloorSoundVolume },
            { Sound.SHOOT, soundData.shootSoundVolume },
        };
    }

    public static void PlaySound(Sound sound)
    {
        Instance.m_AudioSource.PlayOneShot(Instance.soundList[(int)sound], Instance.m_SoundVolumeMap[sound]);
    }
}