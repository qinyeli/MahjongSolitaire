using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    private static LineDrawer instance = null;

    LineRenderer lineRenderer = null;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        DontDestroyOnLoad(gameObject);
    }

    static public void DrawLine(List<Vector3> points)
    {
        instance.lineRenderer.enabled = true;
        instance.lineRenderer.widthMultiplier = 0.2f;
        instance.lineRenderer.positionCount = points.Count;
        instance.lineRenderer.SetPositions(points.ToArray());
    }

    static public void ClearLine()
    {
        instance.lineRenderer.enabled = false;
    }
}
