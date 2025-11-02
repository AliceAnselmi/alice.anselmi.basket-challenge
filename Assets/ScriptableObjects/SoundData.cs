using UnityEngine;
[CreateAssetMenu(fileName = "SoundData", menuName = "Game Data/Sound Data")]
public class SoundData : ScriptableObject
{
    [Range(0f,1f)] public float scoreSoundVolume;
    [Range(0f,1f)] public float perfectScoreSoundVolume;
    [Range(0f,1f)] public float hitBackboardSoundVolume;
    [Range(0f,1f)] public float hitFloorSoundVolume;
    [Range(0f,1f)] public float timerSoundVolume;
    [Range(0f,1f)] public float shootSoundVolume; 
}
