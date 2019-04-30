using UnityEngine;
using System.Collections;

public class AnimControl : MonoBehaviour {

    private Animator myAnim;
    private AnimatorStateInfo[] animatorStateInfos;
    public bool meleeAtk = false;

    public Animation myAnimBackpack;

	// Use this for initialization
	void Awake () {
        myAnim = this.transform.GetChild(0).GetComponent<Animator>();
        animatorStateInfos = new AnimatorStateInfo[this.myAnim.layerCount];

	}
	
	// Update is called once per frame
    void Update()
    {
        float InputV = Input.GetAxis("Vertical");
        float InputH = Input.GetAxis("Horizontal");

        myAnim.SetFloat("InputV", InputV);
        myAnim.SetFloat("InputH", InputH);

        if (Mathf.Approximately(InputV, 0.0f) && Mathf.Approximately(InputH, 0.0f))
        {
            myAnim.SetBool("Moving", false);
            if (!myAnimBackpack.IsPlaying("Close"))
                myAnimBackpack.Play("Close");
        }
        else
        {
            myAnim.SetBool("Moving", true);
            if(!myAnimBackpack.IsPlaying("Open"))
                 myAnimBackpack.Play("Open");
        }

        for (int i = 0; i < this.myAnim.layerCount; i++)
            this.animatorStateInfos[i] = this.myAnim.GetCurrentAnimatorStateInfo(i);

        if (Input.GetMouseButton(0) && this.animatorStateInfos[0].IsName("Atk_Spear_RH") == false)
            myAnim.SetBool("RangeAttack", true);
        else
            myAnim.SetBool("RangeAttack", false);

        if (Input.GetMouseButton(1))
        {
            myAnim.SetBool("MeleeAttack", true);
            this.meleeAtk = true;
        }
        else
        {
            myAnim.SetBool("MeleeAttack", false);
        }

        if (this.animatorStateInfos[0].IsName("Atk_Spear_RH") == false)
            this.meleeAtk = false;
    }   
}
