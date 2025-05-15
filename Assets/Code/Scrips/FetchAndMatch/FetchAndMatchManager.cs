using System;
using Code.Scrips.VisualHelpers;
using Code.ScriptableObjectScripts;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Scrips.FetchAndMatch
{
    
    public class FetchAndMatchManager : MonoBehaviour
    {
        //Can be more elegant but should be though through more
        public InteractEventSO interactEvent;
        public Transform handTransform;
        
        [Header("Visual Effects")] public OutlineMode outlineMode;
        public Color outlineColor = Color.white;
        [Range(0.1f,50f)] //Slider for width
        public float outlineWidth = 2f;
        
        [Header("Sound Effects")] public AudioClip putDownSound;
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
            _dropAudioSource = handTransform.AddComponent<AudioSource>();
            _dropAudioSource.clip = putDownSound;
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
            _dropAudioSource.Play();
            _objectGroup = objectToGrab.parent;
            objectToGrab.SetParent(handTransform, false);
            objectToGrab.transform.localPosition = Vector3.zero;
            _objectBeingHeld = objectToGrab;
            _holdingSomething = true;
        }

        private void Drop()
        {
            _dropAudioSource.Play();
            if (_isOnGoal && (_possibleSnapObject != null))
            {
                _objectBeingHeld.transform.position = _possibleSnapObject.position; //snap to obj
                Success();
                
            }
            _objectBeingHeld.SetParent(_objectGroup, true);
            _objectBeingHeld = null;
            _holdingSomething = false;
            
            
        }


        public bool CheckForGoal(bool isGoal)
        {
            return isGoal;
        }

        private void Success()
        {
            //Add things that happen when u do right
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
        
        
    }
    
    
}