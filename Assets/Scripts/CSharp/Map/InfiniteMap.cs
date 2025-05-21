using UnityEngine;
using UnityEngine.Tilemaps;

public class InfiniteTilemap : MonoBehaviour
{
    public Tilemap[] tilemaps; // 按顺序存放 3 个 Tilemap
    public Transform player;   // 玩家 Transform
    public float tilemapWidth = 21f; // 每个 Tilemap 的宽度

    private void Update()
    {
        // 计算玩家的 X 位置相对于当前 Tilemap 的偏移
        float playerX = player.position.x;
        
        // 如果玩家接近右边界（比如走到第 2 个 Tilemap 的中间）
        if (playerX > tilemaps[1].transform.position.x)
        {
            // 把最左边的 Tilemap 移到最右边
            MoveLeftmostToRight();
        }
        // 如果玩家接近左边界（比如往回走）
        else if (playerX < tilemaps[0].transform.position.x)
        {
            // 把最右边的 Tilemap 移到最左边
            MoveRightmostToLeft();
        }
    }

    // 把最左边的 Tilemap 移到最右边
    private void MoveLeftmostToRight()
    {
        Tilemap leftmost = tilemaps[0];
        
        // 计算新的位置（最右边的 Tilemap 的右侧）
        float newX = tilemaps[2].transform.position.x + tilemapWidth;
        leftmost.transform.position = new Vector3(newX, leftmost.transform.position.y, 0);
        
        // 更新数组顺序（让 tilemaps[0] 变成 tilemaps[2]）
        Tilemap[] newOrder = { tilemaps[1], tilemaps[2], tilemaps[0] };
        tilemaps = newOrder;
    }

    // 把最右边的 Tilemap 移到最左边
    private void MoveRightmostToLeft()
    {
        Tilemap rightmost = tilemaps[2];
        
        // 计算新的位置（最左边的 Tilemap 的左侧）
        float newX = tilemaps[0].transform.position.x - tilemapWidth;
        rightmost.transform.position = new Vector3(newX, rightmost.transform.position.y, 0);
        
        // 更新数组顺序（让 tilemaps[2] 变成 tilemaps[0]）
        Tilemap[] newOrder = { tilemaps[2], tilemaps[0], tilemaps[1] };
        tilemaps = newOrder;
    }
}