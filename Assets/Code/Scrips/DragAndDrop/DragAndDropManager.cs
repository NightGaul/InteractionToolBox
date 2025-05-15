using System.Linq;
using Code.Scrips.VisualHelpers;
using Code.ScriptableObjectScripts;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Scrips.DragAndDrop
{
    public class DragAndDropManager : MonoBehaviour
    {
        

        [Header("Drag and Drop Settings")] public DragEventSO dragEvent;
        public float snapRadius = 2.0f; // Max distance for snapping
        public Transform[] snapPoints;
        private Vector3 _originPosition;

        [Header("Sound Effects")] public AudioClip putDownSound;
        public AudioClip movingSound;

        [Header("Visual Effects")] public OutlineMode outlineMode;
        public Color outlineColor = Color.white;
        [Range(0.1f,50f)] //Slider for width
        public float outlineWidth = 2f;


        //To Do: Find out what you need from quick outline,
        
        void Start()
        {
            //for convenience is here, because of the tag dependency but might change it to manual-adding 
            snapPoints = GameObject.FindGameObjectsWithTag("SnapPoint")
                .Select(obj => obj.transform)
                .ToArray();
            GameObject[] draggables = GameObject.FindGameObjectsWithTag("Draggable").ToArray();

            foreach (var draggable in draggables)
            {
                //It's better to use two for once and continuous sounds but gets complicated quickly so
                //I decided to leave it for this until it shoots me in the leg
                draggable.AddComponent<AudioSource>();
                
                //Add modes for more Outline Modes
                var outline = draggable.AddComponent<Outline>();
                outline.OutlineColor = outlineColor;
                outline.OutlineWidth = outlineWidth;
                switch (outlineMode)
                {
                    case (OutlineMode.ALWAYS):
                        draggable.AddComponent<OutlineAlways>();
                        break;
                    case (OutlineMode.ON_HOVER):
                        draggable.AddComponent<OutlineOnHover>();
                        break;
                }
            }

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
        }

        private void HandleDragUpdate(GameObject obj, Vector3 position)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            Physics.Raycast(ray, out RaycastHit hit);

            obj.transform.position = new Vector3(hit.point.x, obj.transform.position.y, hit.point.z);
            PlaySoundContinuously(obj, movingSound);
        }


        private void HandleDragEnd(GameObject obj)
        {
            Transform nearestPoint = GetNearestSnapPoint(obj.transform.position);

            if (nearestPoint != null && (Vector3.Distance(nearestPoint.position, obj.transform.position) < snapRadius))
            {
                obj.transform.position = nearestPoint.position;
                StopPreviousAudioSource(obj);
                PlaySoundOnce(obj, putDownSound);
            }
            else
            {
                StopPreviousAudioSource(obj);
                obj.transform.position = _originPosition;
            }
        }

        private Transform GetNearestSnapPoint(Vector3 position)
        {
            return snapPoints
                .OrderBy(point => Vector3.Distance(position, point.position))
                .FirstOrDefault(point => Vector3.Distance(position, point.position) < snapRadius);
        }

        private void PlaySoundOnce(GameObject obj, AudioClip clip)
        {
            var audioSource = obj.GetComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
        }

        private void PlaySoundContinuously(GameObject obj, AudioClip clip)
        {
            var audioSource = obj.GetComponent<AudioSource>();
            audioSource.clip = clip;
            if (!audioSource.isPlaying) audioSource.Play();
        }

        private void StopPreviousAudioSource(GameObject obj)
        {
            var audioSource = obj.GetComponent<AudioSource>();
            if (audioSource.isPlaying) audioSource.Stop();
        }
    }
}