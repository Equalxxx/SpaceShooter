using UnityEngine;
using System.Collections;

public class ParticleAutoDisable : MonoBehaviour {

    private ParticleSystem[] particles;
    private bool[] particlesStart;              //시작 여부 배열

    public float limitTime = 5.0f;


    void Awake()
    {
        this.particles = this.GetComponentsInChildren<ParticleSystem>();
        this.particlesStart = new bool[this.particles.Length];

        //일단은 싹다 Disable
        for (int i = 0; i < this.particles.Length; i++)
        {
            this.particles[i].enableEmission = false;
            this.particles[i].simulationSpace = ParticleSystemSimulationSpace.Local;
        }

    }

    void OnEnable()
    {

        //모두다 활성화
        for (int i = 0; i < this.particles.Length; i++)
        {
            this.particles[i].enableEmission = true;
            //아직은 시작한게 아니다.
            this.particlesStart[i] = false;
        }
        GetComponent<AudioSource>().PlayOneShot(this.GetComponent<AudioSource>().clip);

        Invoke("ForceDisable", this.limitTime);

    }

    //강제 Disable
    void ForceDisable()
    {
        for (int i = 0; i < this.particles.Length; i++)
        {
            //아직도 발생중이라면..
            if (this.particles[i].enableEmission)
                this.particles[i].Stop();
        }

    }

	
	// Update is called once per frame
	void Update () {

        //파티클수가 남았지 않다면 Disable 시켜라
        for (int i = 0; i < this.particles.Length; i++)
        {
             //아직 시작 안했다몀...
            if (this.particlesStart[i] == false)
            {
                if (this.particles[i].particleCount > 0)
                    this.particlesStart[i] = true;
            }

            else
            {
                if (this.particles[i].particleCount == 0)
                    this.particles[i].enableEmission = false;
            }
        }

        //모두다 Disable 이니?
        bool allDisable = true;

        for (int i = 0; i < this.particles.Length; i++)
        {
            if (this.particles[i].enableEmission)
            {
                allDisable = false;
                break;
            }
        }

        if (allDisable)
            this.gameObject.SetActive(false);


	}
}
