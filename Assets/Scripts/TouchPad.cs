using UnityEngine;
using System.Collections;

public class TouchPad : MonoBehaviour {

    //private TouchPadArea touchPadArea;
    
    private UISprite outCircle;
    private UISprite inCircle;
    private int fingerID = -1;      //-1 이면 Touch 안된거...

    private float radius = 100.0f;      //반지름 크기...

    private Camera myCam;               //나를 비추는 카메라..

    public Vector3 touchDir = Vector3.zero;
    void Awake()
    {
        this.outCircle = this.transform.Find("OutCircle").GetComponent<UISprite>();
        this.inCircle = this.transform.Find("InCircle").GetComponent<UISprite>();

        //반지름 크기
        this.radius = this.outCircle.localSize.x * 0.5f;

        //나를 랜더링 하는 카메라...
        this.myCam = NGUITools.FindCameraForLayer(this.gameObject.layer);

        //this.touchPadArea = this.GetComponentInParent<TouchPadArea>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //나를 Touch 한 Finger 가 존재한다...
        if (this.fingerID != -1)
        {
            //나의 FingerID 를 찾자.....
            Touch[] touches = Input.touches;
            for (int i = 0; i < touches.Length; i++)
            {
                //나를 Touch 한 ID 를 찾았다...
                if (touches[i].fingerId == this.fingerID)
                {
                    Vector3 touchScreenPos = new Vector3(
                    touches[i].position.x,
                    touches[i].position.y,
                    0.0f);

                    

                    //TouchScreenPos 를 나의 월드 위치로 바꾼다...
                    Vector3 touchWorldPos =
                        this.myCam.ScreenToWorldPoint(touchScreenPos);

                    //World 포인트를 나의 로컬위치로 바꾼다.
                    Vector3 touchLocalPos =
                        this.transform.InverseTransformPoint(touchWorldPos);

                    //z 는 필요 없다.
                    touchLocalPos.z = 0.0f;

                    touchDir.x = touchLocalPos.x;
                    touchDir.z = touchLocalPos.y;
                    
                    //로컬위치의 거리가 나의 반지름보다 작다면...
                    if (touchLocalPos.magnitude <= this.radius)
                    {
                        //안쪽원 이동시킴...
                        this.inCircle.transform.localPosition =
                            touchLocalPos;
                    }

                     //로컬위치의 거리가 나의 반지름보다 크다면...
                    else
                    {
                        touchLocalPos.Normalize();
                        
                        //안쪽원 이동시킴...
                        this.inCircle.transform.localPosition =
                            touchLocalPos * this.radius;

                    }


                    


                    return;
                }
            }

            //Touch 떼었다...
            TouchRelease();
        }
	}

    void OnPress(bool bDown)
    {

        //손가락에 잡히지 않은 상태로 터치 다운이 일어났다면...
        if (bDown && this.fingerID == -1 )
        {

            //나의 FingerID 를 찾자.....
            Touch[] touches = Input.touches;
            for (int i = 0; i < touches.Length; i++)
            {
                Vector3 touchScreenPos = new Vector3(
                    touches[i].position.x,
                    touches[i].position.y,
                    0.0f);

                //TouchScreenPos 를 나의 월드 위치로 바꾼다...
                Vector3 touchWorldPos =
                    this.myCam.ScreenToWorldPoint(touchScreenPos);

                //World 포인트를 나의 로컬위치로 바꾼다.
                Vector3 touchLocalPos =
                    this.transform.InverseTransformPoint(touchWorldPos);

                //z 는 필요 없다.
                touchLocalPos.z = 0.0f;

                //로컬위치의 거리가 나의 반지름보다 작면 이놈이 날 Touch 한놈이다...
                if (touchLocalPos.magnitude <= this.radius)
                {
                    this.fingerID = touches[i].fingerId;

                    //안쪽원 이동시킴...
                    this.inCircle.transform.localPosition =
                        touchLocalPos;

                    TouchOn();

                    return;
                }
            }

            //나를 touch 한놈은 없었다...

        }

    }

    public void TouchOn()
    {
        this.inCircle.spriteName = "TouchPadInnerOn";
        this.outCircle.spriteName = "TouchPadOn";
    }

    public void TouchRelease()
    {
        touchDir = Vector3.zero;
        this.inCircle.transform.localPosition = new Vector3(0, 0, 0);
        this.fingerID = -1;

        this.inCircle.spriteName = "TouchPadInnerOff";
        this.outCircle.spriteName = "TouchPadOff";

        //if (this.touchPadArea != null)
        //    this.touchPadArea.ChildRelease();
    }


    public Vector2 controlVector
    {
        get
        {
            return (this.inCircle.transform.localPosition * (1.0f / this.radius));

        }

    }


    //Touch 정보로 자신을 강제 셋팅
    public void SetTouch(Touch touch)
    {
        this.fingerID = touch.fingerId;


        Vector3 touchScreenPos = new Vector3(
                    touch.position.x,
                    touch.position.y,
                    0.0f);

        //TouchScreenPos 를 나의 월드 위치로 바꾼다...
        Vector3 touchWorldPos =
            this.myCam.ScreenToWorldPoint(touchScreenPos);

        touchWorldPos.z = 0.0f;

        //그월드 위치로 내가 간다.
        this.transform.position = touchWorldPos;

        this.TouchOn();
    }



}
