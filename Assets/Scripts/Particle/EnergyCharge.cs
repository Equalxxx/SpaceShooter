using UnityEngine;
using System.Collections;

public class EnergyCharge : MonoBehaviour {

    private ParticleSystem myParticle;
    private ParticleSystem.Particle[] pt = new ParticleSystem.Particle[100];
    public float chargingSpeed = 5.0f;
	// Use this for initialization
	void Start () {
        myParticle = this.GetComponent<ParticleSystem>();

	}
	
	// Update is called once per frame
	void Update () {

        int length = myParticle.GetParticles(pt);

        for (int i = 0; i < length; i++)
        {
            Vector3 pos = this.transform.TransformPoint(pt[i].position);
            Vector3 dirToBase = this.transform.position - pos;
            float dist = Vector3.Distance(this.transform.position, pos);

            if (dist>0.01f && dirToBase != Vector3.zero)
            {
                dirToBase.Normalize();
                //pt[i].position += dirToBase * Time.deltaTime * chargingSpeed;
                pos += dirToBase * Time.deltaTime * chargingSpeed;
                pt[i].position = this.transform.InverseTransformPoint(pos);
            }
        }
        myParticle.SetParticles(pt, length);
	}
}
