using System;
using UnityEditor;
using UnityEngine;
using Utils;
using Utils.Serializables;

namespace Audio
{
    [Serializable]
    public class SoundReferenceDictionary : SerializableDictionary<SoundReference, SerializableAudioClipProvider>{}
    
    [CreateAssetMenu(fileName = "AudioReferences", menuName = "Facticus/Audio/AudioReferences", order = 0)]
    public class AudioSettings : ScriptableObjectSingleton<AudioSettings>
    {
        [SerializeField, HideInInspector, Obsolete] private SoundReferenceDictionary _referencedAudioClips;
        
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
    public class SerializableAudioClipProvider
    {
        [SerializeReference, SubclassSelector] private IAudioClipProvider _provider;

        public IAudioClipProvider Provider => _provider;

        public AudioClip GetAudioClip()
        {
            return _provider.GetAudioClip();
        }
    }
}