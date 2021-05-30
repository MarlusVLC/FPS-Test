using DefaultNamespace;
using UnityEngine;

namespace AI.States
{
    public abstract class SensorialBeing : MonoCache
    {
        [Space(11)] [Header("Hearing")]
        [SerializeField] protected LayerMask soundObstacleMask;
        
        
        protected RaycastHit[] soundBlockingObstacles ;

        protected override void Awake()
        {
            base.Awake();
            // soundBlockingObstacles = new RaycastHit[5];
        }

        public abstract float ProcessSound(float intensity, Vector3 sourcePosition);

    }
}