using System.Collections.Generic;
using UnityEngine;
using Vector2f = UnityEngine.Vector2;
using Vector2i = ClipperLib.IntPoint;

using int64 = System.Int64;

public class DestructibleTerrain : MonoBehaviour
{
    [SerializeField]
    private Material _material; public Material MaterialTerrail 
    { get { return _material; } }
    [SerializeField]
    private BoxCollider2D _coliderMain;
    [SerializeField]
    private Color _colorGizmo = Color.blue;

    [SerializeField]
    private bool _thermalInfluence;
    [SerializeField]
    [Range(-100, 100)]
    private float _percentageOfThermalInFluence;
    [SerializeField]
    [Range(0.5f, 1.0f)]
    private float _blockSize; public float BlockSize { get { return _blockSize; } }

    private int64 _blockSizeScaled;

    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float _simplifyEpsilonPercent;

    [SerializeField]
    [Range(1, 100)]
    private int _resolutionX = 10;

    [SerializeField]
    [Range(1, 100)]
    private int _resolutionY = 10; public int ResolutionY 
    { get { return _resolutionY; } }

    [SerializeField]
    private float _depth = 1.0f;public float Depth 
    { get { return _depth; } }
    private float _width, _height;


    private DestructibleBlock[] _blocks;

    private void Awake()
    {
        BlockSimplification.epsilon = (int64)(_simplifyEpsilonPercent / 100f * _blockSize * VectorEx.float2int64);

        _width = _blockSize * _resolutionX;
        _height = _blockSize * _resolutionY;
        _blockSizeScaled = (int64)(_blockSize * VectorEx.float2int64);

        _coliderMain.size = new Vector2(_resolutionX, _resolutionY) * _blockSize;
        _coliderMain.offset = new Vector2((float)_resolutionX /2, (float)_resolutionY /2) * _blockSize;

        Initialize();
    }

    public void Initialize()
    {
        //
        _blocks = new DestructibleBlock[_resolutionX * _resolutionY];

        for (int x = 0; x < _resolutionX; x++)
        {
            for (int y = 0; y < _resolutionY; y++)
            {
                List<List<Vector2i>> polygons = new List<List<Vector2i>>();

                List<Vector2i> vertices = new List<Vector2i>();
                vertices.Add(new Vector2i { x = x * _blockSizeScaled, y = (y + 1) * _blockSizeScaled });
                vertices.Add(new Vector2i { x = x * _blockSizeScaled, y = y * _blockSizeScaled });
                vertices.Add(new Vector2i { x = (x + 1) * _blockSizeScaled, y = y * _blockSizeScaled });
                vertices.Add(new Vector2i { x = (x + 1) * _blockSizeScaled, y = (y + 1) * _blockSizeScaled });

                polygons.Add(vertices);

                int idx = x + _resolutionX * y;

                DestructibleBlock block = CreateBlock();
                _blocks[idx] = block;

                UpdateBlockBounds(x, y);

                block.UpdateGeometryWithMoreVertices(polygons, _width, _height, _depth);
                if(_thermalInfluence)
                block.gameObject.AddComponent<Liquid>().Initialization(_percentageOfThermalInFluence);
            }
        }
    }

    public Vector2 GetPositionOffset()
    {
        return transform.position;
    }

    private DestructibleBlock CreateBlock()
    {
        GameObject childObject = new GameObject();
        childObject.name = "DestructableBlock";
        childObject.transform.SetParent(transform);
        childObject.transform.localPosition = Vector3.zero;

        DestructibleBlock blockComp = childObject.AddComponent<DestructibleBlock>();
        blockComp.SetMaterial(_material);

        return blockComp;
    }
    private void UpdateBlockBounds(int x, int y)
    {
        int lx = x;
        int ly = y;
        int ux = x + 1;
        int uy = y + 1;

        if (lx == 0) lx = -1;
        if (ly == 0) ly = -1;
        if (ux == _resolutionX) ux = _resolutionX + 1;
        if (uy == _resolutionY) uy = _resolutionY + 1;

        BlockSimplification.currentLowerPoint = new Vector2i
        {
            x = lx * _blockSizeScaled,
            y = ly * _blockSizeScaled
        };

        BlockSimplification.currentUpperPoint = new Vector2i
        {
            x = ux * _blockSizeScaled,
            y = uy * _blockSizeScaled
        };
    }
    public void ExecuteClip(IClip clip)
    {
        BlockSimplification.epsilon = (int64)(_simplifyEpsilonPercent / 100f * _blockSize * VectorEx.float2int64);

        List<Vector2i> clipVertices = clip.GetVertices();

        ClipBounds bounds = clip.GetBounds();
        int x1 = Mathf.Max(0, (int)(bounds.lowerPoint.x / _blockSize));
        if (x1 > _resolutionX - 1) return;
        int y1 = Mathf.Max(0, (int)(bounds.lowerPoint.y / _blockSize));
        if (y1 > _resolutionY - 1) return;
        int x2 = Mathf.Min(_resolutionX - 1, (int)(bounds.upperPoint.x / _blockSize));
        if (x2 < 0) return;
        int y2 = Mathf.Min(_resolutionY - 1, (int)(bounds.upperPoint.y / _blockSize));
        if (y2 < 0) return;

        for (int x = x1; x <= x2; x++)
        {
            for (int y = y1; y <= y2; y++)
            {
                if (clip.CheckBlockOverlapping(new Vector2f((x + 0.5f) * _blockSize, (y + 0.5f) * _blockSize), _blockSize))
                {
                    DestructibleBlock block = _blocks[x + _resolutionX * y];

                    List<List<Vector2i>> solutions = new List<List<Vector2i>>();

                    ClipperLib.Clipper clipper = new ClipperLib.Clipper();
                    clipper.AddPolygons(block.Polygons, ClipperLib.PolyType.ptSubject);
                    clipper.AddPolygon(clipVertices, ClipperLib.PolyType.ptClip);
                    clipper.Execute(ClipperLib.ClipType.ctDifference, solutions,
                        ClipperLib.PolyFillType.pftNonZero, ClipperLib.PolyFillType.pftNonZero);

                    UpdateBlockBounds(x, y);

                    block.UpdateGeometryWithMoreVertices(solutions, _width, _height, _depth);
                }
                
            }
        }      
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _colorGizmo;
        Gizmos.DrawWireCube(transform.position+ (new Vector3((float)_resolutionX / 2, (float)_resolutionY / 2,0)*_blockSize), new Vector2(_resolutionX, _resolutionY) * _blockSize);
    }
    
}
