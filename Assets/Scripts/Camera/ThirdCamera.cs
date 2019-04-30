using UnityEngine;
using System.Collections;

public class ThirdCamera : MonoBehaviour {

    public Vector3 viewDir = Vector3.zero;

    private UnitState myUnitState;
    private Transform myTransform;
    public float camSideSpeed = 5.0f;
    public float camFollowSpeed = 5.0f;
    public float distRoot = 0.6f;

    public float moveSpeed = 5.0f;
    public bool useThirdCam = true;
	// Use this for initialization
	void Awake () {
        myTransform = this.transform;
        myUnitState = this.transform.root.GetComponent<UnitState>();
	}
	
	// Update is called once per frame
	void Update () {
        

        // -1.0f ~ 1.0f base
        // 0.5f ~ -0.5f inverse
        if (useThirdCam == true)
        {
            float horizontal = myUnitState.moveDir.x;
            float vertical = myUnitState.moveDir.z;

            float posAmountX = -(horizontal * distRoot);
            float posAmountZ = vertical * 0.5f;

            if (posAmountZ < 0.0f)
                posAmountZ = 0.0f;
            viewDir = new Vector3(posAmountX, 0.65f, -posAmountZ - 1.5f);
            this.myTransform.localPosition = Vector3.Lerp(this.myTransform.localPosition, viewDir, Time.deltaTime * camSideSpeed);

            Vector3 lookDir = new Vector3(0.0f, posAmountX, 0.0f);
            Quaternion lookQuat = Quaternion.Euler(lookDir * -10.0f);
            this.myTransform.localRotation = Quaternion.Slerp(this.myTransform.localRotation, lookQuat, Time.deltaTime * camSideSpeed);
        }
        else
            this.myTransform.localPosition = new Vector3(0.0f, 0.65f, -1.5f);


	}
}
