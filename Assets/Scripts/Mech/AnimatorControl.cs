using UnityEngine;
using System.Collections;

public class AnimatorControl : MonoBehaviour {

    private UnitState myUnitState;
    private Animator myAnim;
    
    private AnimatorStateInfo[] animatorStateInfos;
    public bool meleeAtk = false;
    public bool rifleAtk = false;
    public bool laserAtk = false;
    public Animation myAnimBackpack;
    public bool endSkillAtk = false;
    public bool endSkillExit = false;
    private float aniTime = 0.0f;
	// Use this for initialization
	void Start () {
        myUnitState = this.GetComponent<UnitState>();
        myAnim = this.GetComponentInChildren<Animator>();
        animatorStateInfos = new AnimatorStateInfo[this.myAnim.layerCount];
        myAnimBackpack = this.GetComponentInChildren<Animation>();
        
	}
	
	// Update is called once per frame
	void Update () {

        float InputV = myUnitState.moveDir.z;
        float InputH = myUnitState.moveDir.x;

        myAnim.SetFloat("InputV", InputV);
        myAnim.SetFloat("InputH", InputH);

        myAnim.SetBool("EndSkill", endSkillAtk);
        

        if (myUnitState.moveDir==Vector3.zero)
        {
            myAnim.SetBool("Moving", false);
            aniTime += Time.deltaTime;
        }
        else
        {
            myAnim.SetBool("Moving", true);
            aniTime = 0.0f;
        }
        if (aniTime < 0.2f)
        {
            if (!myAnimBackpack.IsPlaying("Open"))
                myAnimBackpack.Play("Open");
        }
        else
        {
            if (!myAnimBackpack.IsPlaying("Close"))
            {
                //Debug.Log("close");
                myAnimBackpack.Play("Close");
            }
        }

#if UNITY_STANDALONE || UNITY_EDITOR
        if (Input.GetMouseButton(0))
            rifleAtk = true;
        else
            rifleAtk = false;

        if (Input.GetMouseButton(1))
        {
            //myAnim.SetBool("MeleeAttack", true);
            //this.meleeAtk = true;
            laserAtk = true;
           // myAnim.SetBool("MultiLaser", true);
        }
        else
        {
            laserAtk = false;
            //myAnim.SetBool("MeleeAttack", false);
            //myAnim.SetBool("MultiLaser", false);

        }
#endif

        for (int i = 0; i < this.myAnim.layerCount; i++)
            this.animatorStateInfos[i] = this.myAnim.GetCurrentAnimatorStateInfo(i);

        if (rifleAtk && this.animatorStateInfos[0].IsName("Atk_Spear_RH") == false)
            myAnim.SetBool("RangeAttack", true);
        else
            myAnim.SetBool("RangeAttack", false);

            myAnim.SetBool("MultiLaser", laserAtk);


        if (this.animatorStateInfos[0].IsName("Atk_Spear_RH") == false)
            this.meleeAtk = false;
	}

    public void FinisherExit()
    {
        myAnim.SetTrigger("EndSkillExit");

    }
}
