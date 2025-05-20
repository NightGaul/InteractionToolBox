using System;
using Code.Scrips.VisualHelpers;
using Code.ScriptableObjectScripts;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scrips.RotateAndAlign
{
    [RequireComponent(typeof(BoxCollider))]
    public class RotatablePiece : MonoBehaviour
    {
        [Header("Rotation Settings")]
        [Tooltip(
            "If you set this manually be very careful! It has to be dividable by the rotation-angle in the manager!")]
        public float targetAngle;

        [Tooltip("Don't touch this with your unwashed finger!")]
        public bool isSolved;

        private ScrollEventSO _scrollEvent;

        private OutlineMode _outlineMode;
        private Color _outlineColor;
        private float _outlineWidth;

        private int _rotationSpeed = 200;
        private RotationAxis _rotationAxis;
        private const float _TOLERANCE = 0.1f;


        private bool _isMouseOver;
        private bool _snapped;


        private AudioSource _clickingSoundSource;
        private AudioClip _clickingSound;
        private AudioSource _rightRotationSource;
        private AudioClip _rightRotationSound;

        private void Start()
        {
            _scrollEvent.onScroll += Scroll;

            var outline = this.AddComponent<Outline>();
            outline.OutlineColor = _outlineColor;
            outline.OutlineWidth = _outlineWidth;

            switch (_outlineMode)
            {
                case (OutlineMode.ALWAYS):
                    this.AddComponent<OutlineAlways>();
                    break;
                case (OutlineMode.ON_HOVER):
                    this.AddComponent<OutlineOnHover>();
                    break;
            }

            //difficulties with fast spinning
            _clickingSoundSource = this.AddComponent<AudioSource>();
            _clickingSoundSource.clip = _clickingSound;
            _rightRotationSource = this.AddComponent<AudioSource>();
            _rightRotationSource.clip = _rightRotationSound;
        }

        public void SetTargetAngle()
        {
            switch (_rotationAxis)
            {
                case RotationAxis.X:
                    targetAngle = transform.localRotation.x;
                    break;
                case RotationAxis.Y:
                    targetAngle = transform.localRotation.y;
                    break;
                case RotationAxis.Z:
                    targetAngle = transform.localRotation.z;
                    break;
                default:
                    targetAngle = transform.localRotation.x;
                    break;
            }
        }

        private void OnDisable()
        {
            _scrollEvent.onScroll -= Scroll;
        }


        private void Rotate(float input)
        {
            switch (_rotationAxis)
            {
                case RotationAxis.X:
                    transform.Rotate(-input * _rotationSpeed, 0, 0);
                    break;
                case RotationAxis.Y:
                    transform.Rotate(0, -input * _rotationSpeed, 0);
                    break;
                case RotationAxis.Z:
                    transform.Rotate(0, 0, input * _rotationSpeed);
                    break;
            }

            _clickingSoundSource.Play();
            CheckSolved();
        }

        public void AutoScramble()
        {
            var tick = 360 / _rotationSpeed;

            int randomRotation = Mathf.RoundToInt(Random.Range(1, tick)) * _rotationSpeed;
            switch (_rotationAxis)
            {
                case RotationAxis.X:
                    transform.Rotate(randomRotation, 0, 0);
                    break;
                case RotationAxis.Y:
                    transform.Rotate(0, randomRotation, 0);
                    break;
                case RotationAxis.Z:
                    transform.Rotate(0, 0, randomRotation);
                    break;
            }
        }

        private void CheckSolved()
        {
            float currentAngle;
            switch (_rotationAxis)
            {
                case RotationAxis.X:
                    currentAngle = transform.localRotation.x;
                    break;
                case RotationAxis.Y:
                    currentAngle = transform.localRotation.y;
                    break;
                case RotationAxis.Z:
                    currentAngle = transform.localRotation.z;
                    break;
                default:
                    currentAngle = transform.localRotation.x;
                    break;
            }

            isSolved = (Mathf.Abs(Math.Abs(currentAngle) - Math.Abs(targetAngle)) < _TOLERANCE);
            if (isSolved) _rightRotationSource.Play();
        }


        private void Scroll(float delta)
        {
            if (_isMouseOver)
            {
                Rotate(delta);
            }
        }

        private void OnMouseEnter()
        {
            _isMouseOver = true;
        }

        private void OnMouseExit()
        {
            _isMouseOver = false;
        }


        public void SetRotationSpeed(int input)
        {
            _rotationSpeed = input;
        }


        public void SetScrollEvent(ScrollEventSO scrollEvent)
        {
            _scrollEvent = scrollEvent;
        }

        public void SetOutline(OutlineMode outlineMode, Color outlineColor, float outlineWidth)
        {
            _outlineMode = outlineMode;
            _outlineColor = outlineColor;
            _outlineWidth = outlineWidth;
        }

        public void SetRotationAxis(RotationAxis rotationAxis)
        {
            _rotationAxis = rotationAxis;
        }

        public void SetClickingClip(AudioClip clip)
        {
            _clickingSound = clip;
        }

        public void SetRightRotationClip(AudioClip clip)
        {
            _rightRotationSound = clip;
        }
    }
}