using UnityEngine;

namespace DrivingSimulation
{
    public class BGMusicManager : SingletonDontDestroy<BGMusicManager>
    {
        public AudioClip[] bgAudios = null;
        public bool isPlaying = true;

        private int currentPlayed = 0;
        private AudioSource source = null;


        protected override void Awake()
        {
            base.Awake();

            Initialize();
        }

        private void Update()
        {
            PlayAudio();
        }

        void Initialize()
        {
            if (!TryGetComponent(out source))
            {
                source = gameObject.AddComponent<AudioSource>();
                return;
            }
        }

        void PlayAudio()
        {
            if (!isPlaying) return;
            if (bgAudios.Length == 0) return;

            if (!source.isPlaying)
            {
                source.clip = bgAudios[currentPlayed];
                source.Play();

                currentPlayed++;

                if (currentPlayed > bgAudios.Length - 1)
                {
                    currentPlayed = 0;
                }
            }
        }
    }
}
