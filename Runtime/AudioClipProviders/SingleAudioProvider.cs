﻿﻿using System;
using UnityEngine;

namespace Audio
{
    [Serializable]
    public class SingleAudioProvider : IAudioClipProvider
    {
        [SerializeField] private AudioClip _audioClip;
        
        public AudioClip GetAudioClip()
        {
            return _audioClip;
        }
    }
}