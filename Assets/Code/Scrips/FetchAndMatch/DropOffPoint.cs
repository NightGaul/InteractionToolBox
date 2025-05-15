using Code.Scrips.Abstractions;
using Code.Scrips.VisualHelpers;
using UnityEngine;

namespace Code.Scrips.FetchAndMatch
{
    [RequireComponent(typeof(Collider))]
    public class DropOffPoint: AbstractInteraction
    {
        private Collider _collider;
        public FetchAndMatchManager manager;
        public bool isGoal;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
            
            var outline = gameObject.AddComponent<Outline>();
            outline.OutlineColor = manager.outlineColor;
            outline.OutlineWidth = manager.outlineWidth;
            switch (manager.outlineMode)
            {
                case (OutlineMode.ALWAYS):
                    gameObject.AddComponent<OutlineAlways>();
                    break;
                case (OutlineMode.ON_HOVER):
                    gameObject.AddComponent<OutlineOnHover3D>();
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Interactable"))
            {
                if (manager.CheckForGoal(isGoal))
                {
                    manager.SetIsOnGoal(true);
                    manager.SetPossibleSnapObject(transform);
                }
               
            }
        }

        private void OnTriggerExit(Collider other)
        {
            manager.SetIsOnGoal(false);
            manager.SetPossibleSnapObject(null);
        }
        
        
    }
}