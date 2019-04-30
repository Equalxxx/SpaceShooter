using UnityEngine;
using System.Collections;

public class SelectStage : MonoBehaviour {
    public string sceneName = "";

    void Start()
    {
        BGMManager.Instance.CrossFadeBGM("Select");

    }
    void Update()
    {

        if (sceneName.Equals("") == false && FadeInOut.Instance.finish == true)
        {
            StageInfo.Instance.SelectSceneName(sceneName);
            Application.LoadLevel("04_LoadingScene");
        }
    }
    public void SelectStage_1()
    {
        if (FadeInOut.Instance.finish == true)
        {
            FadeInOut.Instance.SetFade(false);
            sceneName = "03_GameScene1";
        }
    }
    public void SelectStage_2()
    {
        if (FadeInOut.Instance.finish == true)
        {
            FadeInOut.Instance.SetFade(false);
            sceneName = "03_GameScene2";
        }
    }
    public void SelectStage_3()
    {
        if (FadeInOut.Instance.finish == true)
        {
            FadeInOut.Instance.SetFade(false);
            sceneName = "03_GameScene3";
        }
    }
}
