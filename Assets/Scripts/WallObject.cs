using UnityEngine;
using UnityEngine.Tilemaps;
public class WallObject : CellObject
{
    public Tile ObstacleTile;
    public Tile DamagedObstacleTile; // Gạch tường bị hư hại
    public int MaxHealth = 3;

    private int m_HealthPoint;
    private Tile m_OriginalTile;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);

        m_HealthPoint = MaxHealth;

        m_OriginalTile = GameManager.Instance.BoardManager.GetCellTile(cell);
        GameManager.Instance.BoardManager.SetCellTile(cell, ObstacleTile);
    }

    public override bool PlayerWantsToEnter()
    {
        m_HealthPoint -= 1;
        Debug.Log("Số máu của tường còn lại: " + m_HealthPoint);

        if (m_HealthPoint == 1)
        {
            Debug.Log("Tường gần bị phá hủy, chuyển sang gạch hư hại!");
            GameManager.Instance.BoardManager.SetCellTile(m_Cell, DamagedObstacleTile);
        }

        if (m_HealthPoint > 0)
        {
            return false;
        }

        Debug.Log("Tường bị phá hủy!");
        GameManager.Instance.BoardManager.SetCellTile(m_Cell, m_OriginalTile);
        Destroy(gameObject);
        return true;
    }

}
