using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour {

    private static EffectManager sInstance;
    public static EffectManager Instance
    {
        get
        {
            return sInstance;
        }
    }

    public GameObject BeamHitPrefab;
    public GameObject ExplosionPrefab;

    public List<GameObject> effects_hit;
    public List<GameObject> effects_exp;
    private int hitEffectNum = 20;
    private int expEffectNum = 10;
	// Use this for initialization
	void Awake () {
        sInstance = this;
	}

    void Start()
    {
        for (int i = 0; i < hitEffectNum; i++)
        {
            GameObject newEffect = Instantiate(BeamHitPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            newEffect.transform.SetParent(this.transform);
            newEffect.SetActive(false);

            effects_hit.Add(newEffect);
        }
        for (int i = 0; i < expEffectNum; i++)
        {
            GameObject newEffect = Instantiate(ExplosionPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            newEffect.transform.SetParent(this.transform);
            newEffect.SetActive(false);

            effects_exp.Add(newEffect);
        }

    }

    public void GetEffect(Vector3 pos , int type)
    {
        if (type == 0)
        {
            for (int i = 0; i < hitEffectNum; i++)
            {
                if (effects_hit[i].activeSelf == false)
                {
                    effects_hit[i].transform.position = pos;
                    effects_hit[i].SetActive(true);
                    return;
                }
            }
        }
        else if (type == 1)
        {
            for (int i = 0; i < expEffectNum; i++)
            {
                if (effects_exp[i].activeSelf == false)
                {
                    effects_exp[i].transform.position = pos;
                    effects_exp[i].SetActive(true);
                    return;
                }
            }
        }
    }
}
