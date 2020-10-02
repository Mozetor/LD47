using UnityEngine;
using UnityEngine.Audio;

namespace Sound {

    public class AudioClipPlayer : MonoBehaviour {
        public AudioMixerGroup mixerGroup;
        public AudioClip clip;

        private AudioSource source;

        void Start() {
            source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = mixerGroup;
            //source.volume = OptionsController.instance.options.sfxVolume * OptionsController.instance.options.masterVolume;
            source.loop = false;
            AudioUtils.SetAudioSource(source);
            source.clip = clip;
            source.Play();
        }

        private void Update() {
            if (!source.isPlaying) {
                Destroy(this);
            }
        }
    }
}