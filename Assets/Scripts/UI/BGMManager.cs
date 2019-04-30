using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BGMManager : MonoBehaviour {

    private static BGMManager sInstance;
    public static BGMManager Instance
    {
        get
        {
            if (sInstance == null)
            {
                GameObject newGameObject = new GameObject("_BGMManager");
                sInstance = newGameObject.AddComponent<BGMManager>();
            }

            return sInstance;
        }
    }


    private Dictionary<string, AudioClip> bgmTable;

    private AudioSource audio0;         //주플레이 AudioSource
    private AudioSource audio1;         //빠지는 플레이 AudioSource

    private float crossFadeTime = 5.0f;

    public float masterVolume = 1.0f;       //BGMManager 마스터 볼륨

    private float volume0 = 0.0f;            //AudioSource0 번볼륨
    private float volume1 = 0.0f;            //AudioSource1 번볼륨

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        this.bgmTable = new Dictionary<string, AudioClip>();

        //오디오 소스 추가
        audio0 = this.gameObject.AddComponent<AudioSource>();
        audio1 = this.gameObject.AddComponent<AudioSource>();
        audio0.loop = true;
        audio1.loop = true;
        //둘다 볼륨 0 으로 시작
        this.audio0.volume = 0.0f;
        this.audio1.volume = 0.0f;
    }


    void Update()
    {
        //BGM 이 플레이 중이라면..
        if (this.audio0.isPlaying)
        {
            //주플레이 AudioSource 는 볼륨을 올린다.
            if (volume0 < 1.0f)
            {
                volume0 += Time.deltaTime / this.crossFadeTime;
                if (volume0 >= 1.0f)
                    volume0 = 1.0f;
            }

            //빠진는 AudioSource 는 볼륨을 내린다.
            if (volume1 > 0.0f)
            {
                volume1 -= Time.deltaTime / this.crossFadeTime;
                if (volume1 <= 0.0f)
                {
                    volume1 = 0.0f;
                    this.audio1.Stop();
                }
            }
        }

        this.audio0.volume = this.volume0 * this.masterVolume;
        this.audio1.volume = this.volume1 * this.masterVolume;
        

       

    }




    public void PlayBGM(string bgmName)
    {
        if (this.bgmTable.ContainsKey(bgmName) == false)
        {
            //Resources 에서 BGM 오디오클립 로드
            AudioClip newBGM = Resources.Load("BGM/" + bgmName) as AudioClip;

            if (newBGM == null)
                return;

            //Tablb 에 추가
            this.bgmTable.Add(bgmName, newBGM);
        }

        //클립에 새로운 오디오 클립 물린다.
        this.audio0.clip = this.bgmTable[bgmName];
        this.audio0.Play();

        volume0 = 1.0f;
        volume1 = 0.0f;
    }


    public void CrossFadeBGM(string bgmName, float crossFadeTime = 1.0f )
    {
        if (this.bgmTable.ContainsKey(bgmName) == false)
        {
            //Resources 에서 BGM 오디오클립 로드
            AudioClip newBGM = Resources.Load("BGM/" + bgmName) as AudioClip;

            if (newBGM == null)
                return;

            //Tablb 에 추가
            this.bgmTable.Add(bgmName, newBGM);
        }


        this.crossFadeTime = crossFadeTime;

        //기존에 플레이되는 것을 1 번으로...
        AudioSource temp = this.audio0;
        this.audio0 = this.audio1;
        this.audio1 = temp;

        //볼륨값스왑
        float tempVolume = this.volume0;
        this.volume0 = this.volume1;
        this.volume1 = tempVolume;

        //클립에 새로운 오디오 클립 물린다.
        this.audio0.clip = this.bgmTable[bgmName];
        this.audio0.Play();


        //this.audio0.Pause();
        //this.audio0.Play();
    }


   

}
