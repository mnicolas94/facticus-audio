﻿﻿using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "SoundReference", menuName = "Facticus/Audio/SoundReference", order = 0)]
    public class SoundReference : ScriptableObject, IAudioClipProvider
    {
        [SerializeReference, SubclassSelector] private IAudioClipProvider _provider;

        public IAudioClipProvider Provider
        {
            get => _provider;
            set => _provider = value;
        }

        public AudioClip GetAudioClip()
        {
            return _provider.GetAudioClip();
        }
        
        public void PlayAudio()
        {
            var audio = GetAudioClip();
            AudioSource.PlayClipAtPoint(audio, Vector3.zero, 1);
        }
        
        public void PlayAudio(Vector3 position)
        {
            var audio = GetAudioClip();
            AudioSource.PlayClipAtPoint(audio, position, 1);
        }
        
        public void PlayAudio(AudioSource source)
        {
            var audio = GetAudioClip();
            source.PlayOneShot(audio);
        }
    }
}