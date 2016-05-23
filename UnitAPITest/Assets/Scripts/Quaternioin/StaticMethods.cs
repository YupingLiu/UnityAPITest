using UnityEngine;
using MoreFun;
using UnityEngine.UI;
public class StaticMethods : MonoBehaviour
{
    // 1、Angle方法：Quaternion实例间夹角
    private Quaternion q1 = Quaternion.identity;
    private Quaternion q2 = Quaternion.identity;
    private Vector3 eulerAngle = Vector3.zero;
    private float speed = 10.0f;

    // 2、Dot方法：点乘
    public Transform _dotA, _dotB;
    private float f;

    // 3、Euler方法：欧拉角对应的四元数
    // 记录欧拉角，单位为角度
    public float ex, ey, ez;
    // 用于记录计算结果
    private float qx, qy, qz, qw;
    // 常量
    private float PIover180 = Mathf.PI / 180;
    private Quaternion q = Quaternion.identity;
    public Button calculateEuler;

    // 4、FromToRotation方法：Quaternion变化
    public Transform A, B, C, D;

    public void Start()
    {
        // 1、Angle方法：Quaternion实例间夹角
        // 此方法用于返回从参数a到参数b变换的夹角。
        // 需要注意的是，返回的夹角不是某个局部坐标轴向变换的夹角，
        // 而是GameObject对象从状态a转换到状态b时需要旋转的最小夹角。
        q1.eulerAngles = new Vector3(10.0f, 20.0f, 30.0f);
        float f1 = Quaternion.Angle(q1, q2);
        float f2 = 0.0f;
        Vector3 v1 = Vector3.zero;
        q1.ToAngleAxis(out f2, out v1);
        MoreDebug.MoreLog("angle from a to b : " + f1);
        MoreDebug.MoreLog("angle from identity to a : " + f2);
        MoreDebug.MoreLog("q1的欧拉角 : " + q1.eulerAngles + "q1的rotation： " + q1);
        MoreDebug.MoreLog("q2的欧拉角 : " + q2.eulerAngles + "q2的rotation： " + q2);

        MoreDebug.MoreLog("biubiubiubiubiu~我是分隔线~biubiubiubiubiu~");

        // 2、Dot方法：点乘
        // float f = Quaternion.Dot(q1, q2) = x1 * x2 + y1 * y2 + z1 * z2 + w1 * w2; f的范围为[-1, 1]
        // 当f = +-1时，q1和q2对应的欧拉角是相等的，即在Game视图中看来它们的旋转状态是一样的
        // 当f = 1时，它们的rotation相等；
        // 当f = -1时，说明其中一个rotation比另一个多旋转了一周即360度
        _dotA.eulerAngles = new Vector3(0.0f, 40.0f, 0.0f);
        // B比A绕y轴多转360度
        _dotB.eulerAngles = new Vector3(0.0f, 360.0f + 40.0f, 0.0f);
        q1 = _dotA.rotation;
        q2 = _dotB.rotation;
        f = Quaternion.Dot(q1, q2);
        MoreDebug.MoreLog("q1的rotation : " + q1);
        MoreDebug.MoreLog("q2的rotation : " + q2);
        MoreDebug.MoreLog("q1的欧拉角 : " + q1.eulerAngles);
        MoreDebug.MoreLog("q2的欧拉角 : " + q2.eulerAngles);
        MoreDebug.MoreLog("Dot(q1, q2) : " + f);
    }

    public void OnUpdate()
    {
        // 4、FromToRotation方法：Quaternion变化

    }

    public void OnClick()
    {
        MoreDebug.MoreLog("欧拉角： " + "ex: " + ex + " ey: " + ey + " ez: " + ez);
        // 调用方法计算
        q = Quaternion.Euler(ex, ey, ez);
        MoreDebug.MoreLog("Q.x: " + q.x + " Q.y: " + q.y + " Q.z: " + q.z + " Q.w: " + q.w);
        // 测试算法
        ex = ex * PIover180 / 2.0f;
        ey = ey * PIover180 / 2.0f;
        ez = ez * PIover180 / 2.0f;
        qx = Mathf.Sin(ex) * Mathf.Cos(ey) * Mathf.Cos(ez) + Mathf.Cos(ex) * Mathf.Sin(ey) * Mathf.Sin(ez);
        qy = Mathf.Cos(ex) * Mathf.Sin(ey) * Mathf.Cos(ez) - Mathf.Sin(ex) * Mathf.Cos(ey) * Mathf.Sin(ez);
        qz = Mathf.Cos(ex) * Mathf.Cos(ey) * Mathf.Sin(ez) - Mathf.Sin(ex) * Mathf.Sin(ey) * Mathf.Cos(ez);
        qw = Mathf.Cos(ex) * Mathf.Cos(ey) * Mathf.Cos(ez) + Mathf.Sin(ex) * Mathf.Sin(ey) * Mathf.Sin(ez);
        MoreDebug.MoreLog("qx: " + qx + " qy: " + qy + " qz: " + qz + " qw: " + qw);
    }
}

