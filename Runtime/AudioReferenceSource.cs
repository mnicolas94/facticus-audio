﻿using UnityEngine;
 using Utils.Attributes;

 namespace Audio
{
    public class AudioReferenceSource : MonoBehaviour
    {
        [SerializeField] private SoundReferenceEnum _reference;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField, AutoProperty(AutoPropertyMode.Asset)]
        private AudioReferences _audioReferences;
        
        public bool IsPlaying => _audioSource.isPlaying;
        
        public SoundReferenceEnum Reference
        {
            get => _reference;
            set => _reference = value;
        }

        public void Play()
        {
            var clip = _audioReferences.GetAudio(_reference);
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
            var clip = _audioReferences.GetAudio(_reference);
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