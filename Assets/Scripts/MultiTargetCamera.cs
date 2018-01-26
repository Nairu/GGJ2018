using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTargetCamera : MonoBehaviour {
    
    public enum Projection
    {
        Orthographic,
        Perspective
    }

    public List<Transform> targets;
    public Vector3 offset;
    public float minZoom = 40;
    public float maxZoom = 10;
    public float zoomLimiter = 50;
    public Projection projection;
    public float zoomSpeed = 1.5f;

    private Vector3 velocity;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        Bounds b = GetPlayerBounds();
        Move(b);
        Zoom(b);
    }

    void Move(Bounds playerBounds)
    {
        Vector3 newPosition = playerBounds.center + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, Time.deltaTime);
    }

    void Zoom(Bounds playerBounds)
    {
        switch (projection)
        {
            case Projection.Orthographic:
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, Mathf.Min(Mathf.Max(playerBounds.size.magnitude, minZoom), maxZoom), Time.deltaTime);
                break;
            case Projection.Perspective:
                float zoom = Mathf.Lerp(maxZoom, minZoom, playerBounds.size.magnitude / zoomLimiter);
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoom, Time.deltaTime * zoomSpeed);
                break;
        }
    }

    Bounds GetPlayerBounds()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        foreach (Transform t in targets)
        {
            bounds.Encapsulate(t.position);
        }
        return bounds;
    }
}
