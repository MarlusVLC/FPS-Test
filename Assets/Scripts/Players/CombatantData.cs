using System;
using UnityEngine;

namespace Players
{
    public class CombatantData : MonoBehaviour
    {
        [SerializeField] private Transform recoilPosition;
        [SerializeField] private Transform recoilRotation;


        public Transform RecoilPosition => recoilPosition;
        public Transform RecoilRotation => recoilRotation;
    }
}