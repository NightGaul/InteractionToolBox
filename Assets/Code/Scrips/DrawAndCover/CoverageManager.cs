using Code.Scrips.Abstractions;
using Code.Scrips.AudioHelpers;
using Code.Scrips.UI;
using Code.ScriptableObjectScripts;
using Editors;
using UnityEngine;

namespace Code.Scrips.DrawAndCover
{
    public class CoverageManager : ManagerBase
    {
        private static readonly int _maskTex = Shader.PropertyToID("_MaskTex");
        private static readonly int _revealTex = Shader.PropertyToID("_RevealTex");
        private static readonly int _baseTex = Shader.PropertyToID("_BaseTex");

        [Header("Draw and Cover Settings")] public RightClickEventSO clickEvent;

        [Space] [Tooltip("Size of pixels in grid")]
        public float cellSize = 10f;

        [Tooltip("Percent of coverage that is needed to trigger success")]
        public float successPercentage = 95f;

        private int _gridWidth;
        private int _gridHeight;


        [Header("Brush Settings")]public BrushShape brushShape;
        public int brushSize = 1;
        public bool useCustomCursor;
        [ShowIfBool("useCustomCursor")] public Sprite cursorSprite;

        [Header("Visuals")] public Texture2D baseTexture;
        public Texture2D revealTexture;
        public Material coverageMaterial; // ShaderGraph material
        public GameObject plane;

        private Texture2D _maskTexture; // Dynamic mask updated at runtime
        private MeshFilter _meshFilter;
        private CoverageGrid _grid;

        [Header("Sounds")] public AudioClip drawingSound;
        private AudioSource _finishingSource;
        public AudioClip finishingSound;
        private AudioSource _camAudioSource;
        private AudioFader _audioFader;

        private bool _finished;
        private Camera _cam;

        private void Start()
        {
            _cam = Camera.main;

            if (!ValidatePlane()) return;

            CalculateGridDimensions();

            if (!ValidateGridSize()) return;

            GridSetup();
            CreateMaskTexture();
            ApplyMaterialTextures();
            AssignMaterialToPlane();

            AudioSetup();

            if (useCustomCursor) CustomCursor.instance.SetCursor(cursorSprite);
        }

        private void Update()
        {
            //For further development consider dirty-flag
            UpdateMaskTexture();

            if (!_finished && GetCoveragePercentage() > successPercentage)
            {
                Success();
            }

            Debug.Log(GetCoveragePercentage());
        }

        // Subscribes to right-click input events to handle drawing.
        private void OnEnable()
        {
            clickEvent.onInputReceived += HandleClick;
            clickEvent.onInputStop += StopClick;
        }

        // Unsubscribes from input events to avoid memory leaks or null references.
        private void OnDisable()
        {
            clickEvent.onInputReceived -= HandleClick;
            clickEvent.onInputStop -= StopClick;
        }

        // Verifies the plane GameObject and mesh are properly assigned and usable.
        private bool ValidatePlane()
        {
            if (plane == null)
            {
                Debug.LogError("CoverageManager: Plane GameObject is not assigned.");
                return false;
            }

            _meshFilter = plane.GetComponent<MeshFilter>();
            if (_meshFilter == null || _meshFilter.sharedMesh == null)
            {
                Debug.LogError("CoverageManager: Plane must have a MeshFilter with a valid mesh.");
                return false;
            }

            return true;
        }

        // Calculates the number of grid cells based on the plane's size and configured cell size.
        private void CalculateGridDimensions()
        {
            Bounds meshBounds = _meshFilter.sharedMesh.bounds;
            Vector3 planeSize = Vector3.Scale(meshBounds.size, plane.transform.localScale);

            plane.transform.TransformPoint(meshBounds.min);

            _gridWidth = Mathf.FloorToInt(planeSize.x / cellSize);
            _gridHeight = Mathf.FloorToInt(planeSize.z / cellSize);
        }

        // Checks if the calculated grid dimensions are valid (greater than zero).
        private bool ValidateGridSize()
        {
            if (_gridWidth <= 0 || _gridHeight <= 0)
            {
                Debug.LogError(
                    $"CoverageManager: Invalid grid size calculated (Width: {_gridWidth}, Height: {_gridHeight}). Check cellSize and plane scale.");
                return false;
            }

            return true;
        }

        // Initializes the grid and applies the selected brush shape for marking coverage.
        private void GridSetup()
        {
            _grid = new CoverageGrid(_gridWidth, _gridHeight)
            {
                brushShape = brushShape
            };
        }

        // Creates a blank mask texture used to track and display the player's coverage.
        private void CreateMaskTexture()
        {
            _maskTexture = new Texture2D(_gridWidth, _gridHeight, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            for (int x = 0; x < _gridWidth; x++)
            {
                for (int y = 0; y < _gridHeight; y++)
                {
                    _maskTexture.SetPixel(x, y, new Color(0, 0, 0, 1));
                }
            }

            _maskTexture.Apply();
        }

        // Assigns the base, reveal, and mask textures to the shader material for rendering.
        private void ApplyMaterialTextures()
        {
            coverageMaterial.SetTexture(_baseTex, baseTexture);
            coverageMaterial.SetTexture(_revealTex, revealTexture);
            coverageMaterial.SetTexture(_maskTex, _maskTexture);
        }

        // Applies the configured material to the plane's renderer for visual feedback.
        private void AssignMaterialToPlane()
        {
            Renderer planeRenderer = plane.GetComponent<Renderer>();
            if (planeRenderer == null)
            {
                Debug.LogError("CoverageManager: Plane GameObject has no Renderer component.");
                return;
            }

            planeRenderer.material = coverageMaterial;
        }

        // Sets up the audio components for drawing sounds and fade control.
        private void AudioSetup()
        {
            if (_cam != null) _camAudioSource = _cam.gameObject.AddComponent<AudioSource>();
            _audioFader = gameObject.AddComponent<AudioFader>();
            _audioFader.SetAudioSource(_camAudioSource);
            _camAudioSource.clip = drawingSound;
        }


        // Updates the visual representation of the coverage mask based on the current grid state.
        private void UpdateMaskTexture()
        {
            for (int i = 0; i < _gridWidth; i++)
            {
                for (int j = 0; j < _gridHeight; j++)
                {
                    bool isCovered = _grid.IsCovered(i, j);
                    float value = isCovered ? 1f : 0f; // white where drawn
                    _maskTexture.SetPixel(_gridWidth - 1 - i, _gridHeight - 1 - j, new Color(value, value, value, 1));
                }
            }

            _maskTexture.Apply();
        }

        // Converts a mouse click to a grid position and marks the corresponding area as covered.
        private void HandleClick(Vector3 position)
        {
            Ray ray = _cam.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                MarkAtWorldPosition(hit.point);
            }
        }

        // Fades out the drawing sound when the player stops clicking.
        private void StopClick()
        {
            _audioFader.StartFadeOut();
        }

        // Converts a world-space position into a corresponding grid cell coordinate.
        private Vector2Int WorldToGrid(Vector3 worldPosition)
        {
            Vector3 localPos = plane.transform.InverseTransformPoint(worldPosition);
            Vector3 meshMin = _meshFilter.sharedMesh.bounds.min;

            int x = Mathf.FloorToInt((localPos.x - meshMin.x) / cellSize);
            int y = Mathf.FloorToInt((localPos.z - meshMin.z) / cellSize);
            return new Vector2Int(x, y);
        }

        // Marks the coverage grid at the position where the player clicked, using the brush size.
        private void MarkAtWorldPosition(Vector3 worldPos)
        {
            if (_grid == null)
            {
                Debug.LogWarning("CoverageManager: Grid not initialized.");
                return;
            }

            Vector2Int gridPos = WorldToGrid(worldPos);
            if (IsInBounds(gridPos.x, gridPos.y))
            {
                if (!_camAudioSource.isPlaying) _camAudioSource.Play();
                _grid.MarkCoverage(gridPos.x, gridPos.y, brushSize);
            }
        }

        // Checks whether a given grid coordinate is within the bounds of the grid.
        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < _gridWidth && y >= 0 && y < _gridHeight;
        }

        // Returns the percentage of the grid that has been covered so far.
        private float GetCoveragePercentage()
        {
            return _grid.GetCoveragePercent();
        }

        // Called when the required coverage is reached; plays finishing sound and prevents further drawing.
        public override void Success()
        {
            _finished = true;
            _finishingSource = _cam.gameObject.AddComponent<AudioSource>();
            _finishingSource.clip = finishingSound;
            _finishingSource.Play();
        }

        // Cleans up audio resources to prevent memory leaks when the object is destroyed.
        private void OnDestroy()
        {
            Destroy(_camAudioSource);
            Destroy(_finishingSource);
        }
    }
}