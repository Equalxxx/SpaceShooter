using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public float health = 0.0f;
    public float healthMax = 30.0f;
    public UISprite myTargetSprite;
    private Vector3 StartPos;
    private EnemySquad eSquad;
    public int type = 0;
    void Awake()
    {
        StartPos = this.transform.position;
        eSquad = this.transform.root.GetComponent<EnemySquad>();
    }
    void OnEnable()
    {
        health = healthMax;
        this.transform.position = StartPos;
    }
    void OnDisable()
    {
        if(myTargetSprite!=null)
            myTargetSprite.color = Color.white;
    }
    void Havedamage(float dmg)
    {
        health -= dmg;

        if (health <= 0.0f)
        {
            this.gameObject.SetActive(false);
            if(this.gameObject.layer==LayerMask.NameToLayer("Enemys"))
                eSquad.UnitDestroy(this.gameObject, type);
            EffectManager.Instance.GetEffect(this.transform.position, 1);
        }
    }

    void OnTarget()
    {
        //this.renderer.material.color = Color.blue;
        myTargetSprite.color = Color.red;
    }
    void OffTarget()
    {
        //this.renderer.material.color = Color.red;
        myTargetSprite.color = Color.white;
    }
}
