using UnityEngine;
using System.Collections;

public class HomingLaser2 : MonoBehaviour {

	public GameObject targetObj;
	public float moveSpeed=0.0f;
	private bool followTarget = false;
	private Transform myTrans;
    public float rotateSpeed = 90.0f;
    private float rotateBaseSpeed = 0.0f;
    private float addRotateSpeed = 360.0f;
    //private float randomMove = 0.2f;
    public bool blocking = false;

    public float damage = 2.0f;
    private ParticleSystem myParticle;
    private bool startParticle = false;
    public string targetLayer;


    public float detectRange = 3.0f;
	// Use this for initialization
	void Awake () {
        myParticle = this.GetComponentInChildren<ParticleSystem>();

		this.myTrans = this.transform;
        rotateBaseSpeed = rotateSpeed;
	}
    void Start()
    {
        
        StartCoroutine(StopLaser());
    }

    void OnEnable()
    {
        followTarget = true;
        blocking = false;
        startParticle = false;
        rotateSpeed = rotateBaseSpeed;

        Color ptColor = myParticle.startColor;
        ptColor.a = 1.0f;
        myParticle.startColor = ptColor;
    }



	// Update is called once per frame
	void Update () {
        if (!startParticle)
        {
            if (myParticle.particleCount > 0)
                startParticle = true;
        }
        else
        {
            if (myParticle.particleCount == 0)
                this.gameObject.SetActive(false);
                //Destroy(this.gameObject);
        }

        Color ptColor = myParticle.startColor;
        ptColor.a -= Time.deltaTime*0.25f;
        if (ptColor.a <= 0.0f)
            blocking = true;
        myParticle.startColor = ptColor;

        if (!blocking)
        {
            if (targetObj != null && targetObj.activeSelf)
            {
                if (followTarget)
                {
                    Vector3 dirToTarget = (targetObj.transform.position + new Vector3(0, 0.5f, 0)) - this.myTrans.position;
                    dirToTarget.Normalize();

                    Quaternion lookRotate = Quaternion.LookRotation(dirToTarget);
                    rotateSpeed += addRotateSpeed * Time.deltaTime;
                    float distToTarget = Vector3.Distance(this.transform.position, this.targetObj.transform.position);
                    if (distToTarget > 2.0f)
                    {
                        float distAngle = Quaternion.Angle(lookRotate, this.myTrans.rotation);
                        float deltaRot = rotateSpeed * Time.deltaTime;
                        float t = Mathf.Clamp01(deltaRot / distAngle);
                        this.transform.rotation = Quaternion.Slerp(this.myTrans.rotation, lookRotate, t);
                    }
                    else
                    {
                        this.transform.rotation = lookRotate;
                    }
                }
                this.myTrans.Translate(0.0f, 0.0f, moveSpeed * Time.deltaTime);

            }
            else
            {

                followTarget = false;
                this.myTrans.Translate(0.0f, 0.0f, moveSpeed * Time.deltaTime);
            }
        }

        if (targetObj!=null && targetObj.activeSelf == true)
        {
            float dist = Vector3.Distance(this.transform.position, targetObj.transform.position);
            if (dist < detectRange)
                followTarget = false;
        }
	}
	void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("Dummy") || other.CompareTag("Target"))
        {
            //Debug.Log("Blocking");
            blocking = true;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer(targetLayer))
        {

            EffectManager.Instance.GetEffect(this.transform.position, 0);
            followTarget = false;
            other.SendMessage("Havedamage", damage, SendMessageOptions.DontRequireReceiver);
            
		}
	}

    IEnumerator StopLaser()
    {
        while (true)
        {
            if (!followTarget)
            {
                yield return new WaitForSeconds(1.0f);
                blocking = true;
            }
            else
                yield return null;
        }
    }
}
