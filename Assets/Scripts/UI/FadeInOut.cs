using UnityEngine;
using System.Collections;

public class FadeInOut : MonoBehaviour {

    private static FadeInOut sInstance;
    public static FadeInOut Instance
    {
        get
        {
            return sInstance;
        }
    }
    private UISprite fadeSprite;


    private bool fade = true;
    public bool finish = false;
    public float fadeSpeed = 0.5f;

	// Use this for initialization
	void Awake () {
        sInstance = this;
        fadeSprite = this.GetComponent<UISprite>();
	}
	
	// Update is called once per frame
	void Update () {
       Color oldColor = fadeSprite.color;
        if (fade == true)
        {
            if (oldColor.a > 0.0f)
                oldColor.a -= Time.deltaTime * fadeSpeed;
            else
                finish = true;
        }
        else
        {
            if (oldColor.a < 1.0f)
                oldColor.a += Time.deltaTime * fadeSpeed;
            else
                finish = true;
        }

        Mathf.Clamp01(oldColor.a);
        fadeSprite.color = oldColor;
	}

    public void SetFade(bool b)
    {
        fade = b;
        finish = false;
    }
}
