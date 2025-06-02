using Code.Scrips.Abstractions;
using Code.Scrips.VisualHelpers;
using Code.ScriptableObjectScripts;
using Editors;
using UnityEngine;

namespace Code.Scrips.RotateAndAlign
{
    public class RotateAndAlignManager : ManagerBase
    {

        [Header("Rotate and Align Settings")]
        public ScrollEventSO scrollEvent;
        [Space]
        //yes this is a bit clunky but limits the ability to create buggy puzzles
        [Tooltip("The angle of rotation")]
        public DivisorOf360 rotationAngleDivisorOf360 = DivisorOf360.ONE;
        [Tooltip("The axis of rotation")]
        public RotationAxis rotationAxis;
        [Tooltip("It's recommended to set this to true!")]
        public bool useAutoScramble;
        
        [Header("Rotatable Pieces")] public RotatablePiece[] pieces;


        [Header("Visuals")] public OutlineMode outlineMode;
        [ShowIfEnum("outlineMode", OutlineMode.ON_HOVER, OutlineMode.ALWAYS)]
        public Color outlineColor = Color.white;
        
        [ShowIfEnum("outlineMode", OutlineMode.ON_HOVER, OutlineMode.ALWAYS)]
        [Range(0.1f, 50f)] public float outlineWidth = 2f;

        [Header("Sounds")] public AudioClip rotationSound;
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

        // Applies settings such as rotation, axis, outline, and audio to each piece.
        // Optionally scrambles pieces at the start if auto-scramble is enabled.
        private void PieceSetUp()
        {
            foreach (var piece in pieces)
            {
                piece.tag = "Scrollable";
                piece.SetScrollEvent(scrollEvent);
                piece.SetRotationSpeed((int)rotationAngleDivisorOf360);
                piece.SetOutline(outlineMode, outlineColor, outlineWidth);
                piece.SetRotationAxis(rotationAxis);
                piece.SetTargetAngle();
                piece.SetClickingClip(rotationSound);
                piece.SetRightRotationClip(rightRotateSound);
                if (useAutoScramble) piece.AutoScramble();
            }
        }

        // Verifies if all pieces are correctly aligned.
        // Triggers success logic when the puzzle is solved.
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

        // Marks the puzzle as solved and logs a message to the console.
        public override void Success()
        {
            _puzzleSolved = true;
            Debug.Log("Puzzle Solved!");
        }

        // Cleans up each rotatable piece by destroying them to free resources.
        private void OnDestroy()
        {
            foreach (var piece in pieces)
            {
                Destroy(piece);
            }
        }
        
        
    }
}