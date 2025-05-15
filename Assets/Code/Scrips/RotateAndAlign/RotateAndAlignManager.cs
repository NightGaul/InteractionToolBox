using Code.Scrips.VisualHelpers;
using Code.ScriptableObjectScripts;
using UnityEngine;

namespace Code.Scrips.RotateAndAlign
{
    public class RotateAndAlignManager : MonoBehaviour
    {
        [Header("Puzzle Pieces")]
        public RotatablePiece[] pieces;

        [Header("Rotate and Align Settings")]
        //yes this is a bit clunky but limits the ability to create buggy puzzles
        public DivisorOf360 rotationAngleDivisorOf360 = DivisorOf360.ONE;
        public RotationAxis rotationAxis;
        public bool useAutoScramble;
        [Space]
        public ScrollEventSO scrollEvent;
        
        
        [Header("Visuals")] public OutlineMode outlineMode;
        public Color outlineColor;
        [Range(0.1f,50f)]
        public float outlineWidth;
        
        [Header("Sounds")]
        public AudioClip clickSound;
        public AudioClip rightRotateSound;
        
        private bool _puzzleSolved;
        private void OnEnable()
        {
            foreach (var piece in pieces)
            {
                piece.SetScrollEvent(scrollEvent);
                piece.SetRotationSpeed((int)rotationAngleDivisorOf360);
                piece.SetOutline(outlineMode,outlineColor,outlineWidth);
                piece.SetRotationAxis(rotationAxis);
                piece.SetTargetAngle();
                if(useAutoScramble)piece.AutoScramble();
                piece.SetClickingClip(clickSound);
                piece.SetRightRotationClip(rightRotateSound);
            }
        }

        void Update()
        {
            if (_puzzleSolved) return;

            bool allSolved = true;
            
            foreach (var piece in pieces)
            {
                if (!piece.isSolved)
                {
                    allSolved = false;
                    break;
                }
            }

            if (allSolved)
            {
                _puzzleSolved = true;
                OnPuzzleSolved();
            }
        }

        private void OnPuzzleSolved()
        {
            Debug.Log("Puzzle Solved!");
            
            // TODO: Polishing determines what is gonna be added here
            // Example: Play sound, open a door, spawn fireworks, etc.
        }
    }
}