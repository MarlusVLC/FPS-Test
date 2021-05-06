using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Players
{
    public class FirstPerson_InputHandler : MonoBehaviour
    {
        public static float HorizontalMove { get; private set; }
        public static float VerticalMove { get; private set; }
        public static float HorizontalView { get; private set; }
        public static float VerticalView { get; private set; }
        public static bool SprintKey { get; private set; }
        public static bool CrouchKey { get; private set; }
        public static bool AimKey { get; private set; }
        public static bool JumpKey { get; private set; }
        public static bool PrimaryFireKey_Auto { get; private set; }
        public static bool PrimaryFireKey_Manual{ get; private set; }
        public static bool ReloadKey { get; private set; }
        public static bool GrenadeThrowKey { get; private set; }
        public static float WeaponWheel { get; private set; }
        

        
        //Tentar implementar delegates
        public void Update()
        { 
            HorizontalMove = CrossPlatformInputManager.GetAxis("Horizontal");
            VerticalMove = CrossPlatformInputManager.GetAxis("Vertical");
            HorizontalView = CrossPlatformInputManager.GetAxis("Mouse X");
            VerticalView = CrossPlatformInputManager.GetAxis("Mouse Y");
            SprintKey = Input.GetKey(KeyCode.LeftShift);
            CrouchKey = Input.GetKeyDown(KeyCode.C);
            AimKey = Input.GetMouseButton(1);
            JumpKey =  CrossPlatformInputManager.GetButtonDown("Jump");
            PrimaryFireKey_Auto = Input.GetButton("Fire1");
            PrimaryFireKey_Manual = Input.GetButtonDown("Fire1");
            ReloadKey = Input.GetKeyDown(KeyCode.R);
            GrenadeThrowKey = Input.GetMouseButtonDown(2);
            WeaponWheel = Input.mouseScrollDelta.y;
        }
        
        
    }
}