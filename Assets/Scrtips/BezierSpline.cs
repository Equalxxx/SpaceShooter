using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Bezier 컨트롤 모드
public enum BezierControlPointMode
{
    Free,
    Aligned,
    Mirrored
}



public class BezierSpline : MonoBehaviour {

    //참고로 Undo 먹으려면 맴버가 private 면 안된다.

    [HideInInspector]
    public List<Vector3> points;
    
    [HideInInspector]
    public List<BezierControlPointMode> controlModes;

    [HideInInspector]
    public bool loop;       //루프니?
    

    //Editor 에 적용될때 실행됨 Reset 을 해도 실행됨
    void Reset()
    {
        //점이 4 개
        this.points = new List<Vector3>();
        this.points.Add(new Vector3(1, 0, 0));
        this.points.Add(new Vector3(2, 0, 0));
        this.points.Add(new Vector3(3, 0, 0));
        this.points.Add(new Vector3(4, 0, 0));

        //컨트롤 모드 2 개 추가
        this.controlModes = new List<BezierControlPointMode>();
        this.controlModes.Add(BezierControlPointMode.Free);
        this.controlModes.Add(BezierControlPointMode.Free);

    }

    public Vector3 GetPoint(float t)
    {
        t = Mathf.Clamp01(t);
        //0, 1, 2, 3, 4, 5, 6, 7, 8, 9      
        int startIndex = 0;

        //t 가 1 과 같거나 크다면...
        if (t >= 1.0f)
        {
            t = 1.0f;
            startIndex = this.points.Count - 4;     //마지막 커프 시작인덱스
        }
        else
        {
            //0, 1, 2, 3, 4, 5, 6, 7, 8, 9  -> 3Curve
            //t = 0.4f;

            t = t * CurveCount;         // 1.2
            startIndex = (int)t;        // 1 ( CurveIndex )
            t = t - startIndex;         // 0.2f
            startIndex *= 3;            // 3
        }



        return this.transform.TransformPoint(
            Bezier.GetPoint(
            points[startIndex + 0], 
            points[startIndex + 1],
            points[startIndex + 2],
            points[startIndex + 3], 
            t));
    }



    public Vector3 GetVelocity(float t)
    {
        t = Mathf.Clamp01(t);
        //0, 1, 2, 3, 4, 5, 6, 7, 8, 9      
        int startIndex = 0;

        //t 가 1 과 같거나 크다면...
        if (t >= 1.0f)
        {
            t = 1.0f;
            startIndex = this.points.Count - 4;     //마지막 커프 시작인덱스
        }
        else
        {
            //0, 1, 2, 3, 4, 5, 6, 7, 8, 9  -> 3Curve
            //t = 0.4f;

            t = t * CurveCount;         // 1.2
            startIndex = (int)t;        // 1 ( CurveIndex )
            t = t - startIndex;         // 0.2f
            startIndex *= 3;            // 3
        }

        return this.transform.TransformVector(
            Bezier.GetFirstDerivative(
            points[startIndex + 0],
            points[startIndex + 1],
            points[startIndex + 2],
            points[startIndex + 3], 
            t));
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }


    //커브 추가
    public void AddCurve()
    {
        //마지막 점의 위치
        Vector3 lastPoint = this.points[this.points.Count - 1];

        //마지막 점의 컨트롤 모드
        BezierControlPointMode lastMode = this.controlModes[ this.controlModes.Count - 1];

        //점 3개 추가
        lastPoint += Vector3.right;
        this.points.Add(lastPoint);
        lastPoint += Vector3.right;
        this.points.Add(lastPoint);
        lastPoint += Vector3.right;
        this.points.Add(lastPoint);

        //커브가 추가되면 포인트 모드도 추가
        this.controlModes.Add(lastMode);        //마지막 모드와 같게...

        //점의 위치 변경에 따른 강제 셋팅
        EnforceMode(points.Count - 4);          //마직막으로 추가된 Curve 의 시작인덱스 강제 셋팅

    }

    //커브갯수
    public int CurveCount
    {
        get
        {
            return (this.points.Count - 1) / 3;
        }
    }


    //커브 인덱스에 따른 시작 정점 인덱스
    public int GetCurveStartIndex(int curveIndex)
    {
        //0 , 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 
        //0 = 0;
        //1 = 3;
        //2 = 6;

        return curveIndex * 3;
    }

    public BezierControlPointMode GetPointMode(int pointIndex)
    {
        //해당 점의 모드 인덱스를 얻는다.
        //0 = 0
        //1 = 0
        //2 = 1
        //3 = 1
        //4 = 1
        //5 = 2
        //6 = 2
        //7 = 2
        //8 = 3
        //...
        int modeIndex = (pointIndex + 1) / 3;
        return this.controlModes[modeIndex];

    }

    public void SetPointMode(int pointIndex, BezierControlPointMode mode)
    {
        int modeIndex = (pointIndex + 1) / 3;
        this.controlModes[modeIndex] = mode;

        //Loop 인경우
        if (loop)
        {
            //모드 인덱스가 0 인경우
            if (modeIndex == 0)
                controlModes[controlModes.Count - 1] = mode;     //마지막 모드도 같게 한다.

            //모드인덱스가 마지막인경우
            else if (modeIndex == controlModes.Count - 1)
                controlModes[0] = mode;                    //첫번째 모드도 같게 한다.

        }


        //컨트롤 모드에 따른 강제 셋팅
        EnforceMode(pointIndex);
    }

    public Vector3 GetPoint(int index)
    {
        return this.points[index];
    }

    public void SetPoint(int index, Vector3 point)
    {
        //중간점을 셋팅하는 것이라면 앞뒤 양쪽으로 달린 점들도 같이 컨르롤 되게 해야한다.
        if (index % 3 == 0)
        {
            //변위량
            Vector3 delta = point - points[index];

            //loop 모드 인경우
            if (loop)
            {
                //처음 점을 컨트롤 하는 경우
                if (index == 0)
                {
                    points[1] += delta;     //다음점
                    points[points.Count - 2] += delta; //이전점
                    points[points.Count - 1] = point;  //마지막점은 포인트와 같다.
                }

                //마지막 점을 컨트롤 하는 경우
                else if (index == points.Count - 1)
                {
                    points[1] += delta;    //다음점
                    points[index - 1] += delta;     //이전점
                    points[0] = point;                   //처음점은 포인트와 같다.
                }

                else
                {
                    points[index - 1] += delta;     //이전점
                    points[index + 1] += delta;     //다음점
                }
            }


            else
            {
                if (index > 0)
                    points[index - 1] += delta;
                
                if (index + 1 < points.Count)
                    points[index + 1] += delta;
            }

            
        }


        this.points[index] = point;

        //점의 위치 변경에 따른 강제 셋팅
        EnforceMode(index);
    }


    //컨트롤 모드에 따른 강제 셋팅
    private void EnforceMode(int index)
    {
        //해당 점의 모드 인덱스를 얻는다.
        //0 = 0
        //1 = 0
        //2 = 1
        //3 = 1
        //4 = 1
        //5 = 2
        //6 = 2
        //7 = 2
        //8 = 3
        //9 = 3
        //10 = 3
        //11 = 4
        //12 = 4
        //...               11 + 1 / 4
        int modeIndex = (index + 1) / 3;

        //루프가 아닌 경우에만 첫번째 모드나 마지막모드 하지안는다.
        if (!loop && (modeIndex == 0 || modeIndex == this.controlModes.Count - 1))
            return;

        

        //해당인덱스의 컨트롤 모드를 얻는다.
        BezierControlPointMode mode = this.controlModes[modeIndex];
        if (mode == BezierControlPointMode.Free) return;

        //해당 점인덱스에따른 중간점 인덱스
        //0 = 0 
        //1 = 0 
        //2 = 3 
        //3 = 3
        //4 = 3
        //5 = 6
        //6 = 6
        //7 = 6
        //8 = 9
        int middleIndex = modeIndex * 3;        //중간점의 인덱스 

        int fixedIndex = 0;                     //고정 인덱스
        int enforcedIndex = 0;                  //움직여지는 인덱스

        //점의 인덱스가 중간 인덱스보다 작다면..
        if (index <= middleIndex)
        {
            fixedIndex = middleIndex - 1;       //고정인덱스는 중간점의 이전 인덱스

            //0 보다 작아지는 경우
            if (fixedIndex < 0 )
                fixedIndex = points.Count - 2;  

            enforcedIndex = middleIndex + 1;    //움직이는 인덱스는 중간점의 다음 인덱스

            //길이보다 커지는 경우
            if (enforcedIndex >= points.Count)
                enforcedIndex = 1;

        }

        //점의 인덱스가 중간 인덱스보다 크다면...
        else
        {
            fixedIndex = middleIndex + 1;       //고정인덱스는 중간점의 다음 인덱스
            if (fixedIndex >= points.Count)
                fixedIndex = 1;

            enforcedIndex = middleIndex - 1;    //움직이는 인덱스는 중간점의 이전 인덱스
            if (enforcedIndex < 0)
                enforcedIndex = points.Count - 2;
        }

        Vector3 middle = points[middleIndex];       //중간 지점의 점을 얻는다.
        Vector3 enforcedTangent = middle - points[fixedIndex];          //고정 인덱스 점으로 부터 중간인덱스까지의 방향벡터

       

        //Aligned Mode 
        if (mode == BezierControlPointMode.Aligned)
        {
            float length = Vector3.Distance(middle, points[enforcedIndex]);      //움직여지는 점에서부터 중간점가지의 거리
            points[enforcedIndex] = middle + enforcedTangent.normalized * length;
        }
        //Mirror Mode
        else
        {
            points[enforcedIndex] = middle + enforcedTangent;
        }
    }


    public bool Loop
    {
        get
        {
            return loop;
        }

        set
        {
            loop = value;

            //Loop 모드 셋팅으로 들어왔다면...
            if (value == true)
            {
                controlModes[controlModes.Count - 1] = controlModes[0]; //마지막 모드를 첫번째 모드와 같게....
                SetPoint(0, points[0]);         //의미없고 SetPoint 의 루프로 되었을때의 업데트를 위한 실행


            }



        }
    }



}
