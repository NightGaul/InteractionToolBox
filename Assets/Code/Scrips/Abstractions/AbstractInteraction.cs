using UnityEngine;

namespace Code.Scrips.Abstractions
{
    public abstract class AbstractInteraction : MonoBehaviour
    {
        private string _interactionPrompt;
        
        public virtual string GetInteractionPrompt()
        {
            return _interactionPrompt;
        }
        
        void Interact()
        {
            throw new System.NotImplementedException();
        }

        void OnFocusEnter()
        {
            throw new System.NotImplementedException();
        }

        void OnFocusExit()
        {
            throw new System.NotImplementedException();
        }
    }
}
