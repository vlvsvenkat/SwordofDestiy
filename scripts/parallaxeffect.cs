using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallaxeffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;
    // Start is called before the first frame update
    Vector2 startingPosition;

    float startingZ;
    Vector2 camMovieSinceStart =>(Vector2)cam.transform.position - startingPosition;
    float distanceFromTarget => transform.position.z - followTarget.transform.position.z;
    float clippingPlane => (cam.transform.position.z + (distanceFromTarget > 6? cam.farClipPlane: cam.nearClipPlane));
    float parallaxFactor => Mathf.Abs(distanceFromTarget) / clippingPlane;
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = startingPosition + camMovieSinceStart * parallaxFactor;
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
        
    }
}
