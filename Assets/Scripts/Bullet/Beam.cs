using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour {

    public float moveSpeed = 30.0f;
    public float liveTime = 1.0f;
    public float damage = 1.0f;
    public bool blocking = false;
    private AudioClip myClip;

    void Start()
    {
        myClip = Resources.Load("EffectSound/" + "RailGunSound") as AudioClip;
    }
    void OnEnable()
    {
        StartCoroutine(AutoDisable());
        blocking = false;
        GetComponent<AudioSource>().PlayOneShot(this.GetComponent<AudioSource>().clip);
    }

	void Update () {
        if(!blocking)
            this.transform.Translate(this.transform.forward * moveSpeed * Time.deltaTime,Space.World);
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dummy")
        {
            Debug.Log("Blocking");
            blocking = true;
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
            return;
        }

        if (other.CompareTag("Bullet")==false&&other.gameObject.layer != this.gameObject.layer)
        {
            other.SendMessage("Havedamage", damage, SendMessageOptions.DontRequireReceiver);
            EffectManager.Instance.GetEffect(this.transform.position, 0);
        }
    }
    IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(liveTime);
        if(this.gameObject.activeSelf==true)
            this.gameObject.SetActive(false);
    }
}
