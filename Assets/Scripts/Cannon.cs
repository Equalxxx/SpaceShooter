using UnityEngine;
using System.Collections;


public class Cannon : MonoBehaviour {

    public GameObject target;

    private Transform body;         //캐논바디
    private Transform head;         //캐논헤드

    public float leftAngleMax = -90.0f;
    public float rightAngleMax = 90.0f;
    public float bodyRotSpeed = 90.0f;      //바디 초당 회전 속도
    private float nowBodyAngle = 0.0f;      //현제 바디 앵글
    public bool bodyRotLimit = false;       //이게 false 면 바디 회전한계는 없다.



    public float upAngleMax = -90.0f;
    public float downAngleMax = 10.0f;
    public float headRotSpeed = 90.0f;      //헤드 초당 회전 속도
    private float nowHeadAngle = 0.0f;      //현제 헤드 앵글


    void Awake()
    {
        //바디와 헤드를 찾는다.
        this.body = this.transform.Find("Body");
        this.head = this.body.Find("Head");

        this.body.localRotation = Quaternion.identity;
        this.head.localRotation = Quaternion.identity;




    }
	
	// Update is called once per frame
	void Update () {

        //타겟과의 방향벡터 ( 총이 달려있는 헤드 기준으로 )
        Vector3 dirToTarget = this.target.transform.position - this.head.transform.position;

        //로컬 방향
        Vector3 localDir = this.transform.InverseTransformDirection(dirToTarget);


        //
        // 바디를 돌리자..
        //
        localDir.y = 0.0f;
        if (localDir != Vector3.zero)
        {
            localDir.Normalize();

            //바디 로컬 앵글
            float bodyAngle = Mathf.Acos(localDir.z) * Mathf.Rad2Deg;
            if (localDir.x < 0.0f)
                bodyAngle *= -1.0f;

            //바디의 회전의 한계가 필요할때...
            if (this.bodyRotLimit)
            {

                //한계 각 셋팅
                bodyAngle = Mathf.Clamp(bodyAngle, this.leftAngleMax, this.rightAngleMax);

                float dist = Mathf.Abs(bodyAngle - this.nowBodyAngle);
                float delta = this.bodyRotSpeed * Time.deltaTime;
                float t = Mathf.Clamp01(delta / dist);
                this.nowBodyAngle = Mathf.Lerp(this.nowBodyAngle, bodyAngle, t);

                //바디 로컬회전 셋팅
                this.body.transform.localRotation = Quaternion.Euler(0, this.nowBodyAngle, 0);

            }


            //바디의 한계각이 체크 되지 않을때....
            else
            {
                Quaternion targetRot = Quaternion.Euler(0, bodyAngle, 0);


                float dist = Quaternion.Angle(targetRot, this.body.localRotation);      //사원수끼리의 회전차.
                float delta = this.bodyRotSpeed * Time.deltaTime;
                float t = Mathf.Clamp01(delta / dist);

                this.body.localRotation =
                    Quaternion.Slerp(this.body.localRotation, targetRot, t);

            }
        }


        //로컬 방향
        localDir = this.body.InverseTransformDirection(dirToTarget);

        //
        // 헤드를 돌리자.
        //
        localDir.x = 0.0f;
        if (localDir != Vector3.zero)
        {
            localDir.Normalize();

            //바디 로컬 앵글
            float headAngle = Mathf.Acos(localDir.z) * Mathf.Rad2Deg;
            if (localDir.y >= 0.0f)
                headAngle *= -1.0f;

            //한계 각 셋팅
            headAngle = Mathf.Clamp(headAngle, this.upAngleMax, this.downAngleMax);

            float dist = Mathf.Abs(headAngle - this.nowHeadAngle);
            float delta = this.headRotSpeed * Time.deltaTime;
            float t = Mathf.Clamp01(delta / dist);
            this.nowHeadAngle = Mathf.Lerp(this.nowHeadAngle, headAngle, t);

            //헤드 로컬회전 셋팅
            this.head.transform.localRotation = Quaternion.Euler(this.nowHeadAngle, 0, 0);
        }


	
	}
}
