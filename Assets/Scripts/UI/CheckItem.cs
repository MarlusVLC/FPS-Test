using System;
using DefaultNamespace.Interactions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace UI
{
    
    public class CheckItem : MonoBehaviour
    {
        [SerializeField] private float checkerLineDistance;
        [SerializeField] private LayerMask collectablesMask;
        [SerializeField] private LayerMask targetsMask;
        [SerializeField] private LayerMask interactablesMask;
        
        [SerializeField] private Image crossHair;

        private Color _collectableColor, _targetColor, _interactableColor;
        private Color _colorToBeSet;

        private GameObject _linecastedObj;
        private Interactive _interactable;

        void Start()
        {
            _collectableColor = Color.blue;
            _targetColor = Color.red;
            _interactableColor = Color.green;
        }
        void Update()
        {
            if (Physics.Linecast(transform.position, transform.position + transform.forward * checkerLineDistance, collectablesMask))
            {
                _colorToBeSet = _collectableColor;
            }
            else if (Physics.Linecast(transform.position, transform.position + transform.forward * 100,
                targetsMask))
            {
                _colorToBeSet = _targetColor;
            }
            else if (Physics.Linecast(transform.position, transform.position + transform.forward * 10, interactablesMask))
            {
                _colorToBeSet = _interactableColor;
            }
            else
            {
                _colorToBeSet = Color.black;
            }


            if (_colorToBeSet != crossHair.color) 
            { 
                crossHair.color = _colorToBeSet;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _colorToBeSet;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * checkerLineDistance);
        }
    }
}