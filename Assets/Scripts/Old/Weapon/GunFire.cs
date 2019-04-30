using UnityEngine;
using System.Collections;

public class GunFire : MonoBehaviour {
    
    public GameObject bulletPrefab;

    public Transform fireTrans;
    private Transform myTrans;
    private bool gunTrigger = false;

    public float firePerSec = 10.0f;
    public AnimControl myAnim;

	// Use this for initialization
	void Awake () {
       // fireTrans = this.transform.FindChild("FirePoint").GetComponent<Transform>();
        myAnim = this.GetComponent<AnimControl>();
        myTrans = this.transform;
        StartCoroutine(BulletFire());
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            gunTrigger = true;
        }
        else
            gunTrigger = false;


        if (myAnim.meleeAtk)
            gunTrigger = false;
    }
	

    IEnumerator BulletFire()
    {
        while (true)
        {
            if (gunTrigger)
            {
                Instantiate(bulletPrefab, this.fireTrans.position, this.myTrans.rotation);
                yield return new WaitForSeconds(1.0f / firePerSec);
            }
            else
                yield return null;
        }
    }
}
