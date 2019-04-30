using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MultiTarget : MonoBehaviour {

    public List<Transform> targetsTrans;
    public GameObject[] allEnemy;
    public List<float> angles;
    public bool targetSearch = false;
	// Use this for initialization
	void Start () {
        allEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < allEnemy.Length; i++)
        {
            angles.Add(0.0f);
        }

        StartCoroutine(OnTarget());
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            targetSearch = true;
        }
        else
        {
            
            targetSearch = false;
        }

        for (int i = 0; i < angles.Count; i++)
        {
            Vector3 dirToTarget = allEnemy[i].transform.position - this.transform.position;
            dirToTarget.Normalize();
            angles[i] = Vector3.Angle(Camera.main.transform.forward,dirToTarget);
        }
	}

    IEnumerator OnTarget()
    {
        while (true)
        {
            if (targetSearch)
            {
                for (int i = 0; i < angles.Count; i++)
                {
                    // Debug.Log("lockon");
                    if (!targetSearch)
                        continue;
                    float angle = angles[i];
                    if (angle < 35.0f)
                    {
                        allEnemy[i].SendMessage("OnTarget", SendMessageOptions.DontRequireReceiver);
                        if (targetsTrans.Contains(allEnemy[i].transform) == false)
                            targetsTrans.Add(allEnemy[i].transform);
                    }
                    yield return new WaitForSeconds(0.3f);
                    
                }
            }
            else
            {
                if (targetsTrans.Count > 0)
                {
                    for (int i = 0; i < targetsTrans.Count; i++)
                    {
                        targetsTrans[i].gameObject.SendMessage("OffTarget");
                    }
                    targetsTrans.Clear();
                }
                yield return null;
            }
        }
    }
}
