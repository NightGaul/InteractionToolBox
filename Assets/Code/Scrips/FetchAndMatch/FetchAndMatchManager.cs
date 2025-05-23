using System;
using Code.Scrips.Abstractions;
using Code.Scrips.VisualHelpers;
using Code.ScriptableObjectScripts;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Scrips.FetchAndMatch
{
    public class FetchAndMatchManager : ManagerBase
    {
        [Header("FetchAndMatch Settings")]
        public InteractEventSO interactEvent;
        public Transform handTransform;

        [Header("Visuals")] public OutlineMode outlineMode;
        public Color outlineColor = Color.white;
        [Range(0.1f, 50f)]
        public float outlineWidth = 2f;

        [Header("Sounds")] public AudioClip putDownSound;
        public AudioClip successSound;

        private bool _isOnGoal;
        private Transform _objectGroup;
        private Transform _objectBeingHeld;
        private bool _holdingSomething;
        private Transform _possibleSnapObject;

        private AudioSource _dropAudioSource;
        private AudioSource _successAudioSource;

        private void Start()
        {
            AudioSetup();
        }

        private void AudioSetup()
        {
            _dropAudioSource = handTransform.AddComponent<AudioSource>();
            _successAudioSource = handTransform.AddComponent<AudioSource>();
            _successAudioSource.clip = successSound;
        }
        private void OnEnable()
        {
            interactEvent.onInteract += HandleInteract;
        }

        private void OnDisable()
        {
            interactEvent.onInteract -= HandleInteract;
        }

        private void HandleInteract(RaycastHit objHit, bool isInteractable)
        {
            var obj = objHit.transform;
            if (_holdingSomething)
            {
                Drop();
            }
            else if (isInteractable)
            {
                Grab(obj.transform);
            }
        }


        private void Grab(Transform objectToGrab)
        {
            _dropAudioSource.PlayOneShot(putDownSound);
            _objectGroup = objectToGrab.parent;
            objectToGrab.SetParent(handTransform, false);
            objectToGrab.transform.localPosition = Vector3.zero;
            _objectBeingHeld = objectToGrab;
            _holdingSomething = true;
        }

        private void Drop()
        {
            
            CheckForSuccessfulDrop();
            
            _objectBeingHeld.SetParent(_objectGroup, true);
            _objectBeingHeld = null;
            _holdingSomething = false;
            
            _dropAudioSource.PlayOneShot(putDownSound);
        }

        private void CheckForSuccessfulDrop()
        {
            if (_isOnGoal && (_possibleSnapObject != null))
            {
                _objectBeingHeld.transform.position = _possibleSnapObject.position;
                Success();
            }
        }
        public bool CheckForGoal(GoalSO objectGoal, GoalSO targetGoal)
        {
            return objectGoal != null && targetGoal != null && objectGoal == targetGoal;
        }

        public override void Success()
        {
            _successAudioSource.Play();
            Debug.Log("success");
        }

        public void SetIsOnGoal(bool isOnGoal)
        {
            _isOnGoal = isOnGoal;
        }

        public void SetPossibleSnapObject(Transform possibleSnapObject)
        {
            _possibleSnapObject = possibleSnapObject;
        }

        private void OnDestroy()
        {
            Destroy(_dropAudioSource);
            Destroy(_successAudioSource);
        }
    }
}