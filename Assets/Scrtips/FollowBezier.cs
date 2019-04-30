using UnityEngine;
using System.Collections;

public class FollowBezier : MonoBehaviour {

    public BezierSpline bezierSpline;

    [Range( 0.0f, 1.0f )]       //인스펙터에서 슬라이드 바로 0 ~ 1 로 셋팅 가능
    public float factor = 0.0f;

	// Update is called once per frame
	void Update () {
		if (factor < 1.0f) {
			factor += Time.deltaTime*3.0f;

			//factor %= 1.0f;


			//베지어의 위치를 얻는다.
			this.transform.position = this.bezierSpline.GetPoint (factor);
	

			//진행방향을 얻는다.
			Vector3 forward = this.bezierSpline.GetDirection (factor);

			this.transform.rotation = Quaternion.LookRotation (forward, Vector3.up);

		}
	}
}
