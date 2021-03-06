using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vector2i = ClipperLib.IntPoint;
using Vector2f = UnityEngine.Vector2;


public class RuntimeCircleScorcher : MonoBehaviour, IClip
{
    private struct TouchLineOverlapCheck
    {
        public float a;
        public float b;
        public float c;
        public float angle;
        public float dividend;

        public TouchLineOverlapCheck(Vector2 p1, Vector2 p2)
        {
            Vector2 d = p2 - p1;
            float m = d.magnitude;
            a = -d.y / m;
            b = d.x / m;
            c = -(a * p1.x + b * p1.y);
            angle = Mathf.Rad2Deg * Mathf.Atan2(-a, b);

            float da;
            if (d.x / d.y < 0f)
                da = 45 + angle;
            else
                da = 45 - angle;

            dividend = Mathf.Abs(1.0f / 1.4f * Mathf.Cos(Mathf.Deg2Rad * da));
        }

        public float GetDistance(Vector2 p)
        {
            return Mathf.Abs(a * p.x + b * p.y + c);
        }
    }

    [SerializeField]
    private BallMove _ballMove;
    private List<DestructibleTerrain> _terrains = new List<DestructibleTerrain>();
    private Vector3 _offSet
    {
        get
        {
            return _ballMove.GetDirectionMove().normalized * _OffsetY;
        }
    }
    private Vector2f currentTouchPoint;
    private Vector2f previousTouchPoint;
    private TouchPhase touchPhase;
    private TouchLineOverlapCheck touchLine;
    private List<Vector2i> vertices = new List<Vector2i>();
    //private Camera mainCamera;

    [SerializeField]
    private float _radius = 1.2f ,_OffsetY = -0.2f;
    [SerializeField]
    private int segmentCount = 10;
    [SerializeField]


    private void Awake()
    {
        //mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        BurningOut();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var terrain = collision.GetComponent<DestructibleTerrain>();

        if (terrain != null)
        {
            _terrains.Add(terrain);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var terrain = collision.GetComponent<DestructibleTerrain>();

        if (terrain != null)
        {
            _terrains.Remove(terrain);
        }
    }
    private void BurningOut()
    {
        Vector2 ballPosition = transform.position + _offSet;
        for (int i = 0; i < _terrains.Count; i++)
        {
            currentTouchPoint = ballPosition - _terrains[i].GetPositionOffset();

            BuildVertices(currentTouchPoint);

            _terrains[i].ExecuteClip(this);
        }
    }
    private void BuildVertices(Vector2 center)
    {
        vertices.Clear();
        for (int i = 0; i < segmentCount; i++)
        {
            float angle = Mathf.Deg2Rad * (-90f - 360f / segmentCount * i);

            Vector2 point = new Vector2(center.x + _radius * Mathf.Cos(angle), center.y + _radius * Mathf.Sin(angle));
            Vector2i point_i64 = point.ToVector2i();
            vertices.Add(point_i64);
        }
    }
    private void BuildVertices(Vector2 begin, Vector2 end)
    {
        vertices.Clear();
        int halfSegmentCount = segmentCount / 2;
        touchLine = new TouchLineOverlapCheck(begin, end);

        for (int i = 0; i <= halfSegmentCount; i++)
        {
            float angle = Mathf.Deg2Rad * (touchLine.angle + 270f - (float)360f / segmentCount * i);
            Vector2 point = new Vector2(begin.x + _radius * Mathf.Cos(angle), begin.y + _radius * Mathf.Sin(angle));
            Vector2i point_i64 = point.ToVector2i();
            vertices.Add(point_i64);
        }

        for (int i = halfSegmentCount; i <= segmentCount; i++)
        {
            float angle = Mathf.Deg2Rad * (touchLine.angle + 270f - (float)360f / segmentCount * i);
            Vector2 point = new Vector2(end.x + _radius * Mathf.Cos(angle), end.y + _radius * Mathf.Sin(angle));
            Vector2i point_i64 = point.ToVector2i();
            vertices.Add(point_i64);
        }
    }
    public bool CheckBlockOverlapping(Vector2f p, float size)
    {
        if (touchPhase == TouchPhase.Began)
        {
            float dx = Mathf.Abs(currentTouchPoint.x - p.x) - _radius - size / 2;
            float dy = Mathf.Abs(currentTouchPoint.y - p.y) - _radius - size / 2;
            return dx < 0f && dy < 0f;
        }
        else if (touchPhase == TouchPhase.Moved)
        {
            float distance = touchLine.GetDistance(p) - _radius - size / touchLine.dividend;
            return distance < 0f;
        }
        else
            return false;
    }
    public ClipBounds GetBounds()
    {
        if (touchPhase == TouchPhase.Began)
        {
            return new ClipBounds
            {
                lowerPoint = new Vector2f(currentTouchPoint.x - _radius, currentTouchPoint.y - _radius),
                upperPoint = new Vector2f(currentTouchPoint.x + _radius, currentTouchPoint.y + _radius)
            };
        }
        else if (touchPhase == TouchPhase.Moved)
        {
            Vector2f upperPoint = currentTouchPoint;
            Vector2f lowerPoint = previousTouchPoint;
            if (previousTouchPoint.x > currentTouchPoint.x)
            {
                upperPoint.x = previousTouchPoint.x;
                lowerPoint.x = currentTouchPoint.x;
            }
            if (previousTouchPoint.y > currentTouchPoint.y)
            {
                upperPoint.y = previousTouchPoint.y;
                lowerPoint.y = currentTouchPoint.y;
            }

            return new ClipBounds
            {
                lowerPoint = new Vector2f(lowerPoint.x - _radius, lowerPoint.y - _radius),
                upperPoint = new Vector2f(upperPoint.x + _radius, upperPoint.y + _radius)
            };
        }
        else
            return new ClipBounds();
    }
    public List<Vector2i> GetVertices()
    {
        return vertices;
    }
    public bool ContactDestructibleTerrain()
    {
        return _terrains.Count > 0;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + _offSet, _radius);
    }
}
