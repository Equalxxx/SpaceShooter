using UnityEngine;
using System.Collections;

public class ShakeTransform : MonoBehaviour {

    private float shakePosPower;           //흔들림의 파위
    private float shakePosRelease;         //초당 흔들림 감쇠량

    public bool shakePosX = true;
    public bool shakePosY = true;
    public bool shakePosZ = true;

    private float shakeRotPower;
    private float shakeRotRelease;

    public bool shakeRotX = true;
    public bool shakeRotY = true;
    public bool shakeRotZ = true;

    public bool useShake = true;

    private static ShakeTransform sInstance;
    public static ShakeTransform Instance
    {
        get
        {

            return sInstance;
        }

    }
    void Awake()
    {
        sInstance = this;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (useShake == false)
            return;
        //쉐이크 파워가 존재한다면...
        if (this.shakePosPower > 0)
        {
            //흔드기 방향에 대한 랜덤 방향을 얻자.
            Vector3 randDir;
            randDir.x = ((this.shakePosX) ? Random.Range(-1.0f, 1.0f) : 0);
            randDir.y = ((this.shakePosY) ? Random.Range(-1.0f, 1.0f) : 0);
            randDir.z = ((this.shakePosZ) ? Random.Range(-1.0f, 1.0f) : 0);
            randDir.Normalize();

            //위에서 구한 랜덤 방향으로 shakePower 만큼 로컬 위치 잡는다.
            Vector3 localPostion = randDir * shakePosPower;
            this.transform.localPosition = localPostion;

            //쉐이크 파워 감소
            this.shakePosPower -= this.shakePosRelease * Time.deltaTime;

            if (this.shakePosPower < 0.0f)
            {
                this.transform.localPosition = Vector3.zero;
                this.shakePosPower = 0.0f;
            }
        }

        //쉐이크 파워가 존재한다면...
        if (this.shakeRotPower > 0)
        {
            //흔드기 방향에 대한 랜덤 방향을 얻자.
            Vector3 randDir;
            randDir.x = ((this.shakeRotX) ? Random.Range(-1.0f, 1.0f) : 0);
            randDir.y = ((this.shakeRotY) ? Random.Range(-1.0f, 1.0f) : 0);
            randDir.z = ((this.shakeRotZ) ? Random.Range(-1.0f, 1.0f) : 0);
            randDir.Normalize();

            //위에서 구한 랜덤 방향으로 shakePower 만큼 로컬 회전량
            Vector3 localAngle = randDir * shakeRotPower;
            this.transform.localRotation = Quaternion.Euler( localAngle );

            //쉐이크 파워 감소
            this.shakeRotPower -= this.shakeRotRelease * Time.deltaTime;

            if (this.shakeRotPower < 0.0f)
            {
                this.transform.localRotation = Quaternion.identity;
                this.shakeRotPower = 0.0f;
            }
        }

	}




    public void ShakePosOrder(float shakePower, float shakeTime)
    {
        //흔들기 파워를 준다
        this.shakePosPower = shakePower;

        //흔드기를 몇초동안 하는 것에 따라 초당 감쇠량 결정
        this.shakePosRelease = shakePower / shakeTime;
    }

    public void ShakeRotOrder(float shakePower, float shakeTime)
    {
        Debug.Log("shake");
        //흔들기 파워를 준다
        this.shakeRotPower = shakePower;

        //흔드기를 몇초동안 하는 것에 따라 초당 감쇠량 결정
        this.shakeRotRelease = shakePower / shakeTime;
    }
}
