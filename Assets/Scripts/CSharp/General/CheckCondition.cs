using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCondition : MonoBehaviour
{
    [Header("检查范围")] 
    public Vector2 checkSize;
    public Vector2 checkPos;
    public float offsetX = 1f;
    public float offsetY = 1f;
    public LayerMask targetLayer;

    private void FixedUpdate()
    {
        checkPos.x = transform.position.x + offsetX;
        checkPos.y = transform.position.y + offsetY;
    }

    public bool CheckTarget()
    {
        Collider2D hit = Physics2D.OverlapBox(checkPos, checkSize, 0f, targetLayer);
        if (hit)
        {
            Debug.Log($"{gameObject.name} 检测到目标对象: " + hit.gameObject.name);
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(checkPos, checkSize);
    }
}