using Code.Scrips.Abstractions;
using Code.Scrips.VisualHelpers;
using Code.ScriptableObjectScripts;
using UnityEngine;

namespace Code.Scrips.FetchAndMatch
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class DropOffPoint : AbstractInteraction
    {
        private Collider _collider;
        public FetchAndMatchManager manager;
        public GoalSO goal; // REPLACE isGoal with this

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
            
            OutlineSetup();
        }
        
        private void OutlineSetup()
        {
            VisualSetupHelper.AddOutlineComponents(gameObject.transform, manager.outlineMode, manager.outlineColor, manager.outlineWidth);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<MatchableObject>()!= null)
            {
                var goalContainer = other.GetComponent<MatchableObject>();
                if (goalContainer != null && manager.CheckForGoal(goalContainer.goal, goal))
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