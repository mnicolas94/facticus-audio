using System.IO;
using UnityEditor;
using UnityEngine;
using Utils.Editor;

namespace Audio.Editor.Setup
{
    [CreateAssetMenu(fileName = "AudioSettingsEditor", menuName = "Facticus/Audio/AudioSettingsEditor", order = 0)]
    public class AudioSettingsEditor : ScriptableObjectSingleton<AudioSettingsEditor>
    {
        [SerializeField] public AudioSource musicPrefab;
        [SerializeField] public AudioSource voicePrefab;
        [SerializeField] public AudioSource sfxPrefab;
        [SerializeField] public AudioSource environmentPrefab;
        
        public static AudioSettingsEditor GetOrCreate()
        {
            if (Instance == null)
            {
                // create directory
                var dir = "Assets/Editor/FacticusAudio";
                Directory.CreateDirectory(dir);

                // create asset
                var settings = CreateInstance<AudioSettingsEditor>();
                var path = Path.Combine(dir, "AudioSettingsEditor.asset");
                AssetDatabase.CreateAsset(settings, path);
                AssetDatabase.SaveAssetIfDirty(settings);
            }

            return Instance;
        }
        
        [MenuItem("GameObject/Audio/Sfx audio")]
        private static void CreateSfxAudio()
        {
            InstantiateAudioPrefab(GetOrCreate().sfxPrefab);
        }
        
        [MenuItem("GameObject/Audio/Voice audio")]
        private static void CreateVoiceAudio()
        {
            InstantiateAudioPrefab(GetOrCreate().voicePrefab);
        }
        
        [MenuItem("GameObject/Audio/Environment audio")]
        private static void CreateEnvironmentAudio()
        {
            InstantiateAudioPrefab(GetOrCreate().environmentPrefab);
        }
        
        [MenuItem("GameObject/Audio/Music audio")]
        private static void CreateMusicAudio()
        {
            InstantiateAudioPrefab(GetOrCreate().musicPrefab);
        }

        private static void InstantiateAudioPrefab(AudioSource prefab)
        {
            var selectedObject = Selection.activeGameObject;
            var parent = selectedObject ? selectedObject.transform : null;
            PrefabUtility.InstantiatePrefab(prefab, parent);
        }
    }
}