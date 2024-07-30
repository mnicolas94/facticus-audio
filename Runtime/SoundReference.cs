using System.Threading.Tasks;
using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "SoundReference", menuName = "Facticus/Audio/SoundReference", order = 0)]
    public class SoundReference : ScriptableObject
    {
        [SerializeReference, SubclassSelector] private IAudioClipProvider _provider;

        public IAudioClipProvider Provider
        {
            get => _provider;
            set => _provider = value;
        }
        
        private AudioClip GetAudioClip()
        {
            return _provider.GetAudioClip();
        }
        
        private async void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume=1.0f)
        {
            GameObject gameObject = new GameObject("One shot audio");
            gameObject.transform.position = position;
            AudioSource audioSource = (AudioSource) gameObject.AddComponent(typeof (AudioSource));
            audioSource.clip = clip;
            audioSource.spatialBlend = 1f;
            audioSource.volume = volume;
            audioSource.Play();
            var timeToDestroy = clip.length * (Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale);
#if UNITY_EDITOR
            await Task.Delay((int) (timeToDestroy * 1000));
            DestroyImmediate(gameObject);
#else
            Destroy(gameObject, timeToDestroy);
#endif
        }
        
        [ContextMenu(nameof(PlayAudio))]
        public void PlayAudio()
        {
            var audio = GetAudioClip();
            PlayClipAtPoint(audio, Vector3.zero);
        }
        
        public void PlayAudio(Vector3 position)
        {
            var audio = GetAudioClip();
            PlayClipAtPoint(audio, position);
        }
        
        public void PlayAudio(AudioSource source)
        {
            var audio = GetAudioClip();
            source.PlayOneShot(audio);
        }
    }
}