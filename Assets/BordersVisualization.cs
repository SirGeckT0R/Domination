using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

[RequireComponent(typeof(PolyShape))]
public class BordersVisualization : MonoBehaviour
{
    public Color boundsColor = Color.red;
    public float lineWidth = 0.05f;

    public Texture dashTexture;
    public float dashSize = 0.1f;
    public float gapSize = 0.1f;

    private LineRenderer lineRenderer;
    private PolyShape polyshape;

    void Start()
    {
        polyshape = GetComponent<PolyShape>();
        lineRenderer = GetComponent<LineRenderer>();

        Material lineMaterial = new Material(Shader.Find("Unlit/Transparent")) { color = boundsColor };
        lineMaterial.mainTexture = dashTexture;
        lineRenderer.material = lineMaterial;

        lineRenderer.material.mainTextureScale = new Vector2(1f / (dashSize + gapSize), 1);
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;

        UpdateBoundsVisual();
    }

    void UpdateBoundsVisual()
    {
        lineRenderer.positionCount = polyshape.controlPoints.Count;
        lineRenderer.SetPositions(polyshape.controlPoints.ToArray());
        lineRenderer.material.mainTextureScale = new Vector2(1f / (dashSize + gapSize), 1);
    }

    private void Update()
    {
        UpdateBoundsVisual();
    }
}
