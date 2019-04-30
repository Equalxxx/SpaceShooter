using UnityEngine;
using System.Collections;

public class Enemy_AnimControl : MonoBehaviour {

    public bool moveFoward = false;
    private Animator myAnimator;
	// Use this for initialization
	void Awake () {
        myAnimator = this.transform.GetChild(0).GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
         
        myAnimator.SetBool("Move", moveFoward);

	}
}
