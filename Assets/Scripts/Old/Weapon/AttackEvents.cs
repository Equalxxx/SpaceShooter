using UnityEngine;
using System.Collections;

public class AttackEvents : MonoBehaviour {

    public Collider attackCollider;

    public bool useAttack = false;

    void Attack_Slash()
    {
        useAttack = true;
        attackCollider.enabled = true;
        StartCoroutine(AttackStop());

    }

    void Attack_Thrust()
    {
        useAttack = true;
        
        attackCollider.enabled = true;

        StartCoroutine(AttackStop());
    }

    IEnumerator AttackStop()
    {
        yield return new WaitForSeconds(0.5f);
        useAttack = false;
        attackCollider.enabled = false;
    }
}
