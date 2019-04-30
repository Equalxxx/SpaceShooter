using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchRayCast : MonoBehaviour {

    private static TouchRayCast sInstance;
    public static TouchRayCast Instance
    {
        get
        {
            return sInstance;
        }
    }
    private Camera touchCamera;
    public LayerMask touchMask = -1; //Touch 처리할 레이어 마스크

    private string blockLayerName = "UI";                //블록킹할 레이어 이름
    private List<Touch> touchList = new List<Touch>();  //블록킹에 빠진 Touch 정보.

    public bool blocking = false;

    void Awake()
    {
        sInstance = this;
        this.touchCamera = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {

        int blockLayer = LayerMask.NameToLayer(this.blockLayerName);

        Camera blockCam = NGUITools.FindCameraForLayer(blockLayer);

        int blockMask = 1 << blockLayer;

        Touch[] touches = Input.touches;
        
        for (int i = 0; i < touches.Length; i++)
        {
            Vector3 screenPos = new Vector3(
               touches[i].position.x,
               touches[i].position.y,
               0.0f);

            Ray ray = blockCam.ScreenPointToRay(screenPos);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000.0f, blockMask) == false)
            {
                this.touchList.Add(touches[i]);
            }

        }
        for (int i = 0; i < this.touchList.Count; i++)
        {
            if (this.touchList[i].phase != TouchPhase.Began)
            {
                blocking = false;
                continue;
            }

            Vector3 screenPos = new Vector3(
                this.touchList[i].position.x,
                this.touchList[i].position.y,
                0.0f);

            Ray ray = this.touchCamera.ScreenPointToRay(screenPos);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000.0f, this.touchMask.value))
            {
                blocking = true;
            }
        }
	}
}
