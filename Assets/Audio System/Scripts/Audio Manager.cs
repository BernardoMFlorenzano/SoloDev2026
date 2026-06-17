using UnityEngine;

namespace AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [SerializeField] private AudioData audioData;

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        public void PlayMenuMusic()
        {
            if (musicSource.clip != audioData.menuMusic)
            {
                musicSource.clip = audioData.menuMusic;
                musicSource.loop = true;

            }
        }

        public void PlayGameMusic()
        {
            if (musicSource.clip != audioData.gameMusic)
            {
                musicSource.clip = audioData.gameMusic;
                musicSource.loop = true;

            }
        }

        public void PlayMusic(AudioSource audioSource, AudioClip music)
        {
            AudioSource usedSource;
            usedSource = audioSource ? audioSource : musicSource;

            if (usedSource.clip != music)
            {
                usedSource.clip = music;
                usedSource.loop = true;
            }
        }

        public void PlaySFX(AudioSource audioSource, AudioClip sfx)
        {
            AudioSource usedSource;
            usedSource = audioSource ? audioSource : sfxSource;

            usedSource.PlayOneShot(sfx);
        }
    }
}