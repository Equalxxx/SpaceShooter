using UnityEngine;
using System.Collections;

public class targetTest : MonoBehaviour {

    public int targets = 3;
    public int lasers = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            for (int i = 0; i < targets; i++)
            {
                int sliceNum = lasers;
                sliceNum -= targets+i;
                Debug.Log(sliceNum.ToString());
            }
        }
	}
}
