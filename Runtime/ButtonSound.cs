﻿﻿using System;
using UnityEngine;
using UnityEngine.UI;
  using Utils.Attributes;

  namespace Audio
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(AudioSource))]
    public class ButtonSound : MonoBehaviour
    {
        [SerializeField] private SoundReferenceEnum _soundReference;
        
        [SerializeField, AutoProperty(AutoPropertyMode.Asset)]
        private AudioReferences _audioReferences;

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
            return _audioReferences.GetAudio(_soundReference);
        }
    }
}