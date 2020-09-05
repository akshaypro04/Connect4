using Connect4.Comman.Game;
using System;
using UnityEngine;
using Connect4.Comman.sound;

namespace Connect4.Comman.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;
        void Awake()
        {
            foreach (Sound s in sounds)
            {
                s.audioSource = gameObject.AddComponent<AudioSource>();
                s.audioSource.clip = s.clip;
                s.audioSource.volume = s.volume;
                s.audioSource.pitch = s.pitch;
                s.audioSource.loop = s.IsLoop;
            }
        }

        public void Play(string name)
        {
            if (GameManager.Instances.getAudio() == 0)
                return;

            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (s == null)
            {
                print("sound " + name + " doesn't found in array");
                return;
            }
            s.audioSource.Play();
        }

        void Start()
        {
            Play("music");
        }

        public void Stop(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (s == null)
            {
                print("sound " + name + " doesn't found in array");
                return;
            }
            s.audioSource.Stop();
        }
    }
}