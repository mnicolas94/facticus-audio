﻿using UnityEngine;

 namespace Audio
{
    public class AudioReferenceSource : MonoBehaviour
    {
        [SerializeField] private SoundReference _reference;
        [SerializeField] private AudioSource _audioSource;
        
        public bool IsPlaying => _audioSource.isPlaying;
        
        public SoundReference Reference
        {
            get => _reference;
            set => _reference = value;
        }

        public void Play()
        {
            if (_audioSource != null)
            {
                _reference.PlayAudio(_audioSource);
            }
            else
            {
                _reference.PlayAudio(transform.position);
            }
        }
        
        public void PlayOneShot()
        {
            Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}