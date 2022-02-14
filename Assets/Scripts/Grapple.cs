using UnityEngine;

public class Grapple : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    [SerializeField] float maxDistance = 20f;
    [SerializeField] float _grapplrPointHeight = 2f;
    [SerializeField] float _grapplrPointDis = 5;
    private ConfigurableJoint joint;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartGrapple();
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            StopGrapple();
        }
        Debug.DrawLine(gunTip.position, player.position + (player.up * _grapplrPointHeight + player.forward * _grapplrPointDis), Color.red);
    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Linecast(gunTip.position, player.position + (player.up * _grapplrPointHeight + player.forward * _grapplrPointDis), out hit))
        {
            grapplePoint = hit.point;
        }
        else
        {
            grapplePoint = player.position + (player.up * 1.5f + player.forward * 4f);
            Debug.DrawLine(gunTip.position, player.position + (player.up * 1.5f + player.forward * 4f), Color.red);
        }
        joint = player.gameObject.AddComponent<ConfigurableJoint>();
        SoftJointLimitSpring SoftJointLimitSpring = joint.linearLimitSpring;
        SoftJointLimit SoftJointLimit = joint.linearLimit;
        JointDrive jointxDrive = joint.xDrive;
        JointDrive jointyDrive = joint.xDrive;
        JointDrive jointzDrive = joint.zDrive;

        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = new Vector3(0, 0.8f, 0);
        joint.connectedAnchor = grapplePoint;

        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;
        SoftJointLimitSpring.spring = 5;
        SoftJointLimit.limit = 0.1f;
        jointxDrive.positionSpring = 200;
        jointyDrive.positionSpring = 200;
        jointyDrive.positionDamper = 50;
        jointzDrive.positionSpring = 200;

        joint.linearLimitSpring = SoftJointLimitSpring;
        joint.linearLimit = SoftJointLimit;
        joint.xDrive = jointxDrive;
        joint.yDrive = jointyDrive;
        joint.zDrive = jointzDrive;

        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
