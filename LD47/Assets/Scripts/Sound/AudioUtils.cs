using UnityEngine;

namespace Sound {

    public static class AudioUtils {

        public static void SetAudioSource(AudioSource audioSource) {
            audioSource.bypassEffects = true;
            audioSource.bypassListenerEffects = true;
            audioSource.bypassReverbZones = true;
        }
    }
}