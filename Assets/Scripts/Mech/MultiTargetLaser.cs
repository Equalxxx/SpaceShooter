using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MultiTargetLaser : MonoBehaviour, IComparer<UnitState>
{

    public List<UnitState> allEnemy;
    public List<float> angles;
    public bool targetSearch = false;
    public List<Transform> targetsTrans;

    //public GameObject laserPrefab;
    public Transform[] laserPort;
    public float firePerSec = 2.0f;
    public bool useLaser = false;
    private float fireSec = 0.0f;
    private ParticleSystem chargeEffect;
    public float delayMaxTime = 3.0f;
    public float delayTime = 3.0f;

    private AudioClip[] myClips = new AudioClip[2];
    private UnitState myUnitState;
    private bool getUnits = false;
    void Awake()
    {
        myUnitState = this.transform.root.GetComponent<UnitState>();

        laserPort = this.GetComponentsInChildren<Transform>();
        for (int i = 1; i < laserPort.Length; i++)
        {
            laserPort[i].localPosition += laserPort[i].forward*0.25f;
        }
        chargeEffect = this.transform.parent.Find("EnergeCharge").GetComponent<ParticleSystem>();

        myClips[0] = Resources.Load("EffectSound/" + "plasmx1a") as AudioClip;
        myClips[1] = Resources.Load("EffectSound/" + "ui1") as AudioClip;

        allEnemy = UnitManager.Instance.GetUnitList(UnitForce.Force_Enemy);

        
    }
	// Use this for initialization
	void Start () {

        for (int i = 0; i < allEnemy.Count; i++)
        {
            angles.Add(0.0f);
        }
        StartCoroutine(OnTarget());

	}

    // Update is called once per frame
    void Update()
    {
        if (myUnitState.useFinisher == true)
            return;
#if UNITY_STANDALONE || UNITY_EDITOR

        if (Input.GetMouseButton(1))
        {
            Shoot(true);
        }
        else
        {
            Shoot(false);
        }
#endif

        if (targetSearch)
        {
            if (!chargeEffect.isPlaying)
                chargeEffect.Play();
        }
        else
        {
            if (!chargeEffect.isStopped)
                chargeEffect.Stop();
        }


        //for (int i = 0; i < angles.Count; i++)
        //{
        //    if (allEnemy[i].gameObject.activeSelf==true)
        //    {
        //        Vector3 dirToTarget = allEnemy[i].transform.position - this.transform.position;
        //        dirToTarget.Normalize();
        //        angles[i] = Vector3.Angle(this.transform.forward, dirToTarget);
        //    }
        //}

        HomingDelay();
	}
    public void Shoot(bool onPress)
    {
        if (onPress)
        {
            targetSearch = true;
        }
        else
        {
            targetSearch = false;
            if (!useLaser && targetsTrans.Count > 0)
            {
                StartCoroutine(HomingShoot());
                useLaser = true;
                delayTime = 0.0f;
            }
        }
    }
    void HomingDelay()
    {
        if (useLaser)
        {
            if (delayTime < delayMaxTime)
                delayTime += Time.deltaTime;
            else
            {
                delayTime = delayMaxTime;
                useLaser = false;
            }

        }
    }

    IEnumerator HomingShoot()
    {
        int targetNums = targetsTrans.Count;
        int laserNums = 0;
        GetComponent<AudioSource>().PlayOneShot(this.myClips[0]);

        for (int i = 1; i < laserPort.Length; i++)
        {
            GameObject laserObj = BulletPool.Instance.GetBullet(this.laserPort[i], "HomingLaser_Player"); //Instantiate(laserPrefab, this.laserPort[i].position, this.laserPort[i].rotation) as GameObject;
            HomingLaser2 homing = laserObj.GetComponent<HomingLaser2>();

            if (targetNums > 0)
            {
                laserNums %= targetNums;
                homing.targetObj = targetsTrans[laserNums].gameObject;
                homing.targetLayer = "Enemys";
                homing.detectRange = 0.1f;
                //homing.damage = 50.0f;
                laserNums++;
            }

            yield return new WaitForSeconds(fireSec);
        }

        if (targetsTrans.Count > 0)
        {
            for (int i = 0; i < targetsTrans.Count; i++)
            {
                targetsTrans[i].gameObject.SendMessage("OffTarget",SendMessageOptions.DontRequireReceiver);
            }
            targetsTrans.Clear();
        }
        
    }

    IEnumerator OnTarget()
    {
        while (true)
        {
            if (!useLaser && targetSearch)
            {
                allEnemy.Sort(this);

                for (int i = 0; i < angles.Count; i++)
                {
                    if (allEnemy[i].gameObject.activeSelf == true)
                    {
                        Vector3 dirtotarget = allEnemy[i].transform.position - this.transform.position;
                        dirtotarget.Normalize();
                        angles[i] = Vector3.Angle(this.transform.forward, dirtotarget);
                    }
                
                    if (!targetSearch)
                        continue;
                    float angle = angles[i];
                    if (allEnemy[i].gameObject.activeSelf == true && allEnemy[i].target == true && angle < 35.0f)
                    {
                        if (this.targetsTrans.Contains(allEnemy[i].transform) == false)
                        {
                            this.targetsTrans.Add(allEnemy[i].transform);
                            allEnemy[i].SendMessage("OnTarget", SendMessageOptions.DontRequireReceiver);
                            GetComponent<AudioSource>().PlayOneShot(this.myClips[1]);
                        }
                    }
                    
                    yield return new WaitForSeconds(0.1f);
                }
//                Debug.Log("HomingLaserReady!");
                
            }
            else
            {
                yield return null;
            }
        }
    }

    public int Compare(UnitState x, UnitState y)
    {

        float distX = Vector3.Distance(this.transform.position, x.transform.position);
        float distY = Vector3.Distance(this.transform.position, y.transform.position);

        if (distX < distY)
            return -1;
        else if (distX > distY)
            return 1;
        return 0;
    }
}
