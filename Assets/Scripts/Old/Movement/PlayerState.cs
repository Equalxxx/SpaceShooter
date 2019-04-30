using UnityEngine;
using System.Collections;

public class PlayerState : MonoBehaviour {

    private Animator myAnim;

    void Awake()
    {
        myAnim = this.transform.GetChild(0).GetComponent<Animator>();

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyAtk")
        {
            myAnim.SetTrigger("Damaged");
        }
    }
}
