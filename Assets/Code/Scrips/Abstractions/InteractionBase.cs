using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scrips.Abstractions
{
    public class AbstractInteraction : MonoBehaviour
    {
        public String interactionPrompt;
        
        public virtual void Interact()
        {
            throw new System.NotImplementedException();
        }
        
    }
}
