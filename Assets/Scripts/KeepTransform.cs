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
                // print("Posicao: " + transform.position);
                // print("Rotacap: " + transform.rotation);
            }
            else
            {
                // print("gaaaaaaaaaaaaaaaaaaaaaaaaaaaaaay");
                _originalTransform = transform;
            }
        }
    }
}