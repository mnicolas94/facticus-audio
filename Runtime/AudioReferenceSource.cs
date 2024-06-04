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
            var clip = _reference.GetAudioClip();
            if (_audioSource != null)
            {
                _audioSource.clip = clip;
                _audioSource.Play();
            }
            else
            {
                AudioSource.PlayClipAtPoint(clip, transform.position);
            }
        }
        
        public void PlayOneShot()
        {
            var clip = _reference.GetAudioClip();
            if (_audioSource != null)
            {
                _audioSource.PlayOneShot(clip);
            }
            else
            {
                AudioSource.PlayClipAtPoint(clip, transform.position);
            }
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}