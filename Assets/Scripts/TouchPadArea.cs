using UnityEngine;
using System.Collections;

public class TouchPadArea : MonoBehaviour {

    private TouchPad childTouchPad;
    private int fingerID = -1;

    private Camera myCam;               //나를 비추는 카메라..
    

    void Awake()
    {
        this.childTouchPad = this.GetComponentInChildren<TouchPad>();
        
        //나를 랜더링 하는 카메라...
        this.myCam = NGUITools.FindCameraForLayer(this.gameObject.layer);

    }

    void Start()
    {
        //this.childTouchPad.gameObject.SetActive(false);
    }

    void OnPress(bool bDown)
    {
        if (bDown)
        {
            Bounds bounds = this.GetComponent<Collider>().bounds;

            if (this.fingerID == -1)
            {
                //나를 Touch 한FingerID 가 누구니?
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

                    if (bounds.min.x <= touchWorldPos.x && touchWorldPos.x <= bounds.max.x &&
                        bounds.min.y <= touchWorldPos.y && touchWorldPos.y <= bounds.max.y)
                    {
                        //나를 Touch 한 ID
                        this.fingerID = touches[i].fingerId;

                        //this.childTouchPad.gameObject.SetActive(true);
                        this.childTouchPad.SetTouch(touches[i]);


                        return;
                    }

                    

                }

            }
        }
    }

    void OnDrawGizmos()
    {
        //Collider 의 Bound MinMax 를 얻자.
        Bounds bounds = this.GetComponent<Collider>().bounds;
        //bounds 의 min max 는 Collider 의 AABB 정보다...

        
        Vector3 center = ( bounds.min + bounds.max ) * 0.5f;
        Vector3 size = new Vector3(
            bounds.max.x - bounds.min.x, 
            bounds.max.y - bounds.min.y, 
            bounds.max.z - bounds.min.z );
        Gizmos.DrawWireCube(center, size);

    }


    //자식의 TouchPad 로 부터 호출된다...
    public void ChildRelease()
    {
        //this.childTouchPad.gameObject.SetActive(false);
        this.fingerID = -1;

    }


}
