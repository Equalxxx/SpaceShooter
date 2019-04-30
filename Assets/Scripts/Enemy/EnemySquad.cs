using UnityEngine;
using System.Collections;

public class EnemySquad : MonoBehaviour {

    public UnitState[] enemys;
    public UILabel countLabel;
    private bool battleStart = false;
    public int deathCount = 0;
    public int maxDeathCount = 30;
    public bool bossDestroy = false;
    public int bossStartCount = 5;
    public float genTime = 10.0f;
    private bool bossStart = false;
    public bool finish = false;
	// Use this for initialization
	void Awake () {

        enemys = this.GetComponentsInChildren<UnitState>();
        countLabel.text = deathCount.ToString();
	}

    void Start()
    {
        for (int i = 0; i < enemys.Length; i++)
        {
            EnemyAI aiType = enemys[i].gameObject.GetComponent<EnemyAI>();

            if (aiType.enemyType != 0)
            {
                enemys[i].gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (battleStart == true)
            return;

        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].gameObject.activeSelf == false)
                continue;

            if (enemys[i].targetUnit == null)
            {
                enemys[i].myUnitInfo.moveSpeed = 0.5f;
                enemys[i].Movement(new Vector3(0, 0, 1.0f));
            }
            else
            {
                enemys[i].myUnitInfo.moveSpeed = 3.0f;

                battleStart = true;
            }
        }
        
	}

    public void UnitDestroy(GameObject enemyObj,int type)
    {
        if (type == 1)
            bossDestroy = true;
        
        deathCount++;

        if (deathCount >= maxDeathCount || bossDestroy==true)
        {
            countLabel.text = "Finish!";
            finish = true;
            RotateCore.Instance.onFinish = true;
            return;
        }
        float ed = deathCount;
        float maxed = maxDeathCount;
        float resEd = ed / maxed;
        Debug.Log(resEd.ToString());
        RotateCore.Instance.AddAngleColor(resEd * 720.0f);
        countLabel.text = deathCount.ToString();
        StartCoroutine(Revive(enemyObj));
        if (deathCount > bossStartCount && bossStart == false)
        {
            for (int i = 0; i < enemys.Length; i++)
            {
                EnemyAI aiType = enemys[i].gameObject.GetComponent<EnemyAI>();

                if (aiType.enemyType != 0)
                {
                    enemys[i].gameObject.SetActive(true);
                }
            }
            bossStart = true;
        }
    }

    IEnumerator Revive(GameObject enemyObj)
    {
        yield return new WaitForSeconds(genTime);
        enemyObj.SetActive(true);
    }
}
