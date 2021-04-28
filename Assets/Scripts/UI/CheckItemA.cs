using System;
using DefaultNamespace.Interactions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace UI
{
    
    public class CheckItemA : MonoBehaviour
    {
        [SerializeField] private float checkerLineDistance;
        [SerializeField] private LayerMask collectablesMask;
        [SerializeField] private LayerMask targetsMask;
        [SerializeField] private LayerMask interactablesMask;
        
        [SerializeField] private Image crossHair;

        private Color _collectableColor, _targetColor, _interactableColor;
        private Color _colorToBeSet;

        private Interactive _interactable;

        void Start()
        {
            _collectableColor = Color.blue;
            _targetColor = Color.red;
            _interactableColor = Color.green;
        }
        void Update()
        {
            RaycastHit hit;
            // Vector3 fwd = transform.TransformDirection(Vector3.forward);
            
            if (Physics.Linecast(transform.position, transform.position + transform.forward * checkerLineDistance,out hit, collectablesMask))
            {
                // _isAimingAtCollectable = true;
                _colorToBeSet = _collectableColor;
            }
            else if (Physics.Linecast(transform.position, transform.position + transform.forward * 100,
                out hit, targetsMask))
            {
                _colorToBeSet = _targetColor;
            }
            else if (Physics.Linecast(transform.position, transform.position + transform.forward * 10,
                out hit, interactablesMask))
            {
                if (_interactable == null)
                {
                    _interactable = hit.collider.gameObject.GetComponent<Interactive>();
                }
                else if (Input.GetKeyDown(KeyCode.B))
                {
                    _interactable.Interact();
                }
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


            if (_interactable != null)
            {
                if (!Physics.Linecast(transform.position, transform.position + transform.forward * 10,
                    interactablesMask))
                {
                    _interactable = null;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _colorToBeSet;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * checkerLineDistance);
        }


        private void SetCrossHairColor()
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
    }
}