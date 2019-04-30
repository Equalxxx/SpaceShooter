using UnityEngine;
using System.Collections;

public class LargeBeam : MonoBehaviour {

    public GameObject beamPrefab;
    private ParticleSystem chargeEffect;
    private bool shoot;
	// Use this for initialization
	void Start () {
        chargeEffect = this.GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (shoot)
        {
            Vector3 pos = new Vector3(0, 0, -0.5f);
            BulletPool.Instance.GetBullet(this.transform, "BeamLarge_Boss");//Instantiate(beamPrefab, this.transform.position+pos, this.transform.rotation);
            shoot = false;
        }
	}

    public void ShootLargeBeam()
    {
        StartCoroutine(ChargeStart());
    }

    IEnumerator ChargeStart()
    {
        if (!chargeEffect.isPlaying)
            chargeEffect.Play();
        yield return new WaitForSeconds(1.5f);
        if (chargeEffect.isPlaying)
            chargeEffect.Stop();
        shoot = true;
    }
}
