using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class HideInsideSimulation : MonoBehaviour {

    public GameObject ObjectToHide;
    public VideoPlayer Movie;

    private MeshRenderer render;

    // Update is called once per frame
    void Update () {
        render = ObjectToHide.GetComponentInChildren<MeshRenderer>();
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
