using UnityEngine;
using System.Collections;

public class ShipInfo
{
    public float moveSpeed=0.5f;
    public float health = 550;
}
public class Spaceship : MonoBehaviour {

    private ShipInfo myShipInfo;
    private Transform myTrans;
    public EnemySquad es;
    public bool emptyEnemys = false;
    void Awake()
    {
        myShipInfo = new ShipInfo();
        myTrans = this.transform;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float moveDelta = Time.deltaTime*myShipInfo.moveSpeed;
        this.myTrans.Translate(0.0f, 0.0f, moveDelta);
        //if (emptyEnemys == false && es.deathCount >= es.maxDeathCount)
        if(emptyEnemys==false && es.finish==true)
        {
            myShipInfo.health = 50;
            emptyEnemys = true;
        }
	}

    public void Havedamage(float dmg)
    {
        Debug.Log("damage!"+myShipInfo.health.ToString());
        myShipInfo.health -= dmg;
        if (myShipInfo.health < 0.0f)
        {
            Debug.Log("Destroy!");
            this.gameObject.SetActive(false);
            ExitGame.Instance.EndGame(0);
            EffectManager.Instance.GetEffect(this.transform.position, 1);
        }
    }
}
