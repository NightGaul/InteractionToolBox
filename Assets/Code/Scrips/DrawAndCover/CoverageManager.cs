using System;
using Code.Scrips.Abstractions;
using Code.Scrips.AudioHelpers;
using Code.Scrips.UI;
using Code.ScriptableObjectScripts;
using UnityEngine;

namespace Code.Scrips.DrawAndCover
{
    public class CoverageManager : ManagerBase
    {
        private static readonly int _maskTex = Shader.PropertyToID("_MaskTex");
        private static readonly int _revealTex = Shader.PropertyToID("_RevealTex");
        private static readonly int _baseTex = Shader.PropertyToID("_BaseTex");

        [Header("Grid Settings")] public float cellSize = 10f; // size of each grid cell (world units)
        private int _gridWidth;
        private int _gridHeight;

        [Header("Events")] public RightClickEventSO clickEvent;

        [Header("Visuals")] public Texture2D baseTexture;
        public Texture2D revealTexture;
        public Material coverageMaterial; // ShaderGraph material
        public GameObject plane;

        private Texture2D _maskTexture; // Dynamic mask updated at runtime
        private MeshFilter _meshFilter;
        private CoverageGrid _grid;

        [Header("Sounds")] public AudioClip drawingSound;
        public AudioClip finishingSound;
        private AudioSource _camAudioSource;
        private AudioFader _audioFader;

        [Header("Drawing Settings")] public float successPercentage = 95f;
        [Space] public BrushShape brushShape;
        public int brushRadius;
        public Sprite cursorSprite;
        public bool useCustomCursor;

        private bool _finished;
        private Camera _cam;

        private void Start()
        {
            if (!ValidatePlane()) return;

            CalculateGridDimensions();
            if (!ValidateGridSize()) return;

            InitializeGrid();
            CreateMaskTexture();
            ApplyMaterialTextures();
            AssignMaterialToPlane();

            SetupCameraAndAudio();

            if (useCustomCursor) CustomCursor.instance.SetCursor(cursorSprite);
        }

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

        private void CalculateGridDimensions()
        {
            Bounds meshBounds = _meshFilter.sharedMesh.bounds;
            Vector3 planeSize = Vector3.Scale(meshBounds.size, plane.transform.localScale);

            plane.transform.TransformPoint(meshBounds.min);

            _gridWidth = Mathf.FloorToInt(planeSize.x / cellSize);
            _gridHeight = Mathf.FloorToInt(planeSize.z / cellSize);

            // Debug.Log($"Grid Width: {_gridWidth}, Grid Height: {_gridHeight}");
        }

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

        private void InitializeGrid()
        {
            _grid = new CoverageGrid(_gridWidth, _gridHeight)
            {
                brushshape = brushShape
            };
        }

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

        private void ApplyMaterialTextures()
        {
            coverageMaterial.SetTexture(_baseTex, baseTexture);
            coverageMaterial.SetTexture(_revealTex, revealTexture);
            coverageMaterial.SetTexture(_maskTex, _maskTexture);
        }

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

        private void SetupCameraAndAudio()
        {
            _cam = Camera.main;
            if (_cam != null) _camAudioSource = _cam.gameObject.AddComponent<AudioSource>();
            _audioFader = gameObject.AddComponent<AudioFader>();
            _audioFader.SetAudioSource(_camAudioSource);
            _camAudioSource.clip = drawingSound;
        }


        private void OnEnable()
        {
            clickEvent.onInputReceived += HandleClick;
            clickEvent.onInputStop += StopClick;
        }

        private void OnDisable()
        {
            clickEvent.onInputReceived -= HandleClick;
            clickEvent.onInputStop -= StopClick;
        }

        private void Update()
        {
            UpdateMaskTexture();

            if (!_finished && GetCoveragePercentage() > successPercentage)
            {
                Success();
            }
            Debug.Log(GetCoveragePercentage());
        }

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

        private void HandleClick(Vector3 position)
        {
            Ray ray = _cam.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (!_camAudioSource.isPlaying) _camAudioSource.Play();
                MarkAtWorldPosition(hit.point);
            }
        }

        private void StopClick()
        {
            _audioFader.StartFadeOut();
        }

        // Convert world position to grid coordinate based on plane local space & mesh bounds
        private Vector2Int WorldToGrid(Vector3 worldPosition)
        {
            Vector3 localPos = plane.transform.InverseTransformPoint(worldPosition);
            Vector3 meshMin = _meshFilter.sharedMesh.bounds.min;

            int x = Mathf.FloorToInt((localPos.x - meshMin.x) / cellSize);
            int y = Mathf.FloorToInt((localPos.z - meshMin.z) / cellSize);
            return new Vector2Int(x, y);
        }

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
                _grid.MarkCoverage(gridPos.x, gridPos.y, brushRadius);
            }
        }

        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < _gridWidth && y >= 0 && y < _gridHeight;
        }

        private float GetCoveragePercentage()
        {
            return _grid.GetCoveragePercent();
        }

        public override void Success()
        {
            _finished = true;
            var finishingSource = _cam.gameObject.AddComponent<AudioSource>();
            finishingSource.clip = finishingSound;
            finishingSource.Play();
        }

        private void OnDestroy()
        {
            Destroy(_camAudioSource);
        }
    }
}