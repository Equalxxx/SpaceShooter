using UnityEngine;
using System.Collections;

public class UITest : MonoBehaviour {

    public bool btnDown = false;
    public UIButton btn;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void PressBtn()
    {
        btnDown = true;
        Debug.Log("press");
    }
    public void EndBtn()
    {
        btnDown = false;
        Debug.Log("end");
    }
}
