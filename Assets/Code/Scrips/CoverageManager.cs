using Code.ScriptableObjects;
using UnityEngine;

namespace Code.Scrips
{
    public class CoverageManager : MonoBehaviour
    {
        [Header("Grid Settings")] public int gridWidth = 32;
        public int gridHeight = 16;
        public float cellSize = 1f;
        public Vector3 origin = Vector3.zero;

        [Header("Debug")] public Color coveredColor = Color.red;
        public Color uncoveredColor = Color.green;
        public bool showGizmos = true;

        [Header("Events")] public InputEventSO clickEvent;

        private CoverageGrid _grid;

        private void Awake()
        {
            _grid = new CoverageGrid(gridWidth, gridHeight);
        }

        void OnEnable()
        {
            clickEvent.onInputReceived += HandleClick;
        }

        void OnDisable()
        {
            clickEvent.onInputReceived -= HandleClick;
        }

        private void HandleClick(Vector3 position)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("i can click");
                Debug.Log(position);
                MarkAtWorldPosition(hit.point);
            }
            
        }
        
        
        private Vector2Int WorldToGrid(Vector3 worldPosition)
        {
            Vector3 local = worldPosition - origin;
            int x = Mathf.FloorToInt(local.x / cellSize);
            int y = Mathf.FloorToInt(local.z / cellSize); 
            return new Vector2Int(x, y);
        }

        private Vector3 GridToWorld(Vector2Int gridPosition)
        {
            return origin + new Vector3(gridPosition.x * cellSize, 0, gridPosition.y * cellSize);
        }

        private void MarkAtWorldPosition(Vector3 worldPos)
        {
            Vector2Int gridPos = WorldToGrid(worldPos);
            Debug.Log($"Click at worldPos: {worldPos}, gridPos: {gridPos}");

            
                _grid.MarkCoverage(gridPos.x, gridPos.y);
            
        }

        public float GetCoveragePercentage()
        {
            return _grid.GetCoveragePercent();
        }

        private void OnDrawGizmos()
        {
            if (!showGizmos || _grid == null) return;

            for (int x = 0; x < _grid.GetGridCoverage().GetLength(0); x++)
            {
                for (int y = 0; y < _grid.GetGridCoverage().GetLength(1); y++)
                {
                    Vector3 center = GridToWorld(new Vector2Int(x, y)) + new Vector3(cellSize / 2f, 0f, cellSize / 2f);
                    Gizmos.color = _grid.IsCovered(x, y) ? coveredColor : uncoveredColor;
                    Gizmos.DrawCube(center, new Vector3(cellSize, 0.1f, cellSize));
                }
            }
        }
        
       
        
        
    }
}