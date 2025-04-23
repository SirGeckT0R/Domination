using Assets.Scripts.Map.Counties;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

[RequireComponent(typeof(LineRenderer))]
public class BordersVisualization : MonoBehaviour
{
    //[SerializeField] private Color _boundsColor = Color.red;
    [SerializeField] private float _lineWidth = 0.05f;

    [SerializeField] private Material _lineMaterial;
    [SerializeField] private float _dashSize = 0.001f;
    [SerializeField] private float _gapSize = 0.01f;

    private LineRenderer _lineRenderer;
    private County _county;
    private PolyShape _polyshape;

    void Start()
    {
        _county = GetComponent<County>();
        _polyshape = GetComponent<PolyShape>();
        _lineRenderer = GetComponent<LineRenderer>();

        ConfigureLineRenderer();

        UpdateBoundsVisual();
    }

    private void ConfigureLineRenderer()
    {
        _lineRenderer.material = _lineMaterial;
        _lineRenderer.material.color = BorderColors.colors[_county.BelongsTo - 1];
        _lineRenderer.material.mainTextureScale = new Vector2(1f / (_dashSize + _gapSize), 1);

        _lineRenderer.startWidth = _lineWidth;
        _lineRenderer.endWidth = _lineWidth;
        _lineRenderer.loop = true;
        _lineRenderer.useWorldSpace = false;
    }

    void UpdateBoundsVisual()
    {
        _lineRenderer.positionCount = _polyshape.controlPoints.Count;
        _lineRenderer.SetPositions(_polyshape.controlPoints.ToArray());
        _lineRenderer.material.mainTextureScale = new Vector2(1f / (_dashSize + _gapSize), 1);
        _lineRenderer.material.color = BorderColors.colors[_county.BelongsTo - 1];
    }

    //DebugOnly
    private void Update()
    {
        UpdateBoundsVisual();
    }
}
