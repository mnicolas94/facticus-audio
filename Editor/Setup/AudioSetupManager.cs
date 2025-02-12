using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio.Editor.Setup
{
    public class AudioSetupManager : ScriptableObject
    {
        [SerializeField] public AudioMixer audioMixer;
        [SerializeField] public AudioSource musicPrefabTemplate;
        [SerializeField] public AudioSource voicePrefabTemplate;
        [SerializeField] public AudioSource sfxPrefabTemplate;
        [SerializeField] public AudioSource environmentPrefabTemplate;

        private static AudioSetupManager _templatesContainer;
        private static AudioSetupManager TemplatesContainer => _templatesContainer ??= CreateInstance<AudioSetupManager>();
        
        [MenuItem("Tools/Facticus.Audio/Trigger initial audio setup")]
        private static void InitialSetup()
        {
            var directory = EditorUtility.OpenFolderPanel("Select Directory", "Assets", "");
            
            if (string.IsNullOrEmpty(directory))
                return;
            
            directory = Path.GetRelativePath(".", directory);
            if (!Directory.Exists(directory) || !directory.StartsWith("Assets"))
            {
                Debug.LogWarning($"Cannot create audio setup, directory is not valid: {directory}");
                return;
            }
                
            // create new container
            var container = TemplatesContainer;
            
            Undo.SetCurrentGroupName("Setup audio prefabs and mixers");
            var group = Undo.GetCurrentGroup();

            var settings = AudioSettingsEditor.GetOrCreate();
            Undo.RecordObject(settings, "Set audio prefabs into settings");
            var newMixer = CopyAssetIntoDirectory(container.audioMixer, directory);
            settings.musicPrefab = CopyAssetIntoDirectory(container.musicPrefabTemplate, directory);
            settings.voicePrefab = CopyAssetIntoDirectory(container.voicePrefabTemplate, directory);
            settings.sfxPrefab = CopyAssetIntoDirectory(container.sfxPrefabTemplate, directory);
            settings.environmentPrefab = CopyAssetIntoDirectory(container.environmentPrefabTemplate, directory);
            UpdateMixerReference(settings.musicPrefab, container.audioMixer, newMixer);
            UpdateMixerReference(settings.voicePrefab, container.audioMixer, newMixer);
            UpdateMixerReference(settings.sfxPrefab, container.audioMixer, newMixer);
            UpdateMixerReference(settings.environmentPrefab, container.audioMixer, newMixer);
            
            EditorUtility.SetDirty(settings);
            
            Undo.CollapseUndoOperations(group);
        }

        private static T CopyAssetIntoDirectory<T>(T asset, string directory) where T : Object
        {
            var assetPath = AssetDatabase.GetAssetPath(asset);
            var assetName = asset.name;
            assetName = assetName.Replace("Template", "");
            var extension = Path.GetExtension(assetPath);
            var newPath = Path.Combine(directory, $"{assetName}{extension}");
            AssetDatabase.CopyAsset(assetPath, newPath);
            var newAsset = AssetDatabase.LoadAssetAtPath<T>(newPath);
            
            // Undo.RegisterCreatedObjectUndo(newAsset, $"Created new {assetName}");
            return newAsset;
        }

        private static void UpdateMixerReference(AudioSource source, AudioMixer oldMixer, AudioMixer newMixer)
        {
            var groupName = source.outputAudioMixerGroup.name;
            var groups = newMixer.FindMatchingGroups("");
            foreach (var group in groups)
            {
                if (group.name == groupName)
                {
                    source.outputAudioMixerGroup = group;
                    return;
                }
            }
        }
    }
}