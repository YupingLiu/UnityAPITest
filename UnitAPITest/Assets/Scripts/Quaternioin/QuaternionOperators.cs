using MoreFun;
using UnityEngine;
public class QuaternionOperators : MonoBehaviour {

    // 1、operator * (lhs: Quaternion, rhs: Quaternion)
    public Transform A, B;
    // 2、operator * (lhs: Quaternion, rhs: Vector3)
    private float speed = 0.1f;
    public Transform C;

	// Use this for initialization
	void Start () {
	    // 设置A的欧拉角
        A.eulerAngles = new Vector3(1.0f, 1.5f, 2.0f);
        // 初始化C的position和eulerAngles
        C.position = Vector3.zero;
        C.eulerAngles = new Vector3(0.0f, 45.0f, 0.0f);
	}
	
	// Update is called once per frame
    void Update()
    {
        // 1、operator * (lhs: Quaternion, rhs: Quaternion)
        // 此运算符用于返回两个Quaternion实例相乘后的结果。
        // 设A和B均为GameObject对象的一个实例，有如下代码：
        // B.rotation *= A.rotation;
        // 则：
        // 1）代码每执行一次，B都会绕着B的局部坐标系的z、x、y轴分别旋转A.eulerAngles.z度、
        // A.eulerAngles.x度和A.eulerAngles.y度，注意它们的旋转次序一定是先绕z轴再绕x轴最后绕y轴进行相应的旋转
        // 另外由于是绕着局部坐标系旋转，故而当绕着其中一个轴旋转时，很可能会影响其余两个坐标轴方向的欧拉角
        //（除非其余两轴的欧拉角都为0才不受影响）
        // 2）设A的欧拉角都为euler_a(ax, ay, az)，则沿着B的初始局部坐标系的euler_a方向向下看，会发现B在绕着
        // euler_a顺时针旋转。B的旋转状况还受其初始状态的影响。
        // 3）此方法主要用于物体自身旋转变换。

        // B绕着其自身坐标系的Vector3(1.0f, 1.5f, 2.0f)方向旋转，虽然每次绕这个轴向旋转的角度相同，
        // 但角度的旋转在3个坐标轴上的值都不为零，其中一轴的旋转会影响其他两轴的角度，故而B的欧拉角
        // 的各个分量的每次递增值是不固定的。
        B.rotation *= A.rotation;
        // 输出B的欧拉角，注意观察B的欧拉角变化
        MoreDebug.MoreLog(B.eulerAngles);

        // 2、operator * (lhs: Quaternion, rhs: Vector3)
        // 此运算符的作用是对参数坐标点point进行rotation变换。
        // 例如，设A为Vector3实例，有如下代码：
        // transform.position += transform.rotation * A;
        // 则每执行一次代码，transform对应的对象便会沿着自身坐标系中变量A的方向移动A的模长的距离。
        // transform.rotation与A相乘主要来确定移动的方向和距离。

        // 沿着C自身坐标系的forward方向每帧前进speed距离
        C.position = C.rotation * (Vector3.forward * speed);
        MoreDebug.MoreLog(C.position);
    }
}
