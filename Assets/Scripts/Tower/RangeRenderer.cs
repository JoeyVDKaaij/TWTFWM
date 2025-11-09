using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(LineRenderer))]
public class RangeRenderer : MonoBehaviour
{
    [Header("Range Circle Settings")]
    [SerializeField, Tooltip("Set how many lines the range circle should have.")]
    private int segments = 50;
    [SerializeField, Tooltip("Set the width of the range circle.")]
    private float rangeCircleWidth = 0.1f;
    
    [SerializeField, Tooltip("Set to true if the range circle should be updated per frame.")]
    private bool updateRangeCirclePerFrame = false;

    private LineRenderer _line;
    private TowerAttack _attackScript;
    private float _radius;

    void Start()
    {
        _line = GetComponent<LineRenderer>();
        _attackScript = GetComponent<TowerAttack>();
        if (_attackScript != null)
            _radius = _attackScript.CurrentAttackRange;
        
        SetupCircle();
    }

    public void UpdateCircle(bool pUseAttackScript, float pRadius = 0)
    {
        if (pUseAttackScript)
        {
            if (_attackScript == null)
            {
                _attackScript = GetComponent<TowerAttack>();
                if (_attackScript == null) return;
            }

            _radius = _attackScript.CurrentAttackRange;
        }
        else
        {
            _radius = pRadius;
        }
        
        SetupCircle();
    }

    private void SetupCircle()
    {
        if (_line == null) return;
        
        _line.positionCount = segments + 1;
        _line.loop = true;
        _line.startWidth = rangeCircleWidth;
        _line.endWidth = rangeCircleWidth;
        
        DrawCircle();
    }

    private void DrawCircle()
    {
        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * _radius;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * _radius;
            _line.SetPosition(i, transform.position + new Vector3(x, 0, z));
            angle += 360f / segments;
        }
    }

    private void Update()
    {
        if (updateRangeCirclePerFrame) DrawCircle();
    }
}