using UnityEngine;
using UnityEngine.Tilemaps;

public class InfiniteTilemap : MonoBehaviour
{
    public Tilemap[] tilemaps; // 按序存放3个Tilemap
    public Transform player;
    public float tilemapWidth = 21f; // 每个Tilemap的宽度

    private void Update()
    {
        float playerX = player.position.x;
        
        if (playerX > tilemaps[1].transform.position.x)
        {
            MoveLeftmostToRight();
        }
        else if (playerX < tilemaps[0].transform.position.x)
        {
            MoveRightmostToLeft();
        }
    }

    // 把最左边的 Tilemap 移到最右边
    private void MoveLeftmostToRight()
    {
        Tilemap leftmost = tilemaps[0];
        
        // 计算新位置
        float newX = tilemaps[2].transform.position.x + tilemapWidth;
        leftmost.transform.position = new Vector3(newX, leftmost.transform.position.y, 0);
        
        // 更新数组顺序
        Tilemap[] newOrder = { tilemaps[1], tilemaps[2], tilemaps[0] };
        tilemaps = newOrder;
    }

    // 把最右边的 Tilemap 移到最左边
    private void MoveRightmostToLeft()
    {
        Tilemap rightmost = tilemaps[2];
        
        float newX = tilemaps[0].transform.position.x - tilemapWidth;
        rightmost.transform.position = new Vector3(newX, rightmost.transform.position.y, 0);
        
        Tilemap[] newOrder = { tilemaps[2], tilemaps[0], tilemaps[1] };
        tilemaps = newOrder;
    }
}