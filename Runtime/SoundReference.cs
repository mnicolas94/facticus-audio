using System.Threading;
using Audio.Utils;
using UnityEngine;
using uPools;

namespace Audio
{
    [CreateAssetMenu(fileName = "SoundReference", menuName = "Facticus/Audio/SoundReference", order = 0)]
    public class SoundReference : ScriptableObject
    {
        [SerializeField] private AudioSource _prefabForAudioDrops;
        
        [Space(12)]
        [SerializeReference, SubclassSelector] private IAudioClipProvider _provider;

        public IAudioClipProvider Provider
        {
            get => _provider;
            set => _provider = value;
        }

        private CancellationTokenSource _cts;

        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
        }

        private void OnDisable()
        {
            if (!_cts.IsCancellationRequested)
            {
                _cts.Cancel();
            }

            _cts.Dispose();
            _cts = null;
        }
        
        private AudioClip GetAudioClip()
        {
            return _provider.GetAudioClip();
        }

        private AudioSource GetAudioSource()
        {
            if (_prefabForAudioDrops)
            {
                if (Application.isPlaying)
                {
                    return SharedGameObjectPool.Rent(_prefabForAudioDrops);
                }
                else
                {
                    return Instantiate(_prefabForAudioDrops);
                }
            }
            else
            {
                var gameObject = new GameObject("One shot audio");
                var audioSource = (AudioSource) gameObject.AddComponent(typeof (AudioSource));
                audioSource.spatialBlend = 1f;
                return audioSource;
            }
        }

        private void DisposeAudioSource(AudioSource audioSource)
        {
            var gameObject = audioSource.gameObject;
            if (!Application.isPlaying)
            {
                DestroyImmediate(gameObject);
            }
            else if (_prefabForAudioDrops)
            {
                SharedGameObjectPool.Return(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private async void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume=1.0f)
        {
            var audioSource = GetAudioSource();
            audioSource.transform.position = position;
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
            var timeToDestroy = clip.length * (Time.timeScale < 0.01f ? 0.01f : Time.timeScale);
            await AsyncUtils.Delay(timeToDestroy, _cts.Token);

            DisposeAudioSource(audioSource);
        }
        
        [ContextMenu(nameof(PlayAudio))]
        public void PlayAudio()
        {
            var audio = GetAudioClip();
            
            var position = Camera.main.transform.position;
            PlayClipAtPoint(audio, position);
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