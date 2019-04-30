using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour {

    private static BulletPool sInstance;
    public static BulletPool Instance
    {
        get 
        {
            return sInstance;
        }
    }

    public GameObject Beam_TeamsPrefab;
    public GameObject Beam_EnemysPrefab;
    public GameObject HomingLaser_PlayerPrefab;
    public GameObject homingLaser_BossPrefab;
    public GameObject BeamLarge_BossPrefab;

    public List<GameObject> Beam_Teams;
    public List<GameObject> Beam_Enemys;
    public List<GameObject> HomingLaser_Player;
    public List<GameObject> HomingLaser_Boss;
    public List<GameObject> BeamLarge_Boss;

    private int beamNums = 30;
    private int HomingLaserNums = 30;
    private int beamLargeNums = 5;
    
    void Awake()
    {
        sInstance = this;
    }

	void Start () {
        for (int i = 0; i < beamNums; i++)
        {
            GameObject newBullet = Instantiate(Beam_TeamsPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            newBullet.transform.SetParent(this.transform);
            newBullet.SetActive(false);

            Beam_Teams.Add(newBullet);
        }
        for (int i = 0; i < beamNums; i++)
        {
            GameObject newBullet = Instantiate(Beam_EnemysPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            newBullet.transform.SetParent(this.transform);
            newBullet.SetActive(false);

            Beam_Enemys.Add(newBullet);
        }
        for (int i = 0; i < HomingLaserNums; i++)
        {
            GameObject newBullet = Instantiate(HomingLaser_PlayerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            newBullet.transform.SetParent(this.transform);
            newBullet.SetActive(false);

            HomingLaser_Player.Add(newBullet);
        }
        for (int i = 0; i < HomingLaserNums; i++)
        {
            GameObject newBullet = Instantiate(homingLaser_BossPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            newBullet.transform.SetParent(this.transform);
            newBullet.SetActive(false);

            HomingLaser_Boss.Add(newBullet);
        }
        for (int i = 0; i < beamLargeNums; i++)
        {
            GameObject newBullet = Instantiate(BeamLarge_BossPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            newBullet.transform.SetParent(this.transform);
            newBullet.SetActive(false);

            BeamLarge_Boss.Add(newBullet);
        }
	}

    public GameObject GetBullet(Transform trans, string typeName)
    {
        if (typeName.CompareTo("Beam_Team") == 0) //Beam_Team
        {
            for (int i = 0; i < beamNums; i++)
            {
                if (Beam_Teams[i].activeSelf == false)
                {
                    Beam_Teams[i].transform.position = trans.position;
                    Beam_Teams[i].transform.forward = trans.forward;
                    Beam_Teams[i].SetActive(true);
                    return Beam_Teams[i];
                }
            }
        }
        else if (typeName.CompareTo("Beam_Enemy") == 0) //beam Enemy
        {
            for (int i = 0; i < beamNums; i++)
            {
                if (Beam_Enemys[i].activeSelf == false)
                {
                    Beam_Enemys[i].transform.position = trans.position;
                    Beam_Enemys[i].transform.forward = trans.forward;
                    Beam_Enemys[i].SetActive(true);
                    return Beam_Enemys[i];
                }
            }
        }
        else if (typeName.CompareTo("HomingLaser_Player") == 0) //HomingLaser_Player
        {
            for (int i = 0; i < HomingLaserNums; i++)
            {
                if (HomingLaser_Player[i].activeSelf == false)
                {
                    HomingLaser_Player[i].transform.position = trans.position;
                    HomingLaser_Player[i].transform.forward = trans.forward;
                    HomingLaser_Player[i].SetActive(true);
                    return HomingLaser_Player[i];
                }
            }
        }
        else if (typeName.CompareTo("HomingLaser_Boss") == 0) //HomingLaser_Boss
        {
            for (int i = 0; i < HomingLaserNums; i++)
            {
                if (HomingLaser_Boss[i].activeSelf == false)
                {
                    HomingLaser_Boss[i].transform.position = trans.position;
                    HomingLaser_Boss[i].transform.forward = trans.forward;
                    HomingLaser_Boss[i].SetActive(true);
                    return HomingLaser_Boss[i];
                }
            }
        }
        else if (typeName.CompareTo("BeamLarge_Boss") == 0) //BeamLarge_Boss
        {
            for (int i = 0; i < beamLargeNums; i++)
            {
                if (BeamLarge_Boss[i].activeSelf == false)
                {
                    BeamLarge_Boss[i].transform.position = trans.position;
                    BeamLarge_Boss[i].transform.forward = trans.forward;
                    BeamLarge_Boss[i].SetActive(true);
                    return BeamLarge_Boss[i];
                }
            }
        }
        return null;
    }
}
