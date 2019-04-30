using UnityEngine;
using System.Collections;

public class TitleSceneControl : MonoBehaviour {

    private bool gameStart = false;

    void Start()
    {
        BGMManager.Instance.PlayBGM("Title");
    }
	void Update () {

#if UNITY_STANDALONE || UNITY_EDITOR
        if (FadeInOut.Instance.finish == true && Input.GetKeyDown(KeyCode.Return))
        {
            gameStart = true;
            FadeInOut.Instance.SetFade(false);
        }
#elif UNITY_ANDROID || UNITY_IOS
        if (FadeInOut.Instance.finish == true && Input.touchCount > 0)
        {
            gameStart = true;            
        }
#endif

        if (gameStart == true)
        {
            if (FadeInOut.Instance.finish == true)
                Application.LoadLevel(1);
        }

    }
}
