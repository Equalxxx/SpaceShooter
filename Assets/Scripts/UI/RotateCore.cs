using UnityEngine;
using System.Collections;

public class RotateCore : MonoBehaviour {

    private static RotateCore sInstance;
    public static RotateCore Instance
    {
        get
        {
            return sInstance;
        }
    }
    public float angle = 0.0f;
    private Transform myTrans;
    public bool onFinish = false;
	// Use this for initialization
    public bool stopCore = false;
    private Vector3 basePos;
    private UIButton finBtn;

    void Awake()
    {
        sInstance = this;
        myTrans = this.transform;
        basePos = myTrans.position;
        finBtn = this.GetComponentInChildren<UIButton>();
    }
	
	// Update is called once per frame
	void Update () {
        if (stopCore == false)
        {

            if (onFinish == true)
            {
                finBtn.enabled = true;
                myTrans.rotation = Quaternion.identity;
                myTrans.position = Vector3.Lerp(myTrans.position, Vector3.zero, Time.deltaTime * 3);
            }
            else
            {

                myTrans.Rotate(0.0f, angle * Time.deltaTime, 0.0f);
            }
        }
        else
        {
            finBtn.enabled = false;
            
            myTrans.position = Vector3.Lerp(myTrans.position, basePos, Time.deltaTime * 3);
        }
	}
    public void AddAngleColor(float a)
    {
        angle = a;
        Color addColor = this.GetComponent<Renderer>().material.GetColor("_TintColor");
        addColor.r += 0.025f;
        addColor.g += 0.025f;
        addColor.b += 0.025f;
        this.GetComponent<Renderer>().material.SetColor("_TintColor", addColor);
    }
}
