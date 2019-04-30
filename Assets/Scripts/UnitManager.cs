using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum UnitForce
{
    Force_Team, Force_Enemy
}

public class UnitManager : MonoBehaviour {

    private static bool isOpen = true;
    public static bool IsOpen
    {
        get
        {
            return isOpen;
        }
    }

    private static UnitManager sInstance;
    public static UnitManager Instance
    {
        get
        {
            if (sInstance == null)// && isOpen)
            {
                GameObject newUnitManager = new GameObject("UnitManager");
                sInstance = newUnitManager.AddComponent<UnitManager>();
            }
            return sInstance;
        }
    }

    private Dictionary<UnitForce,List<UnitState>> UnitTable = null;

    void Awake()
    {
        sInstance = this;

        this.UnitTable = new Dictionary<UnitForce, List<UnitState>>();
    }

    public void AddUnit(UnitState unit)
    {
        if (this.UnitTable.ContainsKey(unit.force))
        {
            this.UnitTable[unit.force].Add(unit);
        }

        else
        {
            List<UnitState> newAirList = new List<UnitState>();
            newAirList.Add(unit);

            this.UnitTable.Add(unit.force, newAirList);
        }

    }

    public void RemoveUnit(UnitState UnitState)
    {
        if (this.UnitTable.ContainsKey(UnitState.force))
        {
            this.UnitTable[UnitState.force].Remove(UnitState);
        }
    }

    public List<UnitState> GetUnitList(UnitForce force)
    {
        if (this.UnitTable.ContainsKey(force) && this.UnitTable[force].Count > 0)
            return this.UnitTable[force];

        return null;
    }

    public int GetLiveEnemyCount(UnitForce force)
    {
        int nums = 0;
        for (int i = 0; i < UnitTable[force].Count; i++)
        {
            if (UnitTable[force][i].gameObject.activeSelf == true)
                nums++;
        }
        return nums;
    }
    public UnitState GetUnitRandom(UnitForce force)
    {
        if (this.UnitTable.ContainsKey(force) && this.UnitTable[force].Count > 0)
        {
            List<UnitState> list = this.UnitTable[force];

            return list[Random.Range(0, list.Count)];
        }

        return null;
    }


    void OnDestroy()
    {
        isOpen = false;
    }

    //void OnGUI()
    //{
    //    float x = 0.0f;
    //    foreach (UnitForce force in this.UnitTable.Keys)
    //    {
    //        List<UnitState> list = this.UnitTable[force];

    //        for (int i = 0; i < list.Count; i++)
    //        {
    //            GUI.Label(new Rect(x, i * 20, 200, 20), list[i].gameObject.name);
    //        }

    //        x += 200;

    //    }
    //}
}
