using UnityEngine;

namespace DefaultNamespace
{
    public class KeepTransform : MonoBehaviour
    {
        private Transform _originalTransform;
        


        void OnEnable()
        {
            if (_originalTransform != null)
            {
                transform.position = _originalTransform.position;
                transform.rotation = _originalTransform.rotation;
            }
            else
            {
                _originalTransform = transform;
            }
        }
    }
}