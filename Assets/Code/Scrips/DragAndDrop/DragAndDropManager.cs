using System.Collections.Generic;
using System.Linq;
using Code.Scrips.Abstractions;
using Code.Scrips.VisualHelpers;
using Code.ScriptableObjectScripts;
using Editors;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Scrips.DragAndDrop
{
    public class DragAndDropManager : ManagerBase
    {
        [Header("Drag and Drop Settings")] public DragEventSO dragEvent;
        public float snapRadius = 2.0f; // Max distance for snapping
        public Transform[] snappableObjects;
        private Dictionary<Transform, GameObject> _occupiedSnapPoints = new Dictionary<Transform, GameObject>();
        
        private Vector3 _originPosition;
        
        [Header("Visuals")] public OutlineMode outlineMode;
        [ShowIfEnum("outlineMode", OutlineMode.ON_HOVER, OutlineMode.ALWAYS)]
        public Color outlineColor = Color.white;
        
        [ShowIfEnum("outlineMode", OutlineMode.ON_HOVER, OutlineMode.ALWAYS)]
        [Range(0.1f, 50f)] public float outlineWidth = 2f;

        [Header("Sounds")] public AudioClip putDownSound;
        public AudioClip movingSound;

        private AudioSource _putDownAudioSource;
        private AudioSource _movingAudioSource;
        private Camera _cam;

        void Start()
        {
            AudioSetup();
            OutlineSetup();
        }

        private void OutlineSetup()
        {
            foreach (var snappable in snappableObjects)
            {
                VisualSetupHelper.AddOutlineComponents(snappable, outlineMode, outlineColor, outlineWidth);
            }
        }

        private void AudioSetup()
        {
            _cam = Camera.main;
            _putDownAudioSource = _cam.AddComponent<AudioSource>();
            _movingAudioSource = _cam.AddComponent<AudioSource>();
            _movingAudioSource.clip = movingSound;
        }

        void OnEnable()
        {
            dragEvent.onDragStart += HandleDragStart;
            dragEvent.onDragUpdate += HandleDragUpdate;
            dragEvent.onDragEnd += HandleDragEnd;
        }

        void OnDisable()
        {
            dragEvent.onDragStart -= HandleDragStart;
            dragEvent.onDragUpdate -= HandleDragUpdate;
            dragEvent.onDragEnd -= HandleDragEnd;
        }

        private void HandleDragStart(GameObject obj)
        {
            _originPosition = obj.transform.position;

            // Unoccupy the snap point if the object is currently on one
            var occupied = _occupiedSnapPoints.FirstOrDefault(kvp => kvp.Value == obj);
            if (occupied.Key != null)
            {
                _occupiedSnapPoints.Remove(occupied.Key);
            }
        }

        private void HandleDragUpdate(GameObject obj, Vector3 position)
        {
            Ray ray = _cam.ScreenPointToRay(position);
            Physics.Raycast(ray, out RaycastHit hit);

            obj.transform.position = new Vector3(hit.point.x, obj.transform.position.y, hit.point.z);
            if (!_movingAudioSource.isPlaying) _movingAudioSource.Play();
        }


        private void HandleDragEnd(GameObject obj)
        {
            Transform nearestPoint = GetNearestSnapPoint(obj.transform.position);

            if (nearestPoint != null &&
                Vector3.Distance(nearestPoint.position, obj.transform.position) < snapRadius &&
                !_occupiedSnapPoints.ContainsKey(nearestPoint))
            {
                obj.transform.position = nearestPoint.position;
                _occupiedSnapPoints[nearestPoint] = obj; // Mark as occupied
                _putDownAudioSource.PlayOneShot(putDownSound);
            }
            else
            {
                obj.transform.position = _originPosition;
            }
        }

        private Transform GetNearestSnapPoint(Vector3 position)
        {
            return snappableObjects
                .OrderBy(point => Vector3.Distance(position, point.position))
                .FirstOrDefault(point => Vector3.Distance(position, point.position) < snapRadius);
        }


        public override void Success()
        {
            Debug.Log("Success!");
        }

        private void OnDestroy()
        {
            foreach (var snappable in snappableObjects)
            {
                Destroy(_movingAudioSource);
                Destroy(_putDownAudioSource);
            }
        }
    }
}