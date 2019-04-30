using UnityEngine;
using System.Collections;

public class MyHealth : MonoBehaviour {

    public float health = 0.0f;
    public float healthMax = 150.0f;
    public UISlider myHpBar;

	// Use this for initialization
	void Start () {
        health = healthMax;

	}

    void Havedamage(float dmg)
    {
        health -= dmg;
        ShakeTransform.Instance.ShakeRotOrder(1.5f, 0.25f);
        if (health <= 0.0f)
        {
            this.gameObject.SetActive(false);
            health = 0.0f;
            ExitGame.Instance.EndGame(1);
            EffectManager.Instance.GetEffect(this.transform.position, 1);
        }
        myHpBar.value = health/healthMax;
    }
}
