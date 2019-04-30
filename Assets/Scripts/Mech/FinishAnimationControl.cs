using UnityEngine;
using System.Collections;

public class FinishAnimationControl : MonoBehaviour {

    public bool finish = false;
    private AnimatorControl myAnim;
    private Animator myAnimator;
    public float time = 0.0f;
	// Use this for initialization
	void Awake () {
        myAnim = this.transform.root.GetComponent<AnimatorControl>();
        myAnimator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(UseFinisher());
        }

        myAnim.endSkillAtk = finish;
	}

    IEnumerator UseFinisher()
    {
        FadeInOut.Instance.SetFade(false);
        yield return new WaitForSeconds(1.2f);
        FadeInOut.Instance.SetFade(true);

        myAnimator.SetBool("PlayFinisher", true);

        yield return new WaitForSeconds(3.0f);

        while (true)
        {
            if (finish == false)
            {
                FadeInOut.Instance.SetFade(false);
                myAnimator.SetBool("PlayFinisher", false);

                yield return new WaitForSeconds(1.2f);
                FadeInOut.Instance.SetFade(true);

                yield break;
            }
            else
                yield return null;
        }
    }
}
