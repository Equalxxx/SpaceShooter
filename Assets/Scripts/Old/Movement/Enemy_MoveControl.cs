using UnityEngine;
using System.Collections;

public class Enemy_MoveControl : MonoBehaviour {
    
    public float moveSpeed = 5.0f;
    //private CharacterController charCon;
    private Transform myTrans;
    public GameObject targetObj;
    public float targetDist = 0.0f;
    private Enemy_AnimControl enemyAnim;
    private Animator myAnim;

    private bool useAttack = false;
	// Use this for initialization
	void Awake () {
        this.myTrans = this.transform;
       // this.charCon = this.GetComponent<CharacterController>();
        this.targetObj = GameObject.FindGameObjectWithTag("Player");
        this.enemyAnim = this.GetComponent<Enemy_AnimControl>();
        myAnim = this.transform.GetChild(0).GetComponent<Animator>();

        StartCoroutine(AttackNormal());
	}
	
	// Update is called once per frame
	void Update () {
        targetDist = Vector3.Distance(targetObj.transform.position, myTrans.position);

        Vector3 targetToDir = targetObj.transform.position - myTrans.position;
        targetToDir.Normalize();

        Quaternion lookRotate = Quaternion.LookRotation(targetToDir, Vector3.up);

        float distAngle = Quaternion.Angle(lookRotate, this.transform.rotation);
        float deltaAngle = 180.0f * Time.deltaTime;
        float t = Mathf.Clamp01(deltaAngle / distAngle);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotate, t);

        if (targetDist >= 1.0f && t > 0.5f)
        {
            //charCon.Move(this.transform.forward * moveSpeed * Time.deltaTime);
            this.transform.Translate(this.transform.forward * moveSpeed * Time.deltaTime,Space.World);
            enemyAnim.moveFoward = true;
        }
        else
        {
            enemyAnim.moveFoward = false;
        }

        if (targetDist <= 1.0f && useAttack == false)
            useAttack = true;

	}

    IEnumerator AttackNormal()
    {
        while (true)
        {
            if (useAttack)
            {
                myAnim.SetTrigger("Attack");
                yield return new WaitForSeconds(2.0f);
                useAttack = false;
            }
            else
                yield return null;
        }
    }
}
