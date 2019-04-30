using UnityEngine;
using System.Collections;

public class FlareView : MonoBehaviour {

    private Camera mainCam;
    public float dist;
    private LensFlare myFlare;
	// Use this for initialization
	void Start () {
        mainCam = Camera.main;
        myFlare = this.GetComponent<LensFlare>();
	}
	
	// Update is called once per frame
	void Update () {
        dist = Vector3.Distance(this.transform.position, mainCam.transform.position);

        if (dist > 100)
        {
            myFlare.enabled = true;
        }
        else
            myFlare.enabled = false;
	}
}
