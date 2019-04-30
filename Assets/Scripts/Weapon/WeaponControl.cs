using UnityEngine;
using System.Collections;

public class WeaponControl : MonoBehaviour {

    private UnitState myUnitstate;
    public UIButton RifleBtn;
    public bool lockOn = false;
	// Use this for initialization
	void Awake () {
        myUnitstate = this.GetComponent<UnitState>();
	}

	// Update is called once per frame
	void Update () {

        if (myUnitstate.targetUnit != null)
        {
            if (!lockOn)
            {
                //homingLaser.targetObj = myUnitstate.targetUnit;
                lockOn = true;
            }
        }
        else
        {
            lockOn = false;
        }

	}
}
