using System.Linq;
using Code.ScriptableObjects;
using UnityEngine;

namespace Code.Scrips
{
    public class DragAndDropManager : MonoBehaviour
    {
        public DragEventSO dragEvent;
        public float snapRadius = 2.0f; // Max distance for snapping
        private Transform[] _snapPoints;
        private Vector3 _originPosition;

        void Start()
        {
            _snapPoints = GameObject.FindGameObjectsWithTag("SnapPoint")
                .Select(obj => obj.transform)
                .ToArray();
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
            Debug.Log("Started dragging: " + obj.name + " | Origin: " + _originPosition);
        }

        private void HandleDragUpdate(GameObject obj, Vector3 position)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            Physics.Raycast(ray, out RaycastHit hit);

            obj.transform.position = new Vector3(hit.point.x, obj.transform.position.y, hit.point.z);
        }


        private void HandleDragEnd(GameObject obj)
        {
            Transform nearestPoint = GetNearestSnapPoint(obj.transform.position);
            if (nearestPoint != null && (Vector3.Distance(nearestPoint.position, obj.transform.position) < snapRadius))
            {
                obj.transform.position = nearestPoint.position;
                Debug.Log("Snapped to: " + nearestPoint.name);
            }
            else
            {
                obj.transform.position = _originPosition;
            }
        }

        private Transform GetNearestSnapPoint(Vector3 position)
        {
            return _snapPoints
                .OrderBy(point => Vector3.Distance(position, point.position))
                .FirstOrDefault(point => Vector3.Distance(position, point.position) < snapRadius);
        }
    }
}