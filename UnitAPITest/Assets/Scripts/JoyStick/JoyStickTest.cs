using UnityEngine;
using MoreFun;
public class JoyStickTest : MonoBehaviour {
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;
    private Rigidbody thisRigidbody;
    private Vector2 moveDir;
    private Vector3 moveVector;
    private VirtualJoyStick joyStick;
	// Use this for initialization
	void Start () {
        //thisRigidbody = GetComponent<Rigidbody>();
        joyStick = GameObject.FindGameObjectWithTag("JoyStick").GetComponent<VirtualJoyStick>();
	}
	
	// Update is called once per frame
	void Update () {
        moveDir = PoolInput();
        Move();
        Rotate();
	}

    private void Move()
    {
        moveVector.x = moveDir.x;
        moveVector.z = moveDir.y;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    private void Rotate()
    {
        if (Vector3.zero == moveVector)
        {
            return;
        }

        Vector2 targetForward = new Vector2(moveDir.x, moveDir.y);
        float yRotAngle = Vector2.Angle(Vector2.up, targetForward);
        yRotAngle = moveDir.x < 0 ? -yRotAngle : yRotAngle;
        Quaternion target = Quaternion.Euler(new Vector3(0, yRotAngle, 0));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotateSpeed * Time.deltaTime);
    }

    private Vector2 PoolInput()
    {
        Vector2 dir = Vector2.zero;

        dir.x = joyStick.Horizontal();
        dir.y = joyStick.Vertical();

        if (dir.magnitude > 1)
	    {
            dir.Normalize();
	    }
        return dir;
    }
}
