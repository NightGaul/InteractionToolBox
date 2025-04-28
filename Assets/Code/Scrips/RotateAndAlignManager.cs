using System;
using Code.ScriptableObjectScripts;
using UnityEngine;

namespace Code.Scrips
{
    public class RotateAndAlignManager : MonoBehaviour
    {
        [Header("Puzzle Pieces")]
        public RotatablePiece[] pieces;

        public ScrollEventSO scrollEvent;
        private bool _puzzleSolved = false;

        private void OnEnable()
        {
            foreach (var piece in pieces)
            {
                piece.scrollEvent = scrollEvent;
            }
        }

        void Update()
        {
            if (_puzzleSolved) return;

            bool allSolved = true;
            foreach (var piece in pieces)
            {
                if (!piece.CheckSolved())
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

            // TODO: Insert your own feedback here
            // Example: Play sound, open a door, spawn fireworks, etc.
        }
    }
}