
using UnityEngine;

using System.Collections.Generic;

namespace Coda.Tools
{
    public class SoundableObject
    {
        public SoundableObject(GameObject obj)
        {
            _myObj = obj;
        }


        public bool isPlaying
        {
            get
            {
                for (int i = 0; i < _audioList.Count; i++)
                {
                    if (_audioList[i].isPlaying)
                        return true;
                }
                return false;
            }
        }


        /// <summary>
        /// Stop all Sound.
        /// </summary>
        public void Stop()
        {
            _AbortBeforeSound();
        }


        /// <summary>
        /// Play a sound.
        /// </summary>
        /// <param name="clip">Sound.</param>
        /// <param name="pitch">Speed of sound.</param>
        /// <param name="abortBefore">Abort object's sound and play new sound.</param>
        /// <param name="loop">New sound loop or not</param>
        /// <param name="dimension">0 is 2D, 1 is 3D</param>
        public void PlaySound(AudioClip clip, float pitch = 1, bool abortBefore = true, bool loop = false, float dimension = 0)
        {
            if (clip == null)
                return;

            if (abortBefore)
                _AbortBeforeSound();

            AudioSource source = _GetOrCreateSource();
            source.clip = clip;
            source.pitch = pitch;
            source.loop = loop;
            source.spatialBlend = dimension;
            source.playOnAwake = false;
            source.Play();
        }


        #region Private Part

        private GameObject _myObj;
        private List<AudioSource> _audioList = new List<AudioSource>();

        private void _AbortBeforeSound()
        {
            for (int i = 0; i < _audioList.Count; i++)
            {
                _audioList[i].Stop();
                _audioList[i].clip = null;
            }
        }

        private AudioSource _GetOrCreateSource()
        {
            for (int i = 0; i < _audioList.Count; i++)
            {
                if (!_audioList[i].isPlaying)
                    return _audioList[i];
            }

            AudioSource source = _myObj.AddComponent<AudioSource>();
            _audioList.Add(source);
            return source;
        }

        #endregion
    }
}