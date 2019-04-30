using UnityEngine;
using System.Collections;

public class LoadingCalculate : MonoBehaviour {

    public UILabel loadingLabel;
    private bool loadComplete = false;
    public string SceneName = "";
    public UISprite background;

	// Use this for initialization
	void Start () {
        SceneName = StageInfo.Instance.sceneName;
        StageInfo.Instance.DestroyThisInfo();

        if (SceneName.Equals("03_GameScene1") == true)
            background.spriteName = "stage1";
        if (SceneName.Equals("03_GameScene2") == true)
            background.spriteName = "stage2";
        if (SceneName.Equals("03_GameScene3") == true)
            background.spriteName = "stage3";

        StartCoroutine(Loading());
	}
	
	// Update is called once per frame
	void Update () {
        if (loadComplete)
        {
            if(Input.anyKeyDown==true)
                Application.LoadLevel(SceneName);
        }
	}

    IEnumerator Loading()
    {
        loadingLabel.text = "Loading . . .";
        yield return new WaitForSeconds(0.5f);
        loadingLabel.text = "Loading .";
        yield return new WaitForSeconds(0.5f);
        loadingLabel.text = "Loading . .";
        yield return new WaitForSeconds(0.5f);
        loadingLabel.text = "Loading . . .";
        yield return new WaitForSeconds(0.5f);
        loadingLabel.text = "Loading .";
        yield return new WaitForSeconds(0.5f);
        loadingLabel.text = "Loading . .";
        yield return new WaitForSeconds(0.5f);
        loadingLabel.text = "Loading . . .";
        yield return new WaitForSeconds(0.5f);
        loadingLabel.text = "Complete !";
        loadComplete = true;
    }
}
