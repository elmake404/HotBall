using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vector2i = ClipperLib.IntPoint;

public class AwakeCircleClipper : MonoBehaviour, IClip
{
    [SerializeField]
    private DestructibleTerrain[] _terrains;

    [SerializeField]
    private float _diameter = 1.2f;

    private float _radius = 1.2f;
    [SerializeField]
    public int _segmentCount = 10;

    private Vector2 _clipPosition;
    [SerializeField]
    private CircleCollider2D _collider2D;

    public bool CheckBlockOverlapping(Vector2 p, float size)
    {       
        float dx = Mathf.Abs(_clipPosition.x - p.x) - _radius - size / 2;
        float dy = Mathf.Abs(_clipPosition.y - p.y) - _radius - size / 2;

        return dx < 0f && dy < 0f;      
    }

    public ClipBounds GetBounds()
    {      
        return new ClipBounds
        {
            lowerPoint = new Vector2(_clipPosition.x - _radius, _clipPosition.y - _radius),
            upperPoint = new Vector2(_clipPosition.x + _radius, _clipPosition.y + _radius)
        };             
    }

    public List<Vector2i> GetVertices()
    {
        List<Vector2i> vertices = new List<Vector2i>();
        for (int i = 0; i < _segmentCount; i++)
        {
            float angle = Mathf.Deg2Rad * (-90f - 360f / _segmentCount * i);

            Vector2 point = new Vector2(_clipPosition.x + _radius * Mathf.Cos(angle), _clipPosition.y + _radius * Mathf.Sin(angle));
            Vector2i point_i64 = point.ToVector2i();
            vertices.Add(point_i64);
        }
        return vertices;
    }

    void Awake()
    {
        _radius = _diameter / 2f;
        _collider2D.radius = _radius;
    }

    void Start()
    {
        Vector2 positionWorldSpace = transform.position;
        for (int i = 0; i < _terrains.Length; i++)
        {
            _clipPosition = positionWorldSpace - _terrains[i].GetPositionOffset();

            _terrains[i].ExecuteClip(this);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position , _diameter / 2f);
    }

}
