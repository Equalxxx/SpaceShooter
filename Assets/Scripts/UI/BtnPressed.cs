using UnityEngine;
using System.Collections;

public class BtnPressed : MonoBehaviour {

    public bool btnPress = false;
    private UseBeamRifle beamRifle;
    private MultiTargetLaser multiLaser;
    private UnitState myUnitState;
    private AnimatorControl myAnim;
    private UISprite ForeSprite;
    public int btnType = 0;
    
    private 
    void Awake()
    {
        myAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<AnimatorControl>();
        ForeSprite = this.transform.Find("Foreground").GetComponent<UISprite>();
        
        if (btnType == 0)
            beamRifle = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<UseBeamRifle>();
        else if (btnType == 1)
            multiLaser = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<MultiTargetLaser>();
        else
            myUnitState = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<UnitState>();
    }

	// Update is called once per frame
	void Update () {
        if (btnType == 0)
        {
            if (btnPress)
                beamRifle.Shoot();
            myAnim.rifleAtk = btnPress;
        }
        else if (btnType == 1)
        {
            ForeSprite.fillAmount = multiLaser.delayTime / multiLaser.delayMaxTime;
        }
        else
            ForeSprite.fillAmount = myUnitState.dashDelay / myUnitState.dashMaxDelay;
	}

    void OnPress(bool press)
    {
        btnPress = press;

        if (btnType == 1)
        {
            multiLaser.Shoot(press);
            myAnim.laserAtk = press;
        }
    }
}
