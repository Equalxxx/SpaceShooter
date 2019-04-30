using UnityEngine;
using System.Collections;

public class UseBeamRifle : MonoBehaviour {

    private Transform myTransform;
    public string targetTag;
    public float firePerSec = 1.0f;
    private bool useRifle = false;
    public float range = 30.0f;
    public string bulletType = "";
    public UnitState myUnitState;
	// Use this for initialization
	void Awake () {
        myUnitState = this.transform.parent.GetComponent<UnitState>();

        myTransform = this.transform;
        if (this.gameObject.layer == LayerMask.NameToLayer("Teams"))
            bulletType = "Beam_Team";
        else if (this.gameObject.layer == LayerMask.NameToLayer("Enemys"))
            bulletType = "Beam_Enemy";
	}
    void Start()
    {
       // StartCoroutine(BeamDelay());
    }
    void OnEnable()
    {
        StartCoroutine(BeamDelay());
    }

    void OnDisable()
    {
        StopCoroutine(BeamDelay());

    }
    void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        if (this.transform.root.CompareTag("Player") && Input.GetMouseButton(0))
            Shoot();
#endif
    }
    public void Shoot()
    {

        if (this.myUnitState.useFinisher==false && !useRifle)
        {
            //Instantiate(beamPrefab, this.myTransform.position, this.myTransform.rotation);

            BulletPool.Instance.GetBullet(this.transform, bulletType);
            useRifle = true;
            StartCoroutine(BeamDelay());
            

            //Debug.DrawRay(ray.origin, ray.direction * range);
        }

    }

    IEnumerator BeamDelay()
    {
        while (true)
        {
            if (useRifle)
            {
                yield return new WaitForSeconds(firePerSec);
                useRifle = false;
            }
            else
                yield return null;
        }
    }
}
