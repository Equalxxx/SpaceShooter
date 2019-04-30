using UnityEngine;
using System.Collections;

public class ExitGame : MonoBehaviour {

    public TweenAlpha tAlpha1;
    public TweenAlpha tAlpha2;

    public string BGMName;
    public float volume;

    private static ExitGame sInstance;
    public static ExitGame Instance
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
        BGMManager.Instance.CrossFadeBGM(BGMName);
        BGMManager.Instance.masterVolume = volume;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
	}

    public void EndGame(int type)
    {
        if (type == 0)
            tAlpha1.PlayForward();
        else
            tAlpha2.PlayForward();

        StartCoroutine(ChangeLevel());
    }

    IEnumerator ChangeLevel()
    {
        yield return new WaitForSeconds(6.5f);
        Application.LoadLevel(1);
    }
}
