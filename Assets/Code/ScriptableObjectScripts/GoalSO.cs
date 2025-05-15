using UnityEngine;
namespace Code.ScriptableObjectScripts
{
    [CreateAssetMenu(fileName = "NewGoal", menuName = "FetchAndMatch/Goal", order = 1)]
    public class GoalSO : ScriptableObject
    {
        public string goalId;
    }
}