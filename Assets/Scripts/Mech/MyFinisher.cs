using UnityEngine;
using System.Collections;

public class MyFinisher : MonoBehaviour {

    public GameObject myFinisherParticle;
    private AnimatorControl myAnim;
    private Animator camAnim;
    private UnitMovement myMovement;
    private ThirdCamera tCam;
    private UnitState myUnitState;
    private bool useFin = false;
	// Use this for initialization
	void Awake () {
        myAnim = this.GetComponent<AnimatorControl>();
        camAnim = Camera.main.GetComponent<Animator>();
        myMovement = this.GetComponent<UnitMovement>();
        tCam = this.GetComponentInChildren<ThirdCamera>();
        myUnitState = this.GetComponent<UnitState>();
	}
	
    public void UseFinisherBtn()
    {
        if (useFin == false)
        {
            StartCoroutine(UseMyFinisher());
            useFin = true;
        }
    }

    IEnumerator UseMyFinisher()
    {
        tCam.useThirdCam = false;
        myFinisherParticle.SetActive(true);
        myAnim.endSkillAtk = true;
        camAnim.SetTrigger("PlayFinisher");
        myMovement.SetMove(false);
        ShakeTransform.Instance.useShake = false;
        myUnitState.useFinisher = true;
        RotateCore.Instance.stopCore = true;
        yield return new WaitForSeconds(7.0f);

        myFinisherParticle.SendMessage("AtkTarget");
        myAnim.endSkillAtk = false;
        myAnim.FinisherExit();

        yield return new WaitForSeconds(1.2f);
        myMovement.SetMove(true);
        tCam.useThirdCam = true;
        myUnitState.useFinisher = false;
        ShakeTransform.Instance.useShake = true;



    }
}
