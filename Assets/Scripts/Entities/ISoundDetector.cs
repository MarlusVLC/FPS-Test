using UnityEngine;

namespace Entities
{
    public interface ISoundDetector
    {
        public float ProcessSound(float intensity, Vector3 sourcePosition);
    }
}