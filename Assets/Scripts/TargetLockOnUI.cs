using UnityEngine;
using System.Collections;

public class TargetLockOnUI : MonoBehaviour
{
    public Transform targetGameObject;
    public Camera targetCamera;
    public Camera myCamera;
    private TweenScale ts;
    private bool tweenPlay;
    private UISprite mySprite;
    void Awake()
    {
        this.targetCamera = Camera.main;
        this.myCamera = this.FindCamera(this.gameObject.layer);
        this.ts = this.GetComponent<TweenScale>();
        this.mySprite = this.GetComponent<UISprite>();
    }

	// Update is called once per frame
	void Update () {
        if (targetGameObject == null)
            return;

        if (targetGameObject.gameObject.activeSelf == false)
            this.gameObject.SetActive(false);

        Vector3 pos = this.targetGameObject.transform.position;
        pos.y += 0.5f;
        //타겟의 Screen 위치
        Vector3 targetScreenPos = this.targetCamera.WorldToScreenPoint(
            pos);

        //내카메의 월드위치
        Vector3 myCamWorldPos = this.myCamera.ScreenToWorldPoint(
            targetScreenPos);


        //내월드위치에서 음수가 나왔다는 것은 해당타겟도 해당카메라 뒤쪽에 있다는 예기
        if (myCamWorldPos.z > 0.0f)
        {
            //Z 는 0 으로..
            myCamWorldPos.z = 0.0f;

            if (mySprite.enabled == false)
                mySprite.enabled = true;

            this.transform.position = myCamWorldPos;
            if (tweenPlay)
            {
                ts.ResetToBeginning();
                ts.PlayForward();
                tweenPlay = false;
            }
        }
        else
        {
            if (mySprite.enabled == true)
                mySprite.enabled = false;
        }
	
	}
    public void PlayTween()
    {
        tweenPlay = true;
    }

    public Camera FindCamera(int layer)
    {
        //씬에 배치된 카메라를 싹다 가져온다..
        Camera[] cams = GameObject.FindObjectsOfType<Camera>();

        int findLayerMask = 1 << layer;
        for (int i = 0; i < cams.Length; i++)
        {
            if ((cams[i].cullingMask & findLayerMask) != 0)
            {
                return cams[i];

            }
        }

        return null;
    }
}
