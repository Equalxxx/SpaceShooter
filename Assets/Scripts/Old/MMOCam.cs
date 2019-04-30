using UnityEngine;
using System.Collections;

public class MMOCam : MonoBehaviour {

    public Transform lookTarget;
    private Transform myTrans;
    public float sensitivityX = 3.0f;
    public float sensitivityY = -3.0f;

    private float nowHorizontalAngle = 0.0f;        //현제 수평각
    private float nowVerticalAngle = 0.0f;          //현제 수직각

    private float nowDistance = 3.0f;
    private float prevDistance = 0.0f;
    public float wheelSensitivity = -3.0f;

    private RaycastHit hit;
    public LayerMask layerMask = -1;
	// Use this for initialization
	void Awake () {
        myTrans = this.transform;
        prevDistance = nowDistance;
	}
	
	// Update is called once per frame
	void LateUpdate () {

        nowHorizontalAngle += Input.GetAxis("Mouse X") * this.sensitivityX;
        nowVerticalAngle += Input.GetAxis("Mouse Y") * this.sensitivityY;
        nowVerticalAngle = Mathf.Clamp(nowVerticalAngle, -80.0f, 80.0f);

        //타겟기준으로 카메라가 있어야될 방향의 벡터 준비
        Vector3 dir = new Vector3(0, 0, -1);        //뒤방향으로 시작

        //회전 사원수
        Quaternion rot = Quaternion.Euler(nowVerticalAngle, nowHorizontalAngle, 0);

        //벡터 회전
        dir = rot * dir;


        DetectCollision();
        

        //카메라가 있어야 할위치
        this.transform.position = lookTarget.position + dir * this.nowDistance;

        this.transform.rotation = rot;

        


	
	}

    void DetectCollision()
    {
        nowDistance += Input.GetAxis("Mouse ScrollWheel") * this.wheelSensitivity;
        if(Input.GetAxis("Mouse ScrollWheel")!=0)
            prevDistance=nowDistance;
        //거리 제한
        nowDistance = Mathf.Clamp(nowDistance, 1.0f, 2.0f);

        Vector3 dirToTarget = lookTarget.position - myTrans.position;
        Ray ray = new Ray(myTrans.position, dirToTarget);

        if (Physics.Raycast(ray, out hit, this.nowDistance, this.layerMask.value))
        {
            //myTrans.position = hit.point;
            //Vector3 pos = myTrans.localPosition;
            //nowDistance = Mathf.Clamp(pos.z, nowDistance, -1);
            //Debug.Log(nowDistance.ToString());
            float dist = Vector3.Distance(lookTarget.position, hit.point);
            dist = Mathf.Clamp(dist, 1.0f, prevDistance);
            nowDistance = dist;
            Debug.Log(hit.collider.name);
        }

        Debug.DrawRay(ray.origin, ray.direction, Color.red);
    }
}
