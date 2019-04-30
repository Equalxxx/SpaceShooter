using UnityEngine;
using System.Collections;

public class Finisher : MonoBehaviour {

    public bool onSkill = false;
    public Transform targetTrans;
    private float speed = 0.0f;
    private float maxSpeed = 30.0f;
    private ParticleSystem myParticle;
    private Transform playerTrans;
    private Transform myTrans;
    private bool activeSkill = false;
    private float risingPos = 0.3f;
    public Transform[] laserPort;
    public AudioClip[] myClip = new AudioClip[2];
    private bool useSkill = false;
	// Use this for initialization
	void Awake () {
        myParticle = this.GetComponent<ParticleSystem>();
        targetTrans = GameObject.FindGameObjectWithTag("Target").GetComponent<Transform>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        myTrans = this.transform;
        laserPort = this.transform.Find("LaserPort").GetComponentsInChildren<Transform>();
	}
    void Start()
    {
        this.gameObject.SetActive(false);
        this.GetComponent<AudioSource>().volume = 0.0f;

    }
    void OnEnable()
    {
        activeSkill = true;
        
    }
	// Update is called once per frame
	void Update () {
        //if (onSkill == true)
        //{
        //    //Vector3 dirToTarget = targetTrans.position - this.transform.position;

        //    //dirToTarget.Normalize();

        //    //Vector3 pos = this.transform.position;
        //    //speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime*0.8f);
        //    //this.transform.Translate(dirToTarget * Time.deltaTime * speed, Space.World);

        //}
        //else
        //{
        //    if (activeSkill == true)
        //    {
        //        if (audio.volume >= 1.0f)
        //            audio.volume = 1.0f;
        //        audio.volume += Time.deltaTime*0.25f;

        //    }
        //    Vector3 pos = playerTrans.position;
        //    pos.y += risingPos;
            
        //    risingPos += Time.deltaTime * 0.25f;
        //    myTrans.position = pos;
        //}
        if (onSkill == false)
        {
            if (activeSkill == true)
            {
                if (GetComponent<AudioSource>().isPlaying != false)
                {
                    //Debug.Log("111");
                    //this.audio.clip = this.myClip[0];
                    //this.audio.Play();
                }
                if (GetComponent<AudioSource>().volume >= 1.0f)
                    GetComponent<AudioSource>().volume = 1.0f;
                GetComponent<AudioSource>().volume += Time.deltaTime * 0.25f;

            }
            Vector3 pos = playerTrans.position;
            pos.y += risingPos;

            risingPos += Time.deltaTime * 0.25f;
            myTrans.position = pos;
        }
        else
        {
            if (useSkill == false)
            {
                StartCoroutine(Fire());
                useSkill = true;
            }
        }

        if (useSkill == true)
        {
            if (myParticle.particleCount == 0)
            {
                this.gameObject.SetActive(false);
            }
        }
	}

    void AtkTarget()
    {
        onSkill = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target") || other.gameObject.CompareTag("Enemy"))
        {
            other.SendMessage("Havedamage", 1000, SendMessageOptions.DontRequireReceiver);
            EffectManager.Instance.GetEffect(this.transform.position, 0);
            
        }
        if(other.gameObject.CompareTag("Target"))
        {
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator Fire()
    {
        for (int i = 0; i < laserPort.Length*2; i++)
        {
            int count = i;
            count %= 11;
            if (laserPort[count].name.Equals("LaserPort") == false)
            {
                Debug.Log(count.ToString());
                GameObject laserObj = BulletPool.Instance.GetBullet(this.laserPort[count], "HomingLaser_Player");
                HomingLaser2 homing = laserObj.GetComponent<HomingLaser2>();

                homing.targetObj = targetTrans.gameObject;
                homing.targetLayer = "Ship";
                homing.detectRange = 0.1f;
                homing.damage = 3.0f;
                GetComponent<AudioSource>().PlayOneShot(this.myClip[1]);
                yield return new WaitForSeconds(0.05f);
            }
        }
        myParticle.Stop();
    }
}
