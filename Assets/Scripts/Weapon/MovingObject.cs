using UnityEngine;
using System.Collections;

public class MovingObject : MonoBehaviour {

	public float delta = 0.0f;
	private bool swap = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (swap) {
			delta+=Time.deltaTime;
			if(delta>1.0f)
			{
				delta=1.0f;
				swap=false;
			}
		} else {
			delta-=Time.deltaTime;
			if(delta<-1.0f)
			{
				delta=-1.0f;
				swap=true;
			}
		}

		this.transform.Translate (delta, 0, 0);
	}
}
