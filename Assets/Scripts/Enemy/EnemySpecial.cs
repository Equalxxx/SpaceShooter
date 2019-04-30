using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpecial : MonoBehaviour {

    public bool spAttack = false;
    public GameObject bulletPrefab;
    public List<UnitState> targets;
    public List<Transform> targetTrans;
    private Transform[] laserPort;
    public LayerMask myLayerMask;
    void Awake()
    {
        laserPort = this.GetComponentsInChildren<Transform>();
    }
	// Use this for initialization
	void Start () {

        targets = UnitManager.Instance.GetUnitList(UnitForce.Force_Team);
	}
    void OnEnable()
    {
        StartCoroutine(AttackDelay());
    }

    void OnDisable()
    {
        StopCoroutine(AttackDelay());
    }

    public void ShootSpecial_1()
    {
        if(spAttack==false)
            StartCoroutine(SpecialAttack_HomingLaser());
    }
    IEnumerator AttackDelay()
    {
        while (true)
        {
            if (spAttack == true)
            {
                float randTime = Random.Range(3.0f, 5.0f);
                yield return new WaitForSeconds(randTime);
                spAttack = false;
            }
            else
                yield return null;
        }
    }

    IEnumerator SpecialAttack_HomingLaser()
    {
        Debug.Log("Warning!");
        yield return new WaitForSeconds(0.3f);
        
        targetTrans.Clear();
        
        Collider[] hitCols;
        hitCols = Physics.OverlapSphere(this.transform.position, 300, myLayerMask);
        Debug.Log(hitCols.Length.ToString());
        //for (int i = 0; i < angles.Count; i++)
        //{
        //    if (targets[i].gameObject.activeSelf==true && angles[i] < 30.0f)
        //    {
        //        targetTrans.Add(targets[i].transform);
        //    }
        //}

        if (hitCols.Length > 0)
        {
            //Debug.Log("add!");
            for (int i = 0; i < hitCols.Length; i++)
            {
                if (hitCols[i].CompareTag("Bullet"))
                    continue;
                
                //if (targets[i].gameObject.activeSelf == true && targetTrans.Contains(hitCols[i].transform) == false)
                if (targetTrans.Contains(hitCols[i].transform) == false)
                {
                    targetTrans.Add(hitCols[i].transform);
                }
            }
            


            int targetNums = targetTrans.Count;
            
            int count = 0;
            
            if (targetNums > 0)
            {
                spAttack = true;
                for (int i = 1; i < this.laserPort.Length; i++)
                {
                    //targetTrans[i]
                        GameObject newBullet = BulletPool.Instance.GetBullet(this.laserPort[i], "HomingLaser_Boss");//Instantiate(bulletPrefab, this.laserPort[i].position, this.laserPort[i].rotation) as GameObject;
                        HomingLaser2 homing = newBullet.GetComponent<HomingLaser2>();


                        homing.targetObj = targetTrans[count].gameObject;
                        homing.targetLayer = "Teams";
                        homing.moveSpeed = 15.0f;
                        homing.rotateSpeed = 60.0f;
                        count++;
                        if (count >= targetNums)
                            count = 0;

                    GetComponent<AudioSource>().PlayOneShot(this.GetComponent<AudioSource>().clip);
                    yield return new WaitForSeconds(0.2f);
                }
                
            }
            
        }
        else
            yield return null;
    }
}
