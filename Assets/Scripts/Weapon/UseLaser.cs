using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UseLaser : MonoBehaviour {

	public GameObject laserPrefab;
	//public GameObject targetObj;
	public GameObject[] laserPort;
    public float firePerSec = 3.0f;
    private bool useLaser = false;
    

	// Use this for initialization
	void Awake () {
		for (int i=0; i<this.transform.childCount; i++) {
			laserPort[i]=this.transform.GetChild(i).gameObject;
		}
        StartCoroutine(HomingDelay());
        //multiTarget = this.GetComponent<MultiTarget>();

	}

    public void Shoot(List<Transform> targetTrans)
    {
        if (!useLaser)
        {
            //for (int i = 0; i < laserPort.Length; i++)
            //{
            //    GameObject laserObj = Instantiate(laserPrefab, this.transform.position, this.transform.rotation) as GameObject;
            //    HomingLaser homing = laserObj.GetComponent<HomingLaser>();

            //    if(targetObj!=null)
            //        homing.targetObj = this.targetObj;

            //    FollowBezier fBezier = laserObj.GetComponent<FollowBezier>();
            //    fBezier.bezierSpline = laserPort[i].GetComponent<BezierSpline>();
            //}
            
            useLaser = true;
        }
    }
    IEnumerator HomingDelay()
    {
        while (true)
        {
            if (useLaser)
            {
                yield return new WaitForSeconds(firePerSec);
                useLaser = false;
            }
            else
                yield return null;
        }
    }
    //public IEnumerator HomingShoot(List<Transform> targetTrans)
    //{
    //    int targetNums = targetTrans.Count;
    //    int laserNums = 0;
    //    for (int i = 0; i < laserPort.Length; i++)
    //    {
    //        GameObject laserObj = Instantiate(laserPrefab, this.transform.position, this.transform.rotation) as GameObject;
    //        HomingLaser homing = laserObj.GetComponent<HomingLaser>();
            
    //        if (targetNums > 0)
    //        {
    //            homing.targetObj = targetTrans[laserNums].gameObject;
    //            if (laserNums < targetNums-1)
    //                laserNums++;
                
    //        }

    //        FollowBezier fBezier = laserObj.GetComponent<FollowBezier>();
    //        fBezier.bezierSpline = laserPort[i].GetComponent<BezierSpline>();
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //    if (targetTrans.Count > 0)
    //    {
    //        for (int i = 0; i < targetTrans.Count; i++)
    //        {
    //            targetTrans[i].gameObject.SendMessage("OffTarget");
    //        }
    //        targetTrans.Clear();
    //        this.multiTarget.angles.Clear();
    //        Debug.Log("clear");
    //    }
    //}
}
