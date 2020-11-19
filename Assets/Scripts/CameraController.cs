using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform cameraTarget;
    private Transform newTarget;
    private Camera cameraComponent;
    public int speed = 10;
    public int defaultZoom = 50;
    public int focusZoom = 20;
    private int targetZoom;

    void Start()
    {
        cameraTarget = transform.parent;
        newTarget = cameraTarget;
        targetZoom = defaultZoom;
        cameraComponent = GetComponent<Camera>();
    }

    void Update()
    {
        if(Vector3.Distance(newTarget.position, cameraTarget.position) >  Vector3.kEpsilon)
        {
            cameraTarget.position = Vector3.Lerp(cameraTarget.position, newTarget.position, Time.deltaTime * speed);
        }
        
        if(Mathf.Abs(cameraComponent.orthographicSize - (int) targetZoom) >  Mathf.Epsilon)
        {
            cameraComponent.orthographicSize = Mathf.Lerp(cameraComponent.orthographicSize, (int) targetZoom, Time.deltaTime * speed);
        }
    }
    
    public void Focus(Transform target)
    {
        newTarget = target;
        targetZoom = focusZoom;
    }

    public void Unfocus()
    {
        targetZoom = defaultZoom;
    }


}
