using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector3 startPosition;

    public void Start()
    {
        startPosition = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y + 100f, transform.position.z);
    }

    public void Appear()
    {
        StartCoroutine(SlideDown());
    }

    private IEnumerator SlideDown()
    {
        while(Vector3.Distance(transform.position, startPosition) >  Vector3.kEpsilon)
        {
            yield return null;
            transform.position = Vector3.Lerp(transform.position, startPosition, 5f * Time.deltaTime);
        }
    }
}
