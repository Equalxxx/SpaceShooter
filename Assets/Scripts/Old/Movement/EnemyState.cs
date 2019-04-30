using UnityEngine;
using System.Collections;

public class EnemyState : MonoBehaviour {

    public float health = 15.0f;
    public float maxHealth = 15.0f;
    private Animator myAnim;

    void Awake()
    {
        myAnim = this.transform.GetChild(0).GetComponent<Animator>();

    }
    void Update()
    {
        if (health < 0.0f)
            Destroy(this.gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerAtk" )
        {
            health -= 5.0f;
            myAnim.SetTrigger("Damaged");
        }
        if (other.tag == "Bullet")
        {
            health -= 1.0f;
            myAnim.SetTrigger("Damaged");
        }
    }
}
