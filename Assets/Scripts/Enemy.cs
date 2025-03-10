using UnityEngine;

public class Enemy : CellObject
{
    public int Health = 3;

    private int m_CurrentHealth;
    private Animator m_Animator;
    private bool m_FacingRight = true; // Track current facing direction

    private void Awake()
    {
        GameManager.Instance.TurnManager.OnTick += TurnHappened;
        m_Animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        GameManager.Instance.TurnManager.OnTick -= TurnHappened;
    }

    public override void Init(Vector2Int coord)
    {
        base.Init(coord);
        m_CurrentHealth = Health;
    }

    public override bool PlayerWantsToEnter()
    {
        m_CurrentHealth -= 1;

        if (m_CurrentHealth <= 0)
        {
            m_Animator.SetTrigger("Death");
            Destroy(gameObject, 0.5f);
        }
        else
        {
            m_Animator.SetTrigger("Hit");
        }

        return false;
    }

    bool MoveTo(Vector2Int coord, bool immediate)
    {
        var board = GameManager.Instance.BoardManager;
        var targetCell = board.GetCellData(coord);

        if (targetCell == null || !targetCell.Passable || targetCell.ContainedObject != null)
        {
            return false;
        }

        // Remove enemy from current cell
        var currentCell = board.GetCellData(m_Cell);
        currentCell.ContainedObject = null;

        // Add it to the next cell
        targetCell.ContainedObject = this;
        m_Cell = coord;

        // Flip sprite based on movement direction
        FlipSprite(coord.x - transform.position.x);

        if (immediate)
        {
            transform.position = board.CellToWorld(coord);
        }
        else
        {
            m_Animator.SetBool("Moving", true);
            transform.position = board.CellToWorld(coord);
        }

        return true;
    }

    void TurnHappened()
    {
        var playerCell = GameManager.Instance.PlayerController.cell;

        int xDist = playerCell.x - m_Cell.x;
        int yDist = playerCell.y - m_Cell.y;

        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        if ((xDist == 0 && absYDist == 1) || (yDist == 0 && absXDist == 1))
        {
            // Attack player if adjacent
            m_Animator.SetTrigger("Attack");
            GameManager.Instance.ChangeFood(1);
        }
        else
        {
            bool moved = false;
            if (absXDist > absYDist)
            {
                moved = TryMoveInX(xDist);
                if (!moved) moved = TryMoveInY(yDist);
            }
            else
            {
                moved = TryMoveInY(yDist);
                if (!moved) moved = TryMoveInX(xDist);
            }

            m_Animator.SetBool("Moving", moved);
        }
    }

    bool TryMoveInX(int xDist)
    {
        return xDist > 0 ? MoveTo(m_Cell + Vector2Int.right, false) : MoveTo(m_Cell + Vector2Int.left, false);
    }

    bool TryMoveInY(int yDist)
    {
        return yDist > 0 ? MoveTo(m_Cell + Vector2Int.up, false) : MoveTo(m_Cell + Vector2Int.down, false);
    }

    private void FlipSprite(float moveDirection)
    {
        if (moveDirection > 0 && !m_FacingRight)
        {
            // Moving right but facing left -> flip to right
            transform.localScale = new Vector3(1, 1, 1);
            m_FacingRight = true;
        }
        else if (moveDirection < 0 && m_FacingRight)
        {
            // Moving left but facing right -> flip to left
            transform.localScale = new Vector3(-1, 1, 1);
            m_FacingRight = false;
        }
    }
}
