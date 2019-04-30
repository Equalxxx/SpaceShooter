using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public float liveTime = 3.0f;
    public float moveSpeed = 100.0f;
    private Transform myTrans;
	// Use this for initialization
	void Start () {
        myTrans = this.transform;
        Destroy(this.gameObject, liveTime);
	}
	
	// Update is called once per frame
	void Update () {
        this.myTrans.Translate(this.myTrans.forward * moveSpeed * Time.deltaTime,Space.World);
	}
}
