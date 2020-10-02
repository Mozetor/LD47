using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Sound {

    public class RandomWeaponSound : MonoBehaviour {
        public List<AudioClipList> creatingClips = new List<AudioClipList>();
        public List<AudioClipList> destroyClips = new List<AudioClipList>();
        public AudioMixerGroup mixerGroup;
        public GameObject audioClipPlayerPrefab;

        /// <summary>
        /// Plays a random clip from the List.
        /// </summary>
        //public void PlayClip(Transform transform, WeaponType weaponType, bool bulledHit = false) {
        //    if ((bulledHit && (destroyClips.Count == 0 || destroyClips[(int)weaponType].audioClips.Count == 0)) || (!bulledHit && (creatingClips.Count == 0 || creatingClips[(int)weaponType].audioClips.Count == 0))) {
        //        return;
        //    }
        //    int weapon = (int)weaponType;
        //    AudioClip clip;
        //    if (bulledHit) {
        //        int randomD = Random.Range(0, destroyClips[weapon].audioClips.Count);
        //        clip = destroyClips[weapon].audioClips[randomD];
        //    }
        //    else {
        //        int randomC = Random.Range(0, creatingClips[weapon].audioClips.Count);
        //        clip = creatingClips[weapon].audioClips[randomC];
        //    }
        //    GameObject gameObject = Instantiate(audioClipPlayerPrefab, transform.position, Quaternion.identity, null);
        //    AudioClipPlayer player = gameObject.GetComponent<AudioClipPlayer>();
        //    player.clip = clip;
        //}

        [System.Serializable]
        public class AudioClipList {
            //public WeaponType weaponType;
            public List<AudioClip> audioClips = new List<AudioClip>();
        }
    }
}