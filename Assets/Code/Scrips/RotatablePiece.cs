using Code.ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scrips
{
    [RequireComponent(typeof(BoxCollider))]
    public class RotatablePiece : MonoBehaviour
    {
        [Header("Rotation Settings")]
        
        public float rotationSpeed = 200f;
        public float targetAngle = 0f;    // Desired final angle in degrees
        public float tolerance = 5f;      // How close it needs to be to snap
        public bool isSolved = false;

        public ScrollEventSO scrollEvent;
        private bool _isMouseOver = false;
        private bool _snapped = false;

        //To be added:
        //Choose angle of rotation
        //Snapping
        //Desired Rotation
        //random rotation in the beginning maybe
        private void Start()
        {
            scrollEvent.onScroll += Scroll;
        }

        private void OnDisable()
        {
            scrollEvent.onScroll -= Scroll;
        }
        void Update()
        {
            //if (isSolved) return; // Don't allow further rotation after solving

            

            //CheckSolved();
        }

        public void Rotate(float input)
        {
            transform.Rotate(0, -input * rotationSpeed * Time.deltaTime,0);
        }

        public bool CheckSolved()
        {
            float currentAngle = transform.eulerAngles.z;
            float difference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));

            if (difference < tolerance && !_snapped)
            {
                SnapToTarget();
                return true;
            }

            isSolved = _snapped;
            return isSolved;
        }

        private void SnapToTarget()
        {
            // Instantly set to the exact angle
            transform.rotation = Quaternion.Euler(0, 0, targetAngle);
            _snapped = true;
            isSolved = true;

            // Optional: add feedback like sound or visual effect
            Debug.Log($"{gameObject.name} snapped into position!");
        }

        public void Scroll(float delta)
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
        

        /*private void OnDrawGizmosSelected()
        {
            // Draw a line showing the target rotation direction
            Gizmos.color = Color.green;
            Vector3 dir = Quaternion.Euler(0, 0, targetAngle) * Vector3.up;
            Gizmos.DrawLine(transform.position, transform.position + dir * 0.5f);
        }*/
    }
}