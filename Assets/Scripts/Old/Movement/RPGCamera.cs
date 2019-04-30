using UnityEngine;
using System.Collections;

public class RPGCamera : MonoBehaviour {

    private Transform playerTrans;
    private Transform myTrans;
    private Transform camTrans;
    public float verticalLimit = 70.0f;
    public float rotateSpeed = 90.0f;
    public float scrollSpeed = 10.0f;
    public float nowPosZ = -3.0f;
    RaycastHit hit;
	// Use this for initialization
	void Awake () {
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        myTrans = this.transform;
        camTrans = this.transform.GetChild(0).GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = playerTrans.position;
        pos.y += 0.5f;
        myTrans.position = pos;

        CameraRotate();
        DetectCollision();
        CameraMove();

	}

    void CameraRotate()
    {
        float mouseRotateX = Input.GetAxis("Mouse X");
        float mouseRotateY = Input.GetAxis("Mouse Y");

        
            myTrans.Rotate(-mouseRotateY * rotateSpeed * Time.deltaTime, mouseRotateX * rotateSpeed * Time.deltaTime, 0,Space.Self);
            Vector3 rotateVec = myTrans.eulerAngles;
            rotateVec.z = 0;
            //rotateVec.x = Mathf.Clamp(rotateVec.x, -verticalLimit, verticalLimit);
            myTrans.rotation = Quaternion.Euler(rotateVec);
        
    }

    void CameraMove()
    {
        float wheelDelta = Input.GetAxis("Mouse ScrollWheel");

        if (wheelDelta != 0.0f)
        {
            Vector3 camPos = camTrans.localPosition;
            camPos.z += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
            camPos.z = Mathf.Clamp(camPos.z, -3, -1);
            nowPosZ = camPos.z;
            camTrans.localPosition = camPos;
        }
    }

    void DetectCollision()
    {
        Vector3 dirToTarget = camTrans.position - myTrans.position;
        Ray ray = new Ray(myTrans.position, dirToTarget);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag != "Player")
            {
                camTrans.position = hit.point;
                Vector3 pos = camTrans.localPosition;
                pos.x = 0;
                pos.y = 0;
                pos.z = Mathf.Clamp(pos.z, nowPosZ, -1);
                camTrans.localPosition = pos;
            }
        }

        Debug.DrawRay(ray.origin,ray.direction,Color.red);
    }
    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            this.transform.position,
            hit.point);

        //히트지점에 빨간 구
        Gizmos.DrawSphere(
            hit.point, 0.3f);

    }
}
