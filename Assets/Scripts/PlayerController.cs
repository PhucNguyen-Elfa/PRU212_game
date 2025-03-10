using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 5.0f;
    public float AttackMoveDistance = 0.2f; // Distance to move forward during attack
    public float AttackMoveDuration = 0.1f; // Time taken to move forward and back

    private bool m_IsMoving;
    private Vector3 m_MoveTarget;
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;
    private bool m_IsGameOver;
    public Vector2Int cell => m_CellPosition;

    private Animator m_Animator;
    private Vector2Int m_FacingDirection = Vector2Int.right; // Default facing right

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        MoveTo(cell, true);
    }

    public void GameOver()
    {
        m_IsGameOver = true;
    }

    public void ResetAttack()
    {
        m_Animator.ResetTrigger("Attack");
    }

    public void MoveTo(Vector2Int cell, bool immediate)
    {
        m_CellPosition = cell;

        if (immediate)
        {
            m_IsMoving = false;
            transform.position = m_Board.CellToWorld(m_CellPosition);
        }
        else
        {
            m_IsMoving = true;
            m_MoveTarget = m_Board.CellToWorld(m_CellPosition);
        }

        m_Animator.SetBool("Moving", m_IsMoving);
        m_Animator.ResetTrigger("Attack");
    }

    

    public void Init()
    {
        m_IsGameOver = false;
        m_IsMoving = false;
    }

    private void Update()
    {
        if (m_IsGameOver)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                GameManager.Instance.StartNewGame();
            }
            return;
        }

        if (m_IsMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_MoveTarget, MoveSpeed * Time.deltaTime);
            if (transform.position == m_MoveTarget)
            {
                m_IsMoving = false;
                m_Animator.SetBool("Moving", false);
                m_Animator.ResetTrigger("Attack");

                var cellData = m_Board.GetCellData(m_CellPosition);
                if (cellData.ContainedObject != null)
                    cellData.ContainedObject.PlayerEntered();
            }
            return;
        }

        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            m_FacingDirection = Vector2Int.up;
            hasMoved = true;
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            m_FacingDirection = Vector2Int.down;
            hasMoved = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face right
            m_FacingDirection = Vector2Int.right;
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            transform.rotation = Quaternion.Euler(0, 180, 0); // Face left
            m_FacingDirection = Vector2Int.left;
            hasMoved = true;
        }

        if (hasMoved)
        {
            BoardManager.CellData cellData = m_Board.GetCellData(newCellTarget);

            if (cellData != null && cellData.Passable)
            {
                GameManager.Instance.TurnManager.Tick();

                if (cellData.ContainedObject == null)
                {
                    MoveTo(newCellTarget, false);
                }
                else if (cellData.ContainedObject != null)
                {
                    m_Animator.SetTrigger("Attack");

                    if (cellData.ContainedObject.PlayerWantsToEnter())
                    {
                        MoveTo(newCellTarget, false);
                    }
                    else
                    {
                        StartCoroutine(SmoothAttackMoveAndReturn());
                    }
                }
            }
        }
    }

    private IEnumerator SmoothAttackMoveAndReturn()
    {
        Vector3 originalPosition = transform.position;
        Vector3 attackOffset = new Vector3(m_FacingDirection.x * AttackMoveDistance, m_FacingDirection.y * AttackMoveDistance, 0);
        Vector3 attackPosition = originalPosition + attackOffset;

        float elapsedTime = 0f;

        // Move forward smoothly
        while (elapsedTime < AttackMoveDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, attackPosition, elapsedTime / AttackMoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = attackPosition;

        elapsedTime = 0f;

        // Move back smoothly
        while (elapsedTime < AttackMoveDuration)
        {
            transform.position = Vector3.Lerp(attackPosition, originalPosition, elapsedTime / AttackMoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }
}
