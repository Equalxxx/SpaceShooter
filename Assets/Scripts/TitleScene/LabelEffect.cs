using UnityEngine;
using System.Collections;

public class LabelEffect : MonoBehaviour {

    private UILabel effectLabel;
    private bool colorChange = false;
    public bool startEffect = false;
	// Use this for initialization
	void Awake () {
        effectLabel = this.GetComponent<UILabel>();

	}
	
	// Update is called once per frame
	void Update () {
        if (FadeInOut.Instance.finish == true)
        {
            startEffect = true;
        }

        if (startEffect == true && FadeInOut.Instance.finish)
        {
            Color oldColor = effectLabel.color;
            if (colorChange)
            {
                if (effectLabel.color.a > 0.0f)
                    oldColor.a -= Time.deltaTime;
                else
                    colorChange = false;
            }
            else
            {
                if (effectLabel.color.a < 1.0f)
                    oldColor.a += Time.deltaTime;
                else
                    colorChange = true;
            }
            Mathf.Clamp01(oldColor.a);
            effectLabel.color = oldColor;
        }
	}
}
