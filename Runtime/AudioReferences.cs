﻿﻿using System;
using System.Collections;
using UnityEngine;
using Utils.Serializables;

namespace Audio
{
    [Serializable]
    public class SoundReferenceDictionary : SerializableDictionary<SoundReferenceEnum, SerializableAudioClipProvider>{}
    
    
    [CreateAssetMenu(fileName = "AudioReferences", menuName = "Facticus/Audio/AudioReferences", order = 0)]
    public class AudioReferences : ScriptableObject
    {
        [SerializeField]
        private SoundReferenceDictionary _referencedAudioClips;

        [SerializeField] private AudioReferenceSource _audioSourcePrefab;
        
        public AudioClip GetAudio(SoundReferenceEnum reference)
        {
            var audios = _referencedAudioClips;
            return audios[reference].GetAudioClip();
        }

        public void DropAudio(SoundReferenceEnum reference)
        {
            var source = Instantiate(_audioSourcePrefab);
            source.Reference = reference;
            source.Play();

            IEnumerator WaitUntilFinishes()
            {
                while (source.IsPlaying)
                {
                    yield return null;
                }
                
                Destroy(source.gameObject);
            }

            source.StartCoroutine(WaitUntilFinishes());
        }
        
        public void PlayAudio(SoundReferenceEnum reference)
        {
            var audio = GetAudio(reference);
            AudioSource.PlayClipAtPoint(audio, Vector3.zero, 1);
        }
        
        public void PlayAudio(SoundReferenceEnum reference, Vector3 position)
        {
            var audio = GetAudio(reference);
            AudioSource.PlayClipAtPoint(audio, position, 1);
        }
        
        public void PlayAudio(SoundReferenceEnum reference, AudioSource source)
        {
            var audio = GetAudio(reference);
            source.PlayOneShot(audio);
        }
    }

    [Serializable]
    public class SerializableAudioClipProvider : IAudioClipProvider
    {
        [SerializeReference, SubclassSelector] private IAudioClipProvider _provider;
        
        public AudioClip GetAudioClip()
        {
            return _provider.GetAudioClip();
        }
    }
}