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

        [Space] [Tooltip("Tolerance used for determining snapping")]
        public float snapTolerance = 2.0f;

        [Tooltip("Objects that act as snapping-points for the dragged object")]
        public Transform[] snappableObjects;

        private readonly Dictionary<Transform, GameObject> _occupiedSnapPoints = new Dictionary<Transform, GameObject>();
        private Vector3 _originPosition;

        [Header("Visuals")] public OutlineMode outlineMode;

        [ShowIfEnum("outlineMode", OutlineMode.ON_HOVER, OutlineMode.ALWAYS)]
        public Color outlineColor = Color.white;

        [ShowIfEnum("outlineMode", OutlineMode.ON_HOVER, OutlineMode.ALWAYS)] [Range(0.1f, 50f)]
        public float outlineWidth = 2f;

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

        
        // Adds outline visual effects to all snappable objects based on the selected outline mode and settings.
        private void OutlineSetup()
        {
            foreach (var snappable in snappableObjects)
            {
                VisualSetupHelper.AddOutlineComponents(snappable, outlineMode, outlineColor, outlineWidth);
            }
        }

        // Sets up the audio sources for drag movement and drop actions using the main camera.
        private void AudioSetup()
        {
            _cam = Camera.main;
            _putDownAudioSource = _cam.AddComponent<AudioSource>();
            _movingAudioSource = _cam.AddComponent<AudioSource>();
            _movingAudioSource.clip = movingSound;
        }

        // Subscribes to drag-related events (start, update, end) when the manager is enabled.
        void OnEnable()
        {
            dragEvent.onDragStart += HandleDragStart;
            dragEvent.onDragUpdate += HandleDragUpdate;
            dragEvent.onDragEnd += HandleDragEnd;
        }

        // Unsubscribes from drag events when the manager is disabled to avoid unintended behavior or memory leaks.
        void OnDisable()
        {
            dragEvent.onDragStart -= HandleDragStart;
            dragEvent.onDragUpdate -= HandleDragUpdate;
            dragEvent.onDragEnd -= HandleDragEnd;
        }

        // Records the original position of the dragged object and clears any previous snap point assignment.
        private void HandleDragStart(GameObject obj)
        {
            _originPosition = obj.transform.position;
            
            var occupied = _occupiedSnapPoints.FirstOrDefault(kvp => kvp.Value == obj);
            if (occupied.Key != null)
            {
                _occupiedSnapPoints.Remove(occupied.Key);
            }
        }

        // Continuously updates the object's position to follow the mouse and plays a movement sound if needed.
        private void HandleDragUpdate(GameObject obj, Vector3 position)
        {
            Ray ray = _cam.ScreenPointToRay(position);
            Physics.Raycast(ray, out RaycastHit hit);

            obj.transform.position = new Vector3(hit.point.x, obj.transform.position.y, hit.point.z);
            if (!_movingAudioSource.isPlaying) _movingAudioSource.Play();
        }


        // Attempts to snap the object to the nearest available snap point or returns it to its original position if snapping fails.
        private void HandleDragEnd(GameObject obj)
        {
            Transform nearestPoint = GetNearestSnapPoint(obj.transform.position);

            if (nearestPoint != null &&
                Vector3.Distance(nearestPoint.position, obj.transform.position) < snapTolerance &&
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

        // Finds the closest snap point within the tolerance range that isn't already occupied.
        private Transform GetNearestSnapPoint(Vector3 position)
        {
            return snappableObjects
                .OrderBy(point => Vector3.Distance(position, point.position))
                .FirstOrDefault(point => Vector3.Distance(position, point.position) < snapTolerance);
        }


        // Placeholder success handler – currently just logs success to the console.
        public override void Success()
        {
            Debug.Log("Success!");
        }

        // Cleans up audio sources when the manager is destroyed to prevent memory leaks.
        private void OnDestroy()
        {
            Destroy(_movingAudioSource);
            Destroy(_putDownAudioSource);
        }
    }
}