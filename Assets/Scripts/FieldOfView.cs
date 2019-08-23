using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    private enum Mode { inactive, passive}
    [Header("Required")]
    [SerializeField]
    private MeshFilter viewMeshFilter = null;

    [Header("Customizables")]
    [SerializeField]
    private Mode runtimeMode = Mode.inactive;
    [SerializeField]
    private float viewRadius = 10;
    [Range(0, 360)]
    [SerializeField]
    private float viewAngle = 20;

    [SerializeField]
    private LayerMask targetMask = 0;
    [SerializeField]
    private LayerMask obstacleMask = 0;
    [SerializeField]
    private float meshResolution = 5;
    [SerializeField]
    private int edgeResultIterations = 4;
    [SerializeField]
    private float edgeDistanceThreshold = 0.5F;

    [Header("Monitoring Purpose")]
    [SerializeField]
    private List<Transform> visibleTargets = new List<Transform>();

    private Mesh viewMesh;

    void Awake()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        if (runtimeMode == Mode.passive)
            FindTargetsWithDelay(0.2F);
    }

    /*
    private void LateUpdate()
    {
        DrawFieldOfView();
    }
    */
    private void FindTargetsWithDelay(float delay)
    {
        StartCoroutine(FindTargetsWithDelayContainer(delay));
    }

    private IEnumerator FindTargetsWithDelayContainer(float delay)
    {
        WaitForSeconds updateFrequency = new WaitForSeconds (delay);
        while (true)
        {
            FindVisibleTargets();
            yield return updateFrequency;
        }
    }

    private void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(viewMeshFilter.transform.position, viewRadius, targetMask);

        for(int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 directionToTarget = (target.position - viewMeshFilter.transform.position).normalized;
            if (Vector3.Angle(viewMeshFilter.transform.forward, directionToTarget) < viewAngle /2)
            {
                float distanceToTarget = Vector3.Distance(viewMeshFilter.transform.position, target.position);

                if (!Physics.Raycast(viewMeshFilter.transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    public void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = viewMeshFilter.transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                        viewPoints.Add(edge.pointA);
                    if (edge.pointB != Vector3.zero)
                        viewPoints.Add(edge.pointB);
                }
            }
            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[( vertexCount - 2 ) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = viewMeshFilter.transform.InverseTransformPoint(viewPoints [i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    public void ClearFieldOfView()
    {
        viewMesh.Clear();
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResultIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDistanceThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            } else
            {
                minAngle = angle;
                maxPoint = newViewCast.point;
            }
        }
        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast (float globalAngle)
    {
        Vector3 dir = DirectionFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(viewMeshFilter.transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, viewMeshFilter.transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += viewMeshFilter.transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public Transform GetClosestTarget()
    {
        FindVisibleTargets();
        Transform currentClosestTarget = null;

        foreach(Transform target in visibleTargets)
        {
            if (currentClosestTarget == null)
                currentClosestTarget = target;

            if (Vector3.Distance(currentClosestTarget.position, viewMeshFilter.transform.position) > Vector3.Distance(target.position, viewMeshFilter.transform.position))
                currentClosestTarget = target;
        }
        return currentClosestTarget;
    }

    public float GetViewRadius()
    {
        return viewRadius;
    }

    public float GetViewAngle()
    {
        return viewAngle;
    }

    public List<Transform> GetVisibleTargets()
    {
        return visibleTargets;
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
