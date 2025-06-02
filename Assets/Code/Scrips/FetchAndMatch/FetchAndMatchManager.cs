using Code.Scrips.Abstractions;
using Code.Scrips.VisualHelpers;
using Code.ScriptableObjectScripts;
using Editors;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Scrips.FetchAndMatch
{
    public class FetchAndMatchManager : ManagerBase
    {
        [Header("FetchAndMatch Settings")]
        public InteractEventSO interactEvent;
        [Space]
        [Tooltip("Gameobject that is used for the position while carrying")]
        public Transform playerTransform;

        [Header("Visuals")] public OutlineMode outlineMode;
        [ShowIfEnum("outlineMode", OutlineMode.ON_HOVER, OutlineMode.ALWAYS)]
        public Color outlineColor = Color.white;
        
        [ShowIfEnum("outlineMode", OutlineMode.ON_HOVER, OutlineMode.ALWAYS)]
        [Range(0.1f, 50f)] public float outlineWidth = 2f;

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

        // Adds audio sources to the playerTransform for handling in-game audio feedback.
        private void AudioSetup()
        {
            _dropAudioSource = playerTransform.AddComponent<AudioSource>();
            _successAudioSource = playerTransform.AddComponent<AudioSource>();
        }
        // Subscribes to interaction events when the script is enabled.
        private void OnEnable()
        {
            interactEvent.onInteract += HandleInteract;
        }

        // Unsubscribes from interaction events to avoid dangling references.
        private void OnDisable()
        {
            interactEvent.onInteract -= HandleInteract;
        }

        // Handles interaction input: either grabs an interactable object or drops the held one.
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


        // Attaches the interactable object to the player, marking it as held.
        private void Grab(Transform objectToGrab)
        {
            _dropAudioSource.PlayOneShot(putDownSound);
            _objectGroup = objectToGrab.parent;
            objectToGrab.SetParent(playerTransform, false);
            objectToGrab.transform.localPosition = Vector3.zero;
            _objectBeingHeld = objectToGrab;
            _holdingSomething = true;
        }

        // Drops the currently held object, checks for goal alignment, and repositions it.
        private void Drop()
        {
            
            CheckForSuccessfulDrop();
            
            _objectBeingHeld.SetParent(_objectGroup, true);
            _objectBeingHeld = null;
            _holdingSomething = false;
            
            _dropAudioSource.PlayOneShot(putDownSound);
        }

        // Checks if the object is dropped onto the correct goal and triggers success logic if true.
        private void CheckForSuccessfulDrop()
        {
            if (_isOnGoal && (_possibleSnapObject != null))
            {
                _objectBeingHeld.transform.position = _possibleSnapObject.position;
                Success();
            }
        }
        // Compares two GoalSO references to determine if they match.
        public bool CheckForGoal(GoalSO objectGoal, GoalSO targetGoal)
        {
            return objectGoal != null && targetGoal != null && objectGoal == targetGoal;
        }

        // Plays success sound and logs a success message.
        public override void Success()
        {
            _successAudioSource.PlayOneShot(successSound);
            Debug.Log("success");
        }

        // Updates the internal flag indicating if the object is over a valid goal zone.
        public void SetIsOnGoal(bool isOnGoal)
        {
            _isOnGoal = isOnGoal;
        }

        // Sets the target position to snap the object to if dropped successfully.
        public void SetPossibleSnapObject(Transform possibleSnapObject)
        {
            _possibleSnapObject = possibleSnapObject;
        }

        // Cleans up audio sources to prevent memory leaks or dangling components.
        private void OnDestroy()
        {
            Destroy(_dropAudioSource);
            Destroy(_successAudioSource);
        }
    }
}