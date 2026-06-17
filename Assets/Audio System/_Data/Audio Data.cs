using UnityEngine;

namespace AudioSystem
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/AudioData")]
    public class AudioData : ScriptableObject
    {
        [Header("Music")]
        public AudioClip menuMusic;
        public AudioClip gameMusic;
    }
}