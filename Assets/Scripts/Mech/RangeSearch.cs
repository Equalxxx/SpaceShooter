using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangeSearch : MonoBehaviour,IComparer<Transform> {

    public float searchSeconds = 1.0f;
    public float searchRangeBase = 10.0f;
    public float searchRangeNow = 0.0f;
    public float addRange = 3.0f;
    private float rangeRefreshTime = 1.0f;

    public LayerMask layerMask;
    public List<Transform> searchEnemys;
    private UnitState myUnitState;
    public LockOnUIManager targetUI;
    public int selectTargetNum = 0;
    private UnitForce enemyForce;
    public AimTargetUI aimUI;
    //
    // 기본 범위 안에 들어온 적들은 무조건 ADD
    // 그 이상의 거리에 있는 적들은 기본 범위 내 적들이 모두 사라졌을때 addRange로 범위를 증가시킴
    // 타겟을 잡았을때 다시 기본 범위로 되돌림 
    // -> 반복
    //

	// Use this for initialization
	void Awake () {
        myUnitState = this.GetComponent<UnitState>();
        if (this.gameObject.layer == LayerMask.NameToLayer("RED"))
        {
            this.layerMask = 1 << LayerMask.NameToLayer("BLUE");
        }
        if (this.gameObject.layer == LayerMask.NameToLayer("BLUE"))
        {
            this.layerMask = 1 << LayerMask.NameToLayer("RED");
        }

        searchRangeNow = searchRangeBase;

        if (myUnitState.force == UnitForce.Force_Team)
            enemyForce = UnitForce.Force_Enemy;
        else
            enemyForce = UnitForce.Force_Team;

	}

    //void Start()
    //{
    //    StartCoroutine(SearchTarget());

    //    //int eCount = UnitManager.Instance.GetEnemyCount(UnitForce.Force_Enemy);
    //    //Debug.Log(eCount.ToString());
    //}
    void OnEnable()
    {
        StartCoroutine(SearchTarget());
    }
    void OnDisable()
    {
        StopCoroutine(SearchTarget());
    }

	// Update is called once per frame
	void Update () {
        if (myUnitState.targetUnit != null && myUnitState.targetUnit.activeSelf==false)
        {
            myUnitState.targetUnit = null;
            Debug.Log("Target Lost!");
        }

	}

    public void SelectTarget()
    {
        if (searchEnemys.Count > 0)
        {
            selectTargetNum++;
            if (selectTargetNum >= searchEnemys.Count)
                selectTargetNum = 0;

            myUnitState.targetUnit = searchEnemys[selectTargetNum].gameObject;
            aimUI.TargetChange(searchEnemys[selectTargetNum].gameObject);
            Debug.Log("adad");
        }
    }

    IEnumerator SearchTarget()
    {
        while (true)
        {
            searchEnemys.Clear();
            Collider[] hitCols;
            hitCols = Physics.OverlapSphere(this.transform.position, searchRangeNow, layerMask);

            //충돌된 적이 있다.
            if (hitCols.Length > 0)
            {
                for (int i = 0; i < hitCols.Length; i++)
                {
                    if (hitCols[i].CompareTag("Bullet"))
                        continue;

                    //이미 있는 적이라면 재낌
                    if (searchEnemys.Contains(hitCols[i].transform) == false)
                    {
                        searchEnemys.Add(hitCols[i].transform);

                        //Player만 적용
                        if (this.gameObject.CompareTag("Player"))
                        {
                            targetUI.EnableTargetUI(hitCols[i].gameObject.name);
                            hitCols[i].gameObject.SendMessage("TargetEnable",SendMessageOptions.DontRequireReceiver);
                        }
                    }
                }
            }
           
            //나와 가까운 적 순서로 sort
            searchEnemys.Sort(this);

            //if (myUnitState.targetUnit == null && searchEnemys.Count>0)
            if (searchEnemys.Count > 0)
            {
                if (this.gameObject.CompareTag("Player") == false)
                {
                    for (int i = 0; i < searchEnemys.Count; i++)
                    {
                        if (searchEnemys[i].gameObject.activeSelf == true)
                        {
                            myUnitState.targetUnit = searchEnemys[i].gameObject;
                            break;
                        }
                    }
                }
                else
                {
                    if (myUnitState.targetUnit == null)
                    {
                        for (int i = 0; i < searchEnemys.Count; i++)
                        {
                            if (searchEnemys[i].gameObject.activeSelf == true)
                            {
                                myUnitState.targetUnit = searchEnemys[i].gameObject;
                                aimUI.TargetChange(searchEnemys[i].gameObject);
                                break;
                            }
                        }
                    }
                }
            }

            //Debug.Log("search...");
            yield return new WaitForSeconds(rangeRefreshTime);

            searchRangeNow += addRange;

            //발견된 적이 없을때 범위 증가
            //if (searchEnemys.Count == 0)
            //{
            //    searchRangeNow += addRange;
            //}
            //else
            //{
            //    // 발견된 적이 있을때 범위 고정
            //    //searchRangeNow = searchRangeBase;
            //}

        }
        //yield return null;
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(this.transform.position, this.searchRangeNow);
    }

    public int Compare(Transform x, Transform y)
    {
        
        float distX = Vector3.Distance(this.transform.position, x.position);
        float distY = Vector3.Distance(this.transform.position, y.position);

        if (distX < distY)
            return -1;
        else if (distX > distY)
            return 1;
        return 0;
    }
}
