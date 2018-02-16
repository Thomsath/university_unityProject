using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class HideAfterSimulation : MonoBehaviour {

    public GameObject ObjectToHide;
    public VideoPlayer Movie;

    private MeshRenderer render;

    // Update is called once per frame
    void Update()
    {
        render = ObjectToHide.GetComponentInChildren<MeshRenderer>();

        if (Movie.isPlaying)
        {
            render.enabled = true;
        }
        else
        {
            render.enabled = false;
        }
    }
}
