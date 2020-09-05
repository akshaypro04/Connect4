using UnityEngine;

namespace Connect4.Comman.sound
{
    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;

        [Range(0, 1)]
        public float volume;

        [Range(0.3f, 3f)]
        public float pitch;

        public bool IsLoop;

        [HideInInspector]
        public AudioSource audioSource;

    }
}