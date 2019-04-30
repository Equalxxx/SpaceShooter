using UnityEngine;
using System.Collections;

public class MoveControl : MonoBehaviour {

    private CharacterController charMotor;
    private Transform camTrans;
    private Quaternion charRotate = Quaternion.identity;
    private Quaternion lookRotate = Quaternion.identity;
    public float moveSpeed = 3.0f;
    private AnimControl myAnim;
	// Use this for initialization
	void Awake () {
        Screen.lockCursor = true;
        charMotor = this.GetComponent<CharacterController>();
        camTrans = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        myAnim = this.GetComponent<AnimControl>();
	}

    void Update()
    {
        Vector3 camFoward = camTrans.forward;
        //camFoward.y = 0;
        camFoward.Normalize();
        charRotate = Quaternion.LookRotation(camFoward);



        float InputV = Input.GetAxis("Vertical");
        float InputH = Input.GetAxis("Horizontal");

        Vector3 lookDir = new Vector3(0, 0, InputV);
        Vector3 moveDir = new Vector3(InputH, 0, InputV);
        if (moveDir != Vector3.zero)
            moveDir.Normalize();
        
        if (InputV == 0.0f && InputH != 0.0f)
            lookDir = Vector3.forward;
        if (lookDir != Vector3.zero)
        {
            lookDir.Normalize();

            if (lookDir.z < 0.0f)
            {
                lookDir.z *= -1.0f;
                lookDir.x *= -1.0f;
            }

            lookRotate = charRotate * Quaternion.LookRotation(lookDir, Vector3.up);
        }

        float distAngle = Quaternion.Angle(this.lookRotate, this.transform.rotation);
        float deltaAngle = 360.0f * Time.deltaTime;
        float t = Mathf.Clamp01(deltaAngle / distAngle);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this.lookRotate, t);

        if(!myAnim.meleeAtk)
            charMotor.Move(this.transform.TransformDirection(moveDir*moveSpeed*Time.deltaTime));
    }
}
