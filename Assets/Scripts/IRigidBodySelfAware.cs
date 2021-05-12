using UnityEngine;

namespace DefaultNamespace
{
    public interface IRigidBodySelfAware
    {
        public void ReceiveForce(Vector3 force, ForceMode mode = ForceMode.Force);

        public void ReceiveExplosionForce(float explosionForce, Vector3 epicenter, float explosionRadius,
            float upwardsModifier = 0.0f, ForceMode mode = ForceMode.Force);
    }
}