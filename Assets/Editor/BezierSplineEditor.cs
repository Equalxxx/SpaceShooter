using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor( typeof( BezierSpline ))]
public class BezierSplineEditor : Editor {

    private const int lineStep = 100;        //몇개로 쪼겔꺼니?
    private const int directionStep = 10;    //

    private const float handleSize = 0.02f;         //PointHandle 의 크기

    private BezierSpline spline;
    private Transform handleTranform;
    private Quaternion handleRotation;


    private int selectIndex = 0;            //선택되 점의 인덱스

    private GUIStyle guiStyle = new GUIStyle();

    private void OnSceneGUI()
    {
        //Target 인스턴스를 얻는다.
        this.spline = target as BezierSpline;

        //Target 인스턴스의 Transform 을 얻는다.
        this.handleTranform = this.spline.transform;

        //Rotation 을 얻는다.
        this.handleRotation = (Tools.pivotRotation == PivotRotation.Global) ? Quaternion.identity : handleTranform.rotation;


        for (int i = 0; i < this.spline.CurveCount; i++)
        {
            //해당 커브의 시작 인덱스
            int startIdx = spline.GetCurveStartIndex(i);

            Vector3 p0 = ShowPoint(startIdx + 0);
            Vector3 p1 = ShowPoint(startIdx + 1);
            Vector3 p2 = ShowPoint(startIdx + 2);
            Vector3 p3 = ShowPoint(startIdx + 3);

            //연결선을 그린다 
            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            //베지어 곡선을 그린다.
            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 3.0f);
        }

        /*
        //베지어 곡선을 그린다.
        Handles.color = Color.white;
        Vector3 startPoint = spline.GetPoint(0.0f);
        int step = lineStep * spline.CurveCount;
        for (int j = 1; j <= step; j++)
        {
            float factor = (j / (float)step);
            Vector3 nowPoint = spline.GetPoint(factor);
            Handles.DrawLine(startPoint, nowPoint);
            startPoint = nowPoint;
        }*/

        //진행방향
        Handles.color = Color.green;
        int step = directionStep * spline.CurveCount;
        for (int j = 0; j <= step; j++)
        {
            float factor = j / (float)step;
            Vector3 startPoint = spline.GetPoint(factor);
            Vector3 direction = spline.GetDirection(factor);
            Handles.DrawLine(startPoint, startPoint + direction);
        }
        
    }

    //해당인덱스의 포인트를 그리고 변경된 값 리턴
    Vector3 ShowPoint(int index)
    {
        //해당인덱스의 점 월드
        Vector3 worldPoint = this.handleTranform.TransformPoint(spline.GetPoint( index ) );


        Color handleColor = Color.white;
        //해당 모드를 얻자
        BezierControlPointMode mode = this.spline.GetPointMode(index);

        if (mode == BezierControlPointMode.Free)
            handleColor = Color.white;
        else if (mode == BezierControlPointMode.Aligned)
            handleColor = Color.magenta;
        else if (mode == BezierControlPointMode.Mirrored)
            handleColor = Color.cyan;

        //핸들컬러 설정
        Handles.color = handleColor;
        guiStyle.normal.textColor = handleColor;

        //처음점
        if (index == 0)
        {
            guiStyle.fontSize = 20;
            guiStyle.fontStyle = FontStyle.BoldAndItalic;
            Handles.Label(worldPoint, "Start", guiStyle);
        }
        else if (index == spline.points.Count - 1)
        {
            guiStyle.fontSize = 20;
            guiStyle.fontStyle = FontStyle.BoldAndItalic;
            Handles.Label(worldPoint, "End", guiStyle);
        }

        else if (index % 3 == 0)
        {
            guiStyle.fontSize = 15;
            guiStyle.fontStyle = FontStyle.Bold;

            Handles.Label(worldPoint, ((index + 1) / 3).ToString(), guiStyle);

        }









        //점의 월드 위치로 점크기의 비율을얻어 원래 크기에 곱한다.
        float dotSize = HandleUtility.GetHandleSize(worldPoint) * handleSize;
        //점이 컨트롤 포인트가 아니라 Curve 의 시작인덱스라면 좀더 크게...
        if( index % 3 == 0 )
            dotSize *= 2.0f;

      
        //에디터 씬뷰에 점 버튼을 배치 해당 버튼이 포커팅 되어있다면 true 를 리턴
        if (Handles.Button(worldPoint, handleRotation, dotSize, dotSize * 2, Handles.DotCap))
        {
            //지금 선택된 점의 인덱스
            selectIndex = index;
            
            //갱신된 값 인스펙터뷰에 바로 적용이 되게...
            //Repaint();
        }


        //위의 Handle.Button 블록 내에서 하면 안되는듯.....
        if (index == selectIndex)
        {
            EditorGUI.BeginChangeCheck();
            worldPoint = Handles.DoPositionHandle(worldPoint, this.handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this.spline, "Change Point");
                EditorUtility.SetDirty(this.spline);

                spline.SetPoint( index, this.handleTranform.InverseTransformPoint(worldPoint) );
            }
        }

        /*
        EditorGUI.BeginChangeCheck();
        worldPoint = Handles.DoPositionHandle(worldPoint, this.handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(this.spline, "Change Point");
            EditorUtility.SetDirty(this.spline);

            spline.points[index] = this.handleTranform.InverseTransformPoint(worldPoint);
        }*/

        return worldPoint;
    }





    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();      //기본 적인거 그리고...

        //타겟 갱신
        this.spline = this.target as BezierSpline;

        //Loop 셋팅 
        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Loop", spline.Loop);
        if (EditorGUI.EndChangeCheck())
        {

            Undo.RecordObject(spline, "Toggle Loop");
            EditorUtility.SetDirty(spline);
            spline.Loop = loop;
        }


        
        //선택된 점의 인덱스가 존재한다면...
        if (selectIndex >= 0 && selectIndex < spline.points.Count)      //범위안에 있다면...
        {
            //선택된 점의 위치 를 셋팅할수 있는 Vector3 Field
            GUILayout.Label("Selected Point");
            EditorGUI.BeginChangeCheck();
            Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetPoint(selectIndex));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.SetPoint(selectIndex, point);
            }


            //점의 포인트 모드를 설정할수 있는 Popup 메뉴 생성
            EditorGUI.BeginChangeCheck();
            //선택된 포인트의 모드가 어똫게 되니?
            BezierControlPointMode selectMode = spline.GetPointMode(selectIndex);

            //변경된 값을 다시 받는다.
            selectMode = (BezierControlPointMode)EditorGUILayout.EnumPopup("Mode", selectMode);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Change Point Mode");
                spline.SetPointMode(selectIndex, selectMode);
                EditorUtility.SetDirty(spline);
            }
            

        }



        //밑에 버튼 추가
        if (GUILayout.Button("Spline Add"))
        {
            Undo.RecordObject(spline, "Add Curve");
            
            //버튼을 클릭했다면...
            this.spline.AddCurve();

            EditorUtility.SetDirty(spline);
        }

    }
}
