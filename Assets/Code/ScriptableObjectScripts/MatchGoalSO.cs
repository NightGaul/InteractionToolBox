using UnityEngine;
namespace Code.ScriptableObjectScripts
{
    [CreateAssetMenu(menuName = "FetchAndMatch/Goal")]
    public class MatchGoalSO : ScriptableObject
    {
        public string goalName;
        public AudioClip successSound;
        public Sprite icon;
    }
}