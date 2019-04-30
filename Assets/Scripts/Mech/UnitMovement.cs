using UnityEngine;
using System.Collections;
public class UnitMovement : MonoBehaviour {

    private UnitState myUnitState;
    public TouchPad touchPad;
    private bool useMove = true;

	// Use this for initialization
	void Awake () {
        myUnitState = this.GetComponent<UnitState>();
	}
	
	// Update is called once per frame
	void Update () {
        if (useMove == false)
        {
            Vector3 dir = new Vector3(0, 0, 0);
            myUnitState.Movement(dir);
            return;
        }
        if (!myUnitState.hideDummy)
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            float inputV = Input.GetAxis("Vertical");
            float inputH = Input.GetAxis("Horizontal");
            Vector3 moveDir = new Vector3(inputH, 0.0f, inputV);

#elif UNITY_ANDROID || UNITY_IOS
            Vector3 moveDir = touchPad.touchDir;

#endif
            myUnitState.Movement(moveDir);

            if (Input.GetKeyDown(KeyCode.LeftShift))
                myUnitState.Dash();
        }
	}

    public void SetMove(bool b)
    {
        useMove = b;
    }
}
