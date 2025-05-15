using Code.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scrips.DrawAndCover
{
    public class CoverageManager : MonoBehaviour
    {
        private static readonly int _maskTex = Shader.PropertyToID("_MaskTex");
        private static readonly int _revealTex = Shader.PropertyToID("_RevealTex");
        private static readonly int _baseTex = Shader.PropertyToID("_BaseTex");
        [Header("Grid Settings")] public int gridWidth = 32;
        public int gridHeight = 16;
        public float cellSize = 10f;
        public Vector3 origin = Vector3.zero;

        [Header("Debug")] public bool showGizmos = true;
        private readonly Color _coveredColor = Color.red;
        private readonly Color _uncoveredColor = Color.green;

        [Header("Events")] public RightClickEventSO clickEvent;

        [Header("Textures")] public Texture2D baseTexture; // Grass texture
        public Texture2D revealTexture; // Dirt texture
        public Material coverageMaterial; // ShaderGraph material
        public GameObject plane;
        
        private Texture2D _maskTexture; // Dynamically created in script and updated at runtime
        
        [FormerlySerializedAs("brushShape")] [Header("BrushSettings")]
        public BrushShape brushshape;
        public int brushRadius;

        [Header("Audio")] public AudioClip drawingSound;
        public AudioClip finishingSound;
        private AudioSource _camAudioSource;
        

        [Header("Game Design Settings")] public float successPercentage = 95f;
        private bool _finished;

        private Camera _cam;
        private CoverageGrid _grid;

        private void Start()
        {
            _grid = new CoverageGrid(gridWidth, gridHeight)
            {
                brushshape = brushshape
            };

            // Generate blank mask texture (black = no reveal, white = full reveal)
            _maskTexture = new Texture2D(gridWidth, gridHeight, TextureFormat.RGBA32, false);
            _maskTexture.filterMode = FilterMode.Point;
            _maskTexture.wrapMode = TextureWrapMode.Clamp;

            //Initialize mask to 0 (black = no reveal)
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    _maskTexture.SetPixel(x, y, new Color(0, 0, 0, 1));
                }
            }

            _maskTexture.Apply();

            // Set up material
            coverageMaterial.SetTexture(_baseTex, baseTexture); 
            coverageMaterial.SetTexture(_revealTex, revealTexture); 
            coverageMaterial.SetTexture(_maskTex, _maskTexture); 
            

            // Apply material to plane
            plane.GetComponent<Renderer>().material = coverageMaterial;

            // Set plane scale/position
            float offsetX = (cellSize * gridWidth) / 2;
            float offsetZ = (cellSize * gridHeight) / 2;
            plane.transform.localScale = new Vector3(gridWidth * cellSize / 10f, 1, gridHeight * cellSize / 10f);
            plane.transform.position = new Vector3(origin.x + offsetX, plane.transform.position.y, origin.z + offsetZ);

            // Position camera
            _cam = Camera.main;
            float verticalFOV = _cam.fieldOfView * Mathf.Deg2Rad;
            float aspectRatio = (float)Screen.width / Screen.height;
            float planeWidth = gridWidth * cellSize;
            float cameraHeight = (planeWidth / (2f * aspectRatio)) / Mathf.Tan(verticalFOV / 2f);
            _cam.transform.position = new Vector3(origin.x + offsetX, cameraHeight, origin.z + offsetZ);
            _cam.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            _camAudioSource = _cam.gameObject.AddComponent<AudioSource>();
            _camAudioSource.clip = drawingSound;
        }

        private void OnEnable()
        {
            clickEvent.onInputReceived += HandleClick;
        }

        private void OnDisable()
        {
            clickEvent.onInputReceived -= HandleClick;
        }

        private void Update()
        {
            UpdateMaskTexture();
            if (!_finished && GetCoveragePercentage() > successPercentage)
            {
                //could do with better accuracy
                _finished = true;
                var finishingSource = _cam.gameObject.AddComponent<AudioSource>();
                finishingSource.clip = finishingSound;
                finishingSource.Play();    
                
                
            }
        }

        private void UpdateMaskTexture()
        {
            for (int i = 0; i < gridWidth; i++)
            {
                for (int j = 0; j < gridHeight; j++)
                {
                    bool isCovered = _grid.IsCovered(i, j);
                    float value = isCovered ? 1f : 0f; // white where drawn
                    _maskTexture.SetPixel(gridWidth - 1 - i, gridHeight - 1 - j, new Color(value, value, value, 1));
                }
            }

            _maskTexture.Apply();
        }

        private void HandleClick(Vector3 position)
        {
            Ray ray = _cam.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if(!_camAudioSource.isPlaying)_camAudioSource.Play();
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
            if (IsInBounds(gridPos.x, gridPos.y))
            {
                _grid.MarkCoverage(gridPos.x, gridPos.y,brushRadius);
            }
        }

        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
        }

        private float GetCoveragePercentage()
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
                    Gizmos.color = _grid.IsCovered(x, y) ? _coveredColor : _uncoveredColor;
                    Gizmos.DrawCube(center, new Vector3(cellSize, 0.1f, cellSize));
                }
            }
        }
    }
}