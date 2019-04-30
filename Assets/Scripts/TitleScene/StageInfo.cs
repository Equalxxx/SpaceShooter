using UnityEngine;
using System.Collections;

public class StageInfo : MonoBehaviour {

    public string sceneName = "";
    private static StageInfo sInstance;
    public static StageInfo Instance
    {
        get
        {
            return sInstance;
        }

    }
    void Awake()
    {
        sInstance = this;
    }

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
	}

    public void SelectSceneName(string name)
    {
        sceneName = name;
    }
    public void DestroyThisInfo()
    {
        Destroy(this.gameObject);
    }
}
