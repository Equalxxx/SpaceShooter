using UnityEngine;
using System.Collections;

public class RotateCube : MonoBehaviour {
    private float angle = 45.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(0.0f, angle*Time.deltaTime, 0.0f,Space.World);
	}
}
