using UnityEngine;
using System.Collections;

public class FollowPos : MonoBehaviour {

    private Transform camTrans;

	// Use this for initialization
	void Awake () {
        camTrans = GameObject.FindGameObjectWithTag("Player").transform.Find("CameraPos");
	}
	
	// Update is called once per frame
	void Update () {
        //if (camTrans.gameObject.activeSelf==true)
        //{
            this.transform.position = camTrans.position;
            this.transform.rotation = camTrans.rotation;
        //}
	}
}
