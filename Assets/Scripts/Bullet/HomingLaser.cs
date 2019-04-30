using UnityEngine;
using System.Collections;

public class HomingLaser : MonoBehaviour {

	public GameObject targetObj;
	public float moveSpeed=0.0f;
	private bool followTarget = false;
	private Transform myTrans;
	private FollowBezier fBezier;
    public float liveTime = 5.0f;
    //private float randomMove = 0.2f;
    public bool blocking = false;
	// Use this for initialization
	void Awake () {
		this.fBezier = this.GetComponent<FollowBezier> ();
		this.myTrans = this.transform;
		followTarget = true;
        Destroy(this.gameObject, liveTime);
	}
	
	// Update is called once per frame
	void Update () {

        if (!blocking)
        {
            if (targetObj != null && targetObj.activeSelf)
            {
                if (fBezier.factor >= 1.0f && followTarget)
                {
                    Vector3 dirToTarget = targetObj.transform.position - this.myTrans.position;
                    dirToTarget.y += 0.5f;
                    dirToTarget.Normalize();
                    this.myTrans.forward = dirToTarget;
                    this.myTrans.Translate(0.0f, 0.0f, moveSpeed * Time.deltaTime);
                }
                if (!followTarget)
                {
                    this.myTrans.Translate(0.0f, 0.0f, moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                this.myTrans.Translate(0.0f, 0.0f, moveSpeed * Time.deltaTime);
            }
        }
	}

	void OnTriggerEnter(Collider other)
	{
        if (other.tag == "Dummy")
        {
            Debug.Log("Blocking");
            blocking = true;
        }
		if (other.tag == "Enemy") {
			Debug.Log("Hit");
            other.SendMessage("Havedamage", 1.0f, SendMessageOptions.DontRequireReceiver);
			followTarget=false;
		}
	}
}
