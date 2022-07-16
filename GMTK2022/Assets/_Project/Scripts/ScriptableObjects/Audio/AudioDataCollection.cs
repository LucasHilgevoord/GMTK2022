using UnityEngine;

/// <summary>
/// AudioDataCollection holds the AudioData objects
/// </summary>
[CreateAssetMenu(fileName = "AudioDataCollection", menuName = "ScriptableObjects/AudioDataCollection")]
public class AudioDataCollection : ScriptableObject
{
    [SerializeField]
    private AudioData[] audioDatas;

    public AudioData[] GetCollection()
    {
        return this.audioDatas;
    }
}
