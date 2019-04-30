using UnityEngine;
using System.Collections;

[System.Serializable]
public class UnitInfo
{
    public float moveSpeed = 3.0f;
    public float dashSpeed = 1.0f;
    public float dashMaxSpeed = 10.0f;
    
    public float AttackPower = 1.0f;
    public float DefendPower = 1.0f;
    public float rotateSpeed = 90.0f;
    
}

public class UnitState : MonoBehaviour
{
    public UnitForce force = 0;

    public UnitInfo myUnitInfo;
    public GameObject targetUnit;
    private GameObject targetDummy;
    public GameObject targetBase;

    private Transform myTransform;
    private CharacterController myController;

    private bool useDash = false;

    public float dashMaxDelay = 1.0f;
    public float dashDelay = 1.0f;
    
    public bool followTarget = false;

    public float targetAngle = 0.0f;

    public Vector3 moveDir = Vector3.zero;
    private Vector3 targetHidePos = Vector3.zero;


    public  Vector3 dirToTarget;

    public bool hideDummy = false;
    public LayerMask layerMask;

    public bool target = false;

    private AudioClip myClip;

    public bool useFinisher = false;
    // Use this for initialization
    void Awake()
    {
        myUnitInfo = new UnitInfo();
        myController = this.GetComponent<CharacterController>();
        myTransform = this.transform;

        if (this.gameObject.layer != LayerMask.NameToLayer("Enemys"))
        {
            GameObject targets = GameObject.FindGameObjectWithTag("Target");
            targetBase = targets;
        }
        if (this.gameObject.tag == "Player")
        {
            myClip = Resources.Load("EffectSound/" + "Boost") as AudioClip;
            this.GetComponent<AudioSource>().clip = myClip;
        }
        UnitManager.Instance.AddUnit(this);
        
    }
    void Start()
    {
        //UnitManager.Instance.AddUnit(this);
    }
    void OnEnable()
    {
        target = false;
    }

    void TargetEnable()
    {
        target = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetUnit != null)
        {
            followTarget = true;
            //distToTarget = Vector3.Distance(targetUnit.transform.position, this.myTransform.position);
        }
        else
        {
            followTarget = false;
            //distToTarget = 0.0f;
        }
        RotateUnit();
#if UNITY_STANDALONE || UNITY_EDITOR
        if (this.gameObject.CompareTag("Player") && Input.GetMouseButtonDown(0))
#elif UNITY_ANDROID || UNITY_IOS
        if (this.gameObject.tag == "Player" && Input.touchCount > 0 && TouchRayCast.Instance.blocking)
#endif
        {
            Debug.Log("hide");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 50.0f,layerMask.value))
            {
                if (hit.collider.CompareTag("Dummy"))
                {
                    targetDummy = hit.collider.gameObject;

                    hideDummy = true;
                }
            }
        }

        if (hideDummy)
        {
            Dash();
            Vector3 dirToDummy = Vector3.zero;
            Vector3 targetPos = Vector3.zero;

            if (targetUnit != null)
            {
                targetPos = this.targetDummy.transform.position - this.targetUnit.transform.position;
                targetPos.Normalize();
                targetHidePos = this.targetDummy.transform.position + (targetPos * 3.0f);
                Debug.DrawLine(targetDummy.transform.position, targetHidePos);
                dirToDummy = targetHidePos - this.myTransform.position;
            }
            else
                dirToDummy = this.targetDummy.transform.position - this.myTransform.position;

            Ray rayToDummy = new Ray(this.myTransform.position, dirToDummy);
            Debug.DrawRay(rayToDummy.origin, rayToDummy.direction,Color.red);
            if (Physics.Raycast(rayToDummy, 3.0f, layerMask))
            {
                hideDummy = false;
                myUnitInfo.dashSpeed = 1.0f;
            }
            Movement(dirToDummy);
        }
        //float vertical = Input.GetAxis("Vertical");
        //float horizontal = Input.GetAxis("Horizontal");

        //moveDir = new Vector3(horizontal, 0, vertical);

        //this.myTransform.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);

        //MoveToTarget();
    }
    public void Movement(Vector3 movedir)
    {
        if (hideDummy)
        {
            float distDummy = Vector3.Distance(this.myTransform.position, this.targetHidePos);

            if (distDummy < 1.0f)
            {
                hideDummy = false;
                myUnitInfo.dashSpeed = 1.0f;

            }
        }
        if (useDash)
        {
            myUnitInfo.dashSpeed -= Time.deltaTime * 10.0f;


            if (myUnitInfo.dashSpeed < 1.0f)
                myUnitInfo.dashSpeed = 1.0f;

            if (dashDelay < dashMaxDelay)
                dashDelay += Time.deltaTime;
            else
            {
                useDash = false;
                dashDelay = dashMaxDelay;
            }
        }

        moveDir = movedir;

        if (moveDir != Vector3.zero)
        {
            moveDir.Normalize();
            Vector3 moveDelta = moveDir * Time.deltaTime * myUnitInfo.moveSpeed * myUnitInfo.dashSpeed;
            if (!hideDummy)
                moveDelta = this.myTransform.TransformDirection(moveDelta);

            myController.Move(moveDelta);
        }

    }
    void RotateUnit()
    {
        Quaternion lookRotate = Quaternion.identity;
        if (followTarget && useFinisher == false)
        {
            if (targetUnit != null)
            {
                Vector3 dirToTarget = this.targetUnit.transform.position - this.myTransform.position;
                dirToTarget.Normalize();

                lookRotate = Quaternion.LookRotation(dirToTarget, Vector3.up);
            }
        }
        else
        {
            if (this.targetBase == null)
                return;

            Vector3 dirToTarget = this.targetBase.transform.position - this.myTransform.position;
            dirToTarget.Normalize();
            lookRotate = Quaternion.LookRotation(dirToTarget, Vector3.up);
        }
        float distAngle = Quaternion.Angle(lookRotate, this.myTransform.rotation);
        this.targetAngle = distAngle;
        float deltaRot = this.myUnitInfo.rotateSpeed * Time.deltaTime;
        
        float t = Mathf.Clamp01(deltaRot / distAngle);
        this.transform.rotation = Quaternion.Slerp(this.myTransform.rotation, lookRotate, t);

    }

    public void Dash()
    {
        if(!hideDummy)
            if (dashDelay < dashMaxDelay)
                return;

        useDash = true;
        if (this.gameObject.tag == "Player")
        {
            if(GetComponent<AudioSource>().isPlaying==false)
                GetComponent<AudioSource>().Play();
        }
        myUnitInfo.dashSpeed = myUnitInfo.dashMaxSpeed;
        dashDelay = 0.0f;
    }

    void OnDestroy()
    {
        if (UnitManager.IsOpen)
        {
            UnitManager.Instance.RemoveUnit(this);
        }

    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawLine(this.transform.position,this.transform.forward*100.0f);
    }

}
