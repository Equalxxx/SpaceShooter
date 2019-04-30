using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

[ExecuteInEditMode]
[RequireComponent(typeof(BezierSpline))]
public class BezierSetObject : MonoBehaviour {

    private BezierSpline bezierCurve;
    
    [HideInInspector]   //public 이여도 안보인다.
	public List<GameObject> obejctList = new List<GameObject>();

	public GameObject srcObject;        //셋팅될 오브잭트 프리팹
	public float minRange = 0.0f;       //최소 범위
	public float maxRange = 1.0f;       //최대 범위
	public int objectNum = 5;           //갯수

    public bool bAllowRotation = false; //배치될때 베지어 로테이션대로 할꺼니?
    public Vector3 offset = Vector3.zero;


    public bool bNumberic = true;      //번호 부여할꺼니?

    private StringBuilder strBuilder;

	void Awake(){
		this.bezierCurve = this.GetComponent<BezierSpline>();
        this.strBuilder = new StringBuilder(1000);
	}


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if( Application.isPlaying == false )
		{
            //베지어 커브가 없으면 찾고 못찾으면 안한다.
			if( this.bezierCurve == null ){
                this.bezierCurve = this.GetComponent<BezierSpline>();
				if( this.bezierCurve == null )
					return;
			}


            //셋팅된 오브젝트가 없어도 안한다.
			if( this.srcObject == null ) return;

			int addNum = this.objectNum - this.obejctList.Count;


			if( addNum > 0 )
			{
				for( int i = 0 ; i < addNum ; i++ )
				{
					GameObject newObject = Instantiate(
						this.srcObject, 
						Vector3.zero,
						Quaternion.identity ) as GameObject;

					newObject.transform.parent = this.transform;

					this.obejctList.Add( newObject );
				}
			}

			else if( addNum < 0 )
			{
				for( int i = 0 ; i < Mathf.Abs( addNum ) ; i++ )
				{
					int del = this.obejctList.Count - 1 - i;
					
                    //Edit 모드에서 셋팅된 오브젝트를 삭제할때는
                    //DestroyImmediate 로 해라...
                    GameObject.DestroyImmediate( this.obejctList[del] );
				}
				
				this.obejctList.RemoveRange( this.obejctList.Count + addNum, Mathf.Abs( addNum ));
			}
			


			//배치
			float factor = this.minRange;
			float deltaFactor = this.maxRange - this.minRange;

			if( deltaFactor < 0.0 ){
			
				return;

			}
			float intervalFactor = deltaFactor / ( this.objectNum - 1 );
    
			for( int i = 0 ; i < this.objectNum ; i++ )
			{
				Vector3 pos = this.bezierCurve.GetPoint( factor );
                Quaternion rot = Quaternion.LookRotation( this.bezierCurve.GetDirection(factor) );

                if (this.bAllowRotation)
                    this.obejctList[i].transform.rotation = rot;

                else
                    this.obejctList[i].transform.rotation = Quaternion.identity;

                //오프셋이 제로가 아니라면...
                if (this.offset != Vector3.zero)
                {
                    this.obejctList[i].transform.position = pos + (rot * this.offset);

                }
                else
                {
                    this.obejctList[i].transform.position = pos;

                }





				factor += intervalFactor;
            }


            //번호가 부여 된다면...
            if (this.bNumberic)
            {
                for (int i = 0; i < this.obejctList.Count; i++)
                {
                    if (this.strBuilder == null)
                        this.strBuilder = new StringBuilder(1000);
                    this.strBuilder.Length = 0; //Clear

                    this.strBuilder.Append(srcObject.name);
                    this.strBuilder.AppendFormat("{0:D5}", i);

                    obejctList[i].name = this.strBuilder.ToString();
                }
              
            }
            





		}
	
	}

 
}
