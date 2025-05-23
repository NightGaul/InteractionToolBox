using Code.Scrips.Abstractions;
using Code.Scrips.VisualHelpers;
using Code.ScriptableObjectScripts;
using Editors;
using UnityEngine;

namespace Code.Scrips.RotateAndAlign
{
    public class RotateAndAlignManager : ManagerBase
    {
        [Header("Puzzle Pieces")] public RotatablePiece[] pieces;

        [Header("Rotate and Align Settings")]
        //yes this is a bit clunky but limits the ability to create buggy puzzles
        public DivisorOf360 rotationAngleDivisorOf360 = DivisorOf360.ONE;
        public RotationAxis rotationAxis;
        [Tooltip("It's recommended to set this to true!")]
        public bool useAutoScramble;
        [Space] public ScrollEventSO scrollEvent;


        [Header("Visuals")] public OutlineMode outlineMode;
        [ShowIfEnum("outlineMode", OutlineMode.ON_HOVER, OutlineMode.ALWAYS)]
        public Color outlineColor = Color.white;
        
        [ShowIfEnum("outlineMode", OutlineMode.ON_HOVER, OutlineMode.ALWAYS)]
        [Range(0.1f, 50f)] public float outlineWidth = 2f;

        [Header("Sounds")] public AudioClip clickSound;
        public AudioClip rightRotateSound;

        private bool _puzzleSolved;

        private void OnEnable()
        {
            PieceSetUp();
        }
        private void Update()
        {
            CheckPuzzleStatus();
        }

        private void PieceSetUp()
        {
            foreach (var piece in pieces)
            {
                piece.SetScrollEvent(scrollEvent);
                piece.SetRotationSpeed((int)rotationAngleDivisorOf360);
                piece.SetOutline(outlineMode, outlineColor, outlineWidth);
                piece.SetRotationAxis(rotationAxis);
                piece.SetTargetAngle();
                piece.SetClickingClip(clickSound);
                piece.SetRightRotationClip(rightRotateSound);
                if (useAutoScramble) piece.AutoScramble();
            }
        }


        private void CheckPuzzleStatus()
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
                Success();
            }
        }

        public override void Success()
        {
            _puzzleSolved = true;
            Debug.Log("Puzzle Solved!");
        }

        private void OnDestroy()
        {
            foreach (var piece in pieces)
            {
                Destroy(piece);
            }
        }
        
        
    }
}