using System;
using UnityEngine;

namespace Players
{
    public class AnimationHandler : MonoBehaviour
    {
        private Animator _anim;
        private Camera _fpCam;


        private void Start()
        {
            _fpCam = GetComponentInChildren<Camera>(); 
        }

        public Animator Anim
        {
            get => _anim;
            set => _anim = value;
        }
        
        
        public void SetMovement(float speed)
        {
            //new Vector3(m_MoveDir.x,0,m_MoveDir.z).magnitude
            _anim.SetFloat("Velocity", speed);
        }

        public void SetAim(bool isAiming)
        {
            _anim.SetBool("isAiming", isAiming);
            if (isAiming && _fpCam.fieldOfView > 40)
            {
                _fpCam.fieldOfView = 40;
            }
            else if (!isAiming && _fpCam.fieldOfView <= 40)
            {
                _fpCam.fieldOfView = 60;
            }
        }

        public void SetFire(bool isFiring)
        {
            _anim.SetBool("isFiring", isFiring);
        }

    }
}