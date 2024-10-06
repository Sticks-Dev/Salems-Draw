using Kickstarter.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Salems_Draw
{
    [RequireComponent(typeof(AudioSource))]
    public class WitchSoundEffects : MonoBehaviour
    {
        [SerializeField, EnumData(typeof(WitchActions))] private AudioClip[] audioClips;

        private Dictionary<WitchActions, AudioClip> clips = new Dictionary<WitchActions, AudioClip>();
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            clips.LoadDictionary(audioClips);
            transform.root.GetComponent<Witch>().OnAction += PlaySound;
        }

        private void PlaySound(string key)
        {
            if (!Enum.TryParse(key, out WitchActions action))
                return;
            audioSource.PlayOneShot(clips[action]);
        }

        private enum WitchActions
        {
            Relax,
            Wander,
            Chase,
            CastSpell,
        }
    }
}
