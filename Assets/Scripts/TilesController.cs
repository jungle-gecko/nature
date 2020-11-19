using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesController : MonoBehaviour
{
	public GameObject[] tiles;
	
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SlideDown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator SlideDown()
    {
        foreach (var tile in tiles)
        {
            yield return new WaitForSeconds(.2f);
            tile.GetComponent<Tile>().Appear();
        }
    }

}
