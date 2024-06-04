﻿﻿using System;
using UnityEngine;
using UnityEngine.UI;

  namespace Audio
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(AudioSource))]
    public class ButtonSound : MonoBehaviour
    {
        [SerializeField] private SoundReference _soundReference;
        
        private Button _button;
        private AudioSource _audioSource;
        
        private void Start()
        {
            _button = GetComponent<Button>();
            _audioSource = GetComponent<AudioSource>();

            _button.onClick.AddListener(PlayAudio);
        }

        private void PlayAudio()
        {
            var audioClip = GetAudio();
            _audioSource.PlayOneShot(audioClip);
        }

        private AudioClip GetAudio()
        {
            return _soundReference.GetAudioClip();
        }
    }
}