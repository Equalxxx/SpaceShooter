using UnityEngine;
using System.Collections;

public class AroundDummy : MonoBehaviour {

    public Transform targetTrans;
    public float angle = 0.0f;
    public float nowAngle = 0.0f;
    public bool around = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float dist = Vector3.Distance(this.transform.position, targetTrans.position);

        if (!around && dist > 3.0f)
        {
            Vector3 dirToTarget = this.targetTrans.position - this.transform.position;

            dirToTarget.Normalize();
            nowAngle = Vector3.Angle(dirToTarget, this.transform.forward) * Mathf.Rad2Deg;
            angle = nowAngle;
            this.transform.Translate(dirToTarget * Time.deltaTime * 10.0f);

        }
        else
            around = true;

        if (around)
        {

            float pitAngle = nowAngle + (180.0f * Mathf.Deg2Rad);
            Vector3 dirToTarget = this.targetTrans.position - this.transform.position;

            dirToTarget.Normalize();
            if (angle < pitAngle)
            {
                angle += 90.0f * Time.deltaTime * Mathf.Deg2Rad;
                this.transform.RotateAround(targetTrans.position, this.transform.up, 90.0f * Time.deltaTime);
            }
        }

	}
}
