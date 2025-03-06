using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
<<<<<<< Updated upstream
    public float MoveSpeed = 5.0f;
=======
    public float MoveSpeed = 3.0f;
>>>>>>> Stashed changes

    private bool m_IsMoving;
    private Vector3 m_MoveTarget;
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;
    private bool m_IsGameOver;

    private Animator m_Animator;
<<<<<<< Updated upstream


=======
    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        MoveTo(cell, false);
    }

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }
>>>>>>> Stashed changes
    public void GameOver()
    {
        m_IsGameOver = true;
    }
<<<<<<< Updated upstream

       private void Awake()
   {
       m_Animator = GetComponent<Animator>();
   }

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        MoveTo(cell, false);
    }
=======
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
   }
>>>>>>> Stashed changes

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
   }

    public void Init()
    {
        m_IsMoving = false;
        m_IsGameOver = false;
        m_IsMoving = false;
    }
    

    private void Update()
    {
        if (m_IsGameOver) return;
        if (m_IsMoving)
       {
           transform.position = Vector3.MoveTowards(transform.position, m_MoveTarget, MoveSpeed * Time.deltaTime);
          
           if (transform.position == m_MoveTarget)
           {
               m_IsMoving = false;
               m_Animator.SetBool("Moving", false);
               var cellData = m_Board.GetCellData(m_CellPosition);
               if(cellData.ContainedObject != null)
                   cellData.ContainedObject.PlayerEntered();
           }

           return;
       }

<<<<<<< Updated upstream
=======
            return;
        }

        if (m_IsMoving)
       {
           transform.position = Vector3.MoveTowards(transform.position, m_MoveTarget, MoveSpeed * Time.deltaTime);
          
           if (transform.position == m_MoveTarget)
           {
               m_IsMoving = false;
               m_Animator.SetBool("Moving", false);
               var cellData = m_Board.GetCellData(m_CellPosition);
               if(cellData.ContainedObject != null)
                   cellData.ContainedObject.PlayerEntered();
           }

           return;
       }
>>>>>>> Stashed changes
        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            hasMoved = true;
        }

        if (hasMoved)
        {
            // Check if the new position is passable, then move there if it is.
            BoardManager.CellData cellData = m_Board.GetCellData(newCellTarget);

            if (cellData != null && cellData.Passable)
            {
                GameManager.Instance.TurnManager.Tick();
                MoveTo(newCellTarget, true);

                if (cellData.ContainedObject != null)
                {
<<<<<<< Updated upstream
                    cellData.ContainedObject.PlayerEntered();
=======
                    MoveTo(newCellTarget, false);
                }
                else if (cellData.ContainedObject.PlayerWantsToEnter())
                {
                    MoveTo(newCellTarget, false);
>>>>>>> Stashed changes
                }
            }
        }
    }
}
