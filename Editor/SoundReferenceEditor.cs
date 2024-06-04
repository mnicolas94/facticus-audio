using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Utils.Editor;

namespace Audio.Editor
{
    [UnityEditor.CustomEditor(typeof(SoundReference))]
    public class SoundReferenceEditor : UnityEditor.Editor
    {
        [SerializeField] private SoundReference _target;
        
        public override VisualElement CreateInspectorGUI()
        {
            _target = (SoundReference)target;
            var root = new VisualElement();
            
            // draw default inspector
            var serializedProperties = PropertiesUtils.GetSerializedProperties(serializedObject);
            foreach (var serializedProperty in serializedProperties)
            {
                var propertyField = new PropertyField(serializedProperty);
                root.Add(propertyField);
            }

            // add button
            var playButton = new Button(PlayAudio);
            playButton.text = "Play";
            root.Add(playButton);
            
            return root;
        }

        private void PlayAudio()
        {
            _target.PlayAudio();
        }
    }
}