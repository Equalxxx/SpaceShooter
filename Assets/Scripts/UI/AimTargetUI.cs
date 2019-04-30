using UnityEngine;
using System.Collections;

public class AimTargetUI : MonoBehaviour {

    public Transform targetUITrans;
    private float followSpeed = 5.0f;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        if (targetUITrans == null)
            return;

        if (targetUITrans.gameObject.activeSelf == true)
        {

            this.transform.position = targetUITrans.position;
        }
        else
        {
            followSpeed = 5.0f;
            this.transform.position = Vector3.Lerp(this.transform.position, Vector3.zero, Time.deltaTime * followSpeed);
        }
        
	}

    public void TargetChange(GameObject targetObj)
    {
        EnemyHealth eh = targetObj.GetComponent<EnemyHealth>();

        targetUITrans = eh.myTargetSprite.transform;

    }
}
