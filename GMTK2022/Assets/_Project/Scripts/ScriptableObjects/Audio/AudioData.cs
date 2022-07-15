using UnityEngine;

/// <summary>
/// AudioData objects hold the info of the sounds
/// </summary>
[CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObjects/AudioData")]
public class AudioData : ScriptableObject
{
    [SerializeField]
    private AudioClip clip;

    [SerializeField]
    public float volume = 1.0f;

    [Range (-2, 4)]
    [SerializeField]
    public float pitch = 1.0f;

    [SerializeField]
    public bool randomPitch;

    public string AudioName;

    public AudioClip GetClip() => clip;
} 



