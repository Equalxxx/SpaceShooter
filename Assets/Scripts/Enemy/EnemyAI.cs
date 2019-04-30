using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour {

    private UnitState myUnit;
    private EnemyAnimControl myAnim;
    private UseBeamRifle beamRifle;
    private EnemyHealth myHealth;

    private float evadeSpeed = 0.0f;
    private Vector3 evadeDir = Vector3.zero;
    private Vector3 teamFormDir = Vector3.zero;
    private float evadeMinimumTime = 1.0f;
    private float evadeMaximumTime = 2.5f;
    private float evadeRandomSpeed = 3.0f;
    private float targetAimDist = 20.0f;
    private bool rangeInPlayer = false;
    public bool attackStart = false;

    public int enemyType = 0;
    public bool spAttackStart = false;
    private int randSkill = 0;
	// Use this for initialization
	void Awake () {

        myUnit = this.GetComponent<UnitState>();
        myAnim = this.GetComponent<EnemyAnimControl>();
        beamRifle = this.GetComponentInChildren<UseBeamRifle>();
        myHealth = this.GetComponent<EnemyHealth>();

        if (enemyType == 1)
            randSkill = 2;
        if (enemyType == 2)
            randSkill = 1;
	}
    void Start()
    {
        myUnit.myUnitInfo.dashMaxSpeed = 5.0f;
    }
    void OnEnable()
    {
        StartCoroutine("SetEvade");
        StartCoroutine("AttackToTarget");
        StartCoroutine("TeamDist");
        StartCoroutine("UseSpecialAttack");
    }
    void OnDisable()
    {
        StopCoroutine("SetEvade");
        StopCoroutine("AttackToTarget");
        StopCoroutine("TeamDist");
        StopCoroutine("UseSpecialAttack");
    }

	// Update is called once per frame
	void Update () {

        if (myUnit.targetUnit != null)
        {
            RangeAttack(myUnit.targetUnit.transform.position);
        }
        else
        {
            if (myUnit.targetBase != null && myUnit.targetBase.activeSelf==true)
            {
                RangeAttack(myUnit.targetBase.transform.position);
            }
            else
                attackStart = false;
        }
        if (myAnim.atkStart)
        {
            beamRifle.Shoot();
        }

        if (enemyType == 1)
        {
            float per = myHealth.health/myHealth.healthMax;
            if (per < 0.95f)
                spAttackStart = true;
        }
        else if (enemyType == 2)
        {
            float per = myHealth.health / myHealth.healthMax;
            if (per < 0.95f)
                spAttackStart = true;
        }

	}

    void RangeAttack(Vector3 targetPos)
    {
        float targetDist = Vector3.Distance(this.transform.position, targetPos);

        if (myUnit.targetAngle < 30.0f)
        {
            Vector3 moveDir = Vector3.zero;
            if (targetDist > targetAimDist)
            {
                myUnit.myUnitInfo.moveSpeed = 6.0f;
                moveDir = new Vector3(0, 0, 1.0f);

                if (myUnit.targetUnit == null)
                    moveDir += teamFormDir;
                rangeInPlayer = false;
                attackStart = false;
            }
            else
            {
                if (teamFormDir == Vector3.zero)
                {
                    myUnit.myUnitInfo.moveSpeed = evadeSpeed;
                    moveDir = evadeDir;
                }
                else
                {
                    myUnit.myUnitInfo.moveSpeed = 3.0f;
                    moveDir = teamFormDir;
                }
                rangeInPlayer = true;
                attackStart = true;
            }

            moveDir.Normalize();
            myUnit.Movement(moveDir);
        }
    }
    IEnumerator UseSpecialAttack()
    {
        while (true)
        {
            if (spAttackStart)
            {
                int rand = Random.Range(0, 1);
                if (rand == 0)
                {
                    if (enemyType == 1)
                        this.transform.Find("HomingLaser(Boss)").SendMessage("ShootSpecial_1", SendMessageOptions.DontRequireReceiver);
                    if (enemyType == 2)
                        this.transform.Find("BeamRifle").SendMessage("ShootLargeBeam",SendMessageOptions.DontRequireReceiver);
                }
                yield return new WaitForSeconds(3.0f);
            }
            else
                yield return null;
        }
    }
    IEnumerator TeamDist()
    {
        while (true)
        {
            if (this.gameObject.activeSelf == false)
                yield return null;

            yield return new WaitForSeconds(0.5f);
            List<UnitState> myTeams = UnitManager.Instance.GetUnitList(this.myUnit.force);

            if (myTeams.Count > 0)
            {
                for (int i = 0; i < myTeams.Count; i++)
                {
                    if (myTeams[i].gameObject.activeSelf == false)
                        continue;

                    float dist = Vector3.Distance(this.transform.position, myTeams[i].transform.position);
                    if (dist < 5.0f)
                    {
                        Vector3 dir = this.transform.position - myTeams[i].transform.position;
                        dir.Normalize();
                        teamFormDir = dir;
                        yield return new WaitForSeconds(1.0f);
                    }
                    else
                    {
                        if (teamFormDir != Vector3.zero)
                            teamFormDir = Vector3.zero;
                        yield return null;
                    }
                }
            }

        }
    }
    IEnumerator SetEvade()
    {

        while (true)
        {
            if (rangeInPlayer)
            {
                evadeDir.x = Random.Range(-1.0f, 1.0f);

                evadeSpeed = Random.Range(1.5f, evadeRandomSpeed);
                myAnim.moveSpeed = evadeSpeed;


                float randomTime = Random.Range(evadeMinimumTime, evadeMaximumTime);

                int useDash = Random.Range(0, 2);
                if (useDash == 1)
                    myUnit.Dash();

                yield return new WaitForSeconds(randomTime);
            }
            else
            {
                evadeSpeed = 0.0f;
                yield return null;
            }
        }
    }

    IEnumerator AttackToTarget()
    {
        while (true)
        {
            if (attackStart)
            {
                myAnim.atkStart = true;
                
                yield return new WaitForSeconds(0.8f);

                myAnim.atkStart = false;

                yield return new WaitForSeconds(1.5f);
            }
            else
            {
                myAnim.atkStart = false;
                //if (this.gameObject.layer == LayerMask.NameToLayer("Enemys"))
                //    Debug.Log("at");
                yield return null;
            }
        }
    }
}
