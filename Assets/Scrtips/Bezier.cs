using UnityEngine;
using System.Collections;

public static class Bezier      //static 맴버만 가질수 있다.
{
    
    //3점의 베지어 곡선의 위치를 얻는다.
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        /*
        Vector3 lerp0 = Vector3.Lerp(p0, p1, t);
        Vector3 lerp1 = Vector3.Lerp(p1, p2, t);
        Vector3 result = Vector3.Lerp(lerp0, lerp1, t);
        return result;
        */

        //공식을 풀어 재끼자.
        //Vector3.Lerp( a, b, t ) = A  
        //A = ( 1 - t )a + tb;

        //Vector3.Lerp( b, c, t ) = B
        //B = ( 1 - t )b + tc;

        //Vector3.Lerp(A, B, t) = Result;
        //Result = ( 1 - t )A + tB;
        //Result = ( 1 - t )( ( 1 - t )a + tb ) + ( t )( ( 1 - t )b + tc );
        //( 1 - t ) => InvT, t => T
        //Result = (InvT)( (InvT)a + Tb) + (T)( (InvT)b + Tc );
        //Result = ( (InvT)^2 )a + (InvT)(T)b + (InvT)(T)b + (T)^2c;
        //Result = ( InvT * InvT * a ) + ( 2 * InvT * T * b ) + ( T * T * c );

        t = Mathf.Clamp01(t);
        float oneMinusT = 1 - t;
        return (oneMinusT * oneMinusT * p0) + (2 * oneMinusT * t * p1) + (t * t * p2); 

    }

    //4점의 베지어 곡선의 위치를 얻는다.
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        //Vector3 a0 = Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
        //Vector3 a1 = Vector3.Lerp(Vector3.Lerp(p1, p2, t), Vector3.Lerp(p2, p3, t), t);
        //return Vector3.Lerp(a0, a1, t);

        t = Mathf.Clamp01(t);
        float oneMinusT = 1 - t;

        return (oneMinusT * oneMinusT * oneMinusT * p0) +
               (3.0f * oneMinusT * oneMinusT * t * p1) +
               (3.0f * oneMinusT * t * t * p2) +
               (t * t * t * p3);
    }




    //해당 위치의 진행 벨로 시티를 얻는다.
    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        //진행방향을 얻는다.
        //이는 각구간의 방향값을 T 의 값으로 평균을 내는 것이다.


        Vector3 dir0 = (p1 - p0);       //0에서 부터 1 의 진행방향
        Vector3 dir1 = (p2 - p1);       //1에서 부터 2 의 진행방향

        Vector3 dirA = (1.0f - t) * dir0;       //0.5 미만에서 의 방향
        Vector3 dirB = t * dir1;                //0.5 이상에서의 방향

        return 2.0f * dirA + 2.0f * dirB;

    }

    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 dir0 = (p1 - p0);       //0에서 부터 1 의 진행방향
        Vector3 dir1 = (p2 - p1);       //1에서 부터 2 의 진행방향
        Vector3 dir2 = (p3 - p2);       //2에서 부터 3 의 진행방향

        t = Mathf.Clamp01(t);
        float oneMinusT = 1.0f - t;
        return (3.0f * oneMinusT * oneMinusT * dir0) +
               (6.0f * oneMinusT * t * dir1) +
               (3.0f * t * t * dir2);
    }
    

}
