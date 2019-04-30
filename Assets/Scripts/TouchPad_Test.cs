using UnityEngine;
using System.Collections;

public class TouchPad_Test : MonoBehaviour {

    public TouchPad movePad;
    public TouchPad rotPad;

    public float moveSpeed = 10.0f;
    public float rotSpeed = 90.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float moveDelta = this.moveSpeed * Time.deltaTime;
        Vector2 moveConVec = this.movePad.controlVector;

        this.transform.Translate(
            moveConVec.x * moveDelta,
            0.0f,
            moveConVec.y * moveDelta);
        
        float rotDelta = this.rotSpeed * Time.deltaTime;
        Vector2 rotConVec = this.rotPad.controlVector;

        this.transform.Rotate(
            -rotConVec.y * rotDelta,
            rotConVec.x * rotDelta, 0.0f);


	
	}
}
