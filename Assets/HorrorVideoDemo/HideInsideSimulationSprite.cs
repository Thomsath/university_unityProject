using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class HideInsideSimulationSprite : MonoBehaviour {

    public SpriteRenderer render;
    public VideoPlayer Movie;

    // Update is called once per frame
    void Update()
    {
        if (Movie.isPlaying)
        {
            render.enabled = false;
        }
        else
        {
            render.enabled = true;
        }
    }
}
