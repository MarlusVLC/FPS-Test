using System;
using UnityEngine;

namespace Entities
{
    public interface ISoundEmitter
    {
        // public ISoundDetector[] listeners;
        // public float _audioIntensity;
        public event Action<float, Vector3> SoundEmitted;
        public void OnSoundEmitted(float audioScale, Vector3 sourcePosition);
    }
}