using UnityEngine;
using System.Collections;

public class EnemyAnimControl : MonoBehaviour {

    private Animator myAnim;
    private UnitState myUnit;
    public float moveSpeed = 0.0f;
    public bool atkStart = false;
    public Animation backpackAnim;
    public int backpackType = 0;
	// Use this for initialization
	void Awake () {
        myAnim = this.GetComponentInChildren<Animator>();
        myUnit = this.GetComponent<UnitState>();
        backpackAnim = this.GetComponentInChildren<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 moveDir = myUnit.moveDir;
        float inputV = moveDir.z;
        float inputH = moveDir.x * moveSpeed;

        myAnim.SetFloat("InputV", inputV);
        myAnim.SetFloat("InputH", inputH);

        if (backpackAnim != null)
        {
            if (backpackType == 0)
            {
                if (inputV > 0.1f || inputV < -0.1f || inputH > 0.1f || inputH < -0.1f)
                {
                    backpackAnim.Play("Open");
                }
                else
                    backpackAnim.Play("Close");
            }
        }
        if (atkStart)
        {
            myAnim.SetBool("RifleAttack", true);
        }
        else
            myAnim.SetBool("RifleAttack", false);

        
	}
}
