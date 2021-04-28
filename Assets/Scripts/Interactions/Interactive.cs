using System;
using UnityEngine;

namespace DefaultNamespace.Interactions
{
    public abstract class Interactive : MonoBehaviour
    {
        private void OnValidate()
        {
            MakeFullyInteractable(LayerMask.NameToLayer("Interactable"));
        }

        public abstract void Interact();
     
        
        
        
        private void MakeFullyInteractable(int interactableLayerNum)
        {
            if (!gameObject.layer.Equals(interactableLayerNum))
            {
                gameObject.layer = interactableLayerNum;
            }
        }
    }
    
    

}