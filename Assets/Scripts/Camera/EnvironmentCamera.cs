using UnityEngine;
using System.Collections;

public class EnvironmentCamera : MonoBehaviour {

    private Camera mainCam;
	// Use this for initialization
	void Start () {
        mainCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.rotation = mainCam.transform.rotation;
	}
}
