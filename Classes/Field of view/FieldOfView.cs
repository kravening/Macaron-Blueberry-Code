using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {
    public float viewRadius;
    public int raycasts;
    public int edgeResolveIterations = 4;
    public int test;

    int layerMask = 1 << 9;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    private void Start()
    {
        viewMesh = new Mesh
        {
            name = "View Mesh"
        };
        viewMeshFilter.mesh = viewMesh;
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    public float AngleFromDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(-dir.x, -dir.y) * Mathf.Rad2Deg;

        return 90 - angle;
    }

    public Vector2 DirectionFromAngle(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        return dir;
        
    }

    public void DrawFieldOfView()
    {
        float stepAngleSize = 360 / raycasts;
        List<Vector3> viewpoints= new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i < raycasts; i++)
        {

            float angle = stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                if(oldViewCast.hit != newViewCast.hit || oldViewCast.collider != newViewCast.collider)
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if(edge.pointA != Vector3.zero)
                    {
                        viewpoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewpoints.Add(edge.pointB);
                    }
                }
            }

           
            viewpoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        //Mesh Generation
        int vertexCount = viewpoints.Count; // number of points
        Vector3[] vertices = new Vector3[vertexCount]; //position of points
        int[] triangles = new int[(vertexCount - 1) * 3]; //references to points in pairs of 3, for generating triangles
        vertices[0] = Vector3.zero;

        int lastIndex = 0; 

        for (int i = 0; i < vertexCount-2; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewpoints[i]); //transforms world position to local position

            if( i < vertexCount - 2) //chooces vertexes for triangles
            {
                triangles[i * 3] = 0; //lightcaster position
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
            lastIndex = i;
        }

        lastIndex++;

        triangles[lastIndex * 3] = 0; //lightcaster position
        triangles[lastIndex * 3 + 1] = lastIndex;
        triangles[lastIndex * 3 + 2] = 1;

        viewMesh.Clear(); // clears previous mesh

        //generate new mesh
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge( ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);
            if(newViewCast.hit == minViewCast.hit)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }
        return new EdgeInfo(minPoint,maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector2 dir = DirFromAngle(globalAngle, true);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, layerMask);
        if (hit.collider != null){
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle,hit.collider);
           
        }
        else
        {
            return new ViewCastInfo(false, new Vector2(transform.position.x, transform.position.y) + dir * viewRadius, viewRadius, globalAngle,null);
        }
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad),0);
        
    }
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector2 point;
        public float dst;
        public float angle;
        public Collider2D collider;

        public ViewCastInfo(bool _hit, Vector2 _point, float _dst, float _angle, Collider2D _collider)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
            collider = _collider;
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
