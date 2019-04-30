using UnityEngine;
using System.Collections;

public class PressTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //
    // NGUI Event
    //

    //Click 시호출됨....
    void OnClick()
    {
        print("OnClick");
    }


    void OnPress( bool bDown )
    {
        if (bDown)
            print("눌렀다");

        else
            print("뗏다");
   
    }
}
