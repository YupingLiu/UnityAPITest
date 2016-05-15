using MoreFun;
using UnityEngine;
public class InstanceFields : MonoBehaviour {

    // 1、eulerAngles属性：欧拉角
    public Transform _eulerAnglesA, _eulerAnglesB;
    private Quaternion rotations = Quaternion.identity;
    private Vector3 eulerAngle = Vector3.zero;
    private float speed = 10.0f;

    // 2、SetFromToRotation方法：创建rotation实例
    public Transform _setFromToRotationTrans;
    private Quaternion _fromToRotationQuat = Quaternion.identity;

    // 3、SetLookRotation方法：设置Quaternion实例的朝向
    public Transform _setLookRotationTrans;
    private Quaternion _lookRotationQuat = Quaternion.identity;

    // 4、ToAngleAxis方法：Quaternion实例的角轴表示

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // 1、eulerAngles属性：欧拉角
        // 第一种方式：将Quaternion赋值给transform的rotation
        rotations.eulerAngles = new Vector3(0.0f, speed * Time.time, 0.0f);
        _eulerAnglesA.rotation = rotations;
        // 第二种方式：将三维向量代表的欧拉角直接赋值给transform的eulerAngles
        eulerAngle = new Vector3(0.0f, speed * Time.time, 0.0f);
        _eulerAnglesB.eulerAngles = eulerAngle;

        //2、SetFromToRotation方法：创建rotation实例
        //此方法用于创建一个从fromDirection到toDirection的rotation
        //相当于将GameObject对象进行如下变换：
        //首先将GameObject对象自身坐标系的x、y、z轴方向和世界坐标系一致，
        //然后将GameObject对象自身坐标系中向量v1指向的方向旋转到v2方向。
        //注意：不可以直接使用transfrom.rotation.SetFromToDirection（v1，v2）方式进行设置
        //只能将实例化的Quaternion赋值给transform.rotation
        _fromToRotationQuat.SetFromToRotation(_eulerAnglesA.position, _eulerAnglesB.position);
        _setFromToRotationTrans.rotation = _fromToRotationQuat;
        //在Scene面板中绘制直线
        Debug.DrawLine(Vector3.zero, _eulerAnglesA.position, Color.red);
        Debug.DrawLine(Vector3.zero, _eulerAnglesB.position, Color.green);
        Debug.DrawLine(_setFromToRotationTrans.position, (_setFromToRotationTrans.position + Vector3.up) * 1.5f, Color.black);
        Debug.DrawLine(_setFromToRotationTrans.position, _setFromToRotationTrans.TransformPoint(Vector3.up * 1.5f), Color.yellow);
        Debug.DrawLine(_setFromToRotationTrans.position, (_setFromToRotationTrans.position + Vector3.forward) * 1.5f, Color.black);
        Debug.DrawLine(_setFromToRotationTrans.position, _setFromToRotationTrans.TransformPoint(Vector3.forward * 1.5f), Color.yellow);
        Debug.DrawLine(_setFromToRotationTrans.position, (_setFromToRotationTrans.position + Vector3.right) * 1.5f, Color.black);
        Debug.DrawLine(_setFromToRotationTrans.position, _setFromToRotationTrans.TransformPoint(Vector3.right * 1.5f), Color.yellow);

        //2、SetLookRotation方法：设置Quaternion实例的朝向
        //transform.forward方法与v1方向相同
        //transform.right垂直于由Vector3.zero、v1和v2这三点构成的平面
        //v2除了与Vector3.zero和v1构成平面来决定transform.right的方向外，
        //还用来决定transform.up的朝向，因为当transform.forward和transform.right方向确定后，
        //transform.up方向剩下两种可能，到底选用哪一种便由v2来影响
        //transform.up方向的选取方式总会使得transform.up的方向和v2方向的夹角小于或等于90度
        //一般情况下v2.normalized和transform.up是不相同的。
        //注意：
        //当v1为Vector3.zero时，方法失效
        //不可以直接使用transfrom.rotation.SetLookRotation（v1，v2）方式来使用SetLookRotation方法，否则会不起作用
        //应该首先实例化一个Quaternion，然后对其使用SetLookRotation，最后将其赋值给transform.rotation
        _lookRotationQuat.SetLookRotation(_eulerAnglesA.position, _eulerAnglesB.position);
        _setLookRotationTrans.rotation = _lookRotationQuat;
        //分别绘制A、B和C.right的朝向线
        Debug.DrawLine(_setLookRotationTrans.position, _setLookRotationTrans.TransformPoint(Vector3.right * 2.5f), Color.yellow);
        Debug.DrawLine(_setLookRotationTrans.position, _setLookRotationTrans.TransformPoint(Vector3.forward * 2.5f), Color.yellow);
        //分别打印C.right与A、B的夹角
        MoreDebug.MoreLog("C.right与A的夹角：" + Vector3.Angle(_setLookRotationTrans.right, _eulerAnglesA.position));
        MoreDebug.MoreLog("C.right与B的夹角：" + Vector3.Angle(_setLookRotationTrans.right, _eulerAnglesB.position));
        //C.up与B的夹角
        MoreDebug.MoreLog("C.up与B的夹角：" + Vector3.Angle(_setLookRotationTrans.up, _eulerAnglesB.position));
    }
}
