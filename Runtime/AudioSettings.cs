﻿﻿using System;
using System.Collections;
  using UnityEditor;
  using UnityEngine;
  using UnityEngine.Pool;
  using Utils.Serializables;

namespace Audio
{
    [Serializable]
    public class SoundReferenceDictionary : SerializableDictionary<SoundReference, SerializableAudioClipProvider>{}
    
    [CreateAssetMenu(fileName = "AudioReferences", menuName = "Facticus/Audio/AudioReferences", order = 0)]
    public class AudioSettings : ScriptableObject
    {
        [SerializeField, Obsolete] private SoundReferenceDictionary _referencedAudioClips;

        [SerializeField] private AudioReferenceSource _audioSourcePrefab;

        private ObjectPool<AudioReferenceSource> _droppedAudiosPool;

        private void OnEnable()
        {
            _droppedAudiosPool = new ObjectPool<AudioReferenceSource>(
                OnCreateDroppedAudio,
                OnGetDroppedAudio,
                OnReleaseDroppedAudio);
        }

        private AudioReferenceSource OnCreateDroppedAudio()
        {
            return Instantiate(_audioSourcePrefab);
        }

        private void OnGetDroppedAudio(AudioReferenceSource obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void OnReleaseDroppedAudio(AudioReferenceSource obj)
        {
            obj.gameObject.SetActive(false);
        }

        public void DropAudio(SoundReference reference)
        {
            var source = _droppedAudiosPool.Get();
            source.Reference = reference;
            source.Play();

            IEnumerator WaitUntilFinishes()
            {
                while (source.IsPlaying)
                {
                    yield return null;
                }
                
                _droppedAudiosPool.Release(source);
            }

            source.StartCoroutine(WaitUntilFinishes());
        }
        
        public void PlayAudio(SoundReference reference)
        {
            reference.PlayAudio();
        }
        
        public void PlayAudio(SoundReference reference, Vector3 position)
        {
            reference.PlayAudio(position);
        }
        
        public void PlayAudio(SoundReference reference, AudioSource source)
        {
            reference.PlayAudio(source);
        }

#if UNITY_EDITOR
        [ContextMenu("Migrate from v0.1.2")]
        private void Migrate012()
        {
            foreach (var (reference, provider) in _referencedAudioClips)
            {
                reference.Provider = provider.Provider;
                EditorUtility.SetDirty(reference);
            }
            AssetDatabase.SaveAssets();
        }
#endif
    }

    [Serializable]
    public class SerializableAudioClipProvider : IAudioClipProvider
    {
        [SerializeReference, SubclassSelector] private IAudioClipProvider _provider;

        public IAudioClipProvider Provider => _provider;

        public AudioClip GetAudioClip()
        {
            return _provider.GetAudioClip();
        }
    }
}