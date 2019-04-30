using UnityEngine;
using System.Collections;

public class LockOnUIManager : MonoBehaviour {

    private TargetLockOnUI[] LockOnObj;
    private AudioClip myClip;
	// Use this for initialization

    void Awake()
    {
        LockOnObj = this.GetComponentsInChildren<TargetLockOnUI>();
        myClip = Resources.Load("EffectSound/" + "Swipe") as AudioClip;
    }
	void Start () {
        
        for (int i = 0; i < LockOnObj.Length; i++)
        {
            LockOnObj[i].gameObject.SetActive(false);
        }
	}

    public void EnableTargetUI(string targetName)
    {
        for (int i = 0; i < LockOnObj.Length; i++)
        {
            if (targetName == LockOnObj[i].targetGameObject.name)
            {
                if (LockOnObj[i].gameObject.activeSelf == false)
                {
                    GetComponent<AudioSource>().PlayOneShot(this.myClip);
                    LockOnObj[i].gameObject.SetActive(true);
                    LockOnObj[i].SendMessage("PlayTween");
                }
            }
        }
    }
}
