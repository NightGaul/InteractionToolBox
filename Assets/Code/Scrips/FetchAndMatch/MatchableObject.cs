using System;
using Code.ScriptableObjectScripts;
using UnityEngine;
namespace Code.Scrips.FetchAndMatch
{
    

    public class MatchableObject : MonoBehaviour
    {
        public GoalSO goal;

        private void Start()
        {
            gameObject.tag = "Interactable";
        }
    }
}