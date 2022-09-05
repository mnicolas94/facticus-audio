﻿﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Utils.Extensions;

namespace Audio
{
    [Serializable]
    public class RandomAudioProvider : IAudioClipProvider
    {
        [SerializeField] private List<AudioClip> _audioClips;


        public AudioClip GetAudioClip()
        {
            return _audioClips.GetRandom();
        }
    }
}