using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public BoardManager BoardManager;
    public PlayerController PlayerController;
    private int m_FoodAmount = 100;

    public UIDocument UIDoc;
    private Label m_FoodLabel;
    public TurnManager TurnManager { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_FoodLabel.text = "Food : " + m_FoodAmount;
        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen;

        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));
    }
    void OnTurnHappen()
{
    // Ensure food gained before deducting for the turn
    if (m_FoodAmount > 0)
    {
<<<<<<< Updated upstream
        m_FoodAmount -= 1;
        m_FoodLabel.text = "Food : " + m_FoodAmount;
=======
        Debug.Log("Food available before consumption: " + m_FoodAmount);
>>>>>>> Stashed changes
    }

    // Apply food consumption at the end of turn
    m_FoodAmount = Mathf.Max(0, m_FoodAmount - 1);
    m_FoodLabel.text = "Food : " + m_FoodAmount;

    if (m_FoodAmount <= 0)
    {
        GameOver();
    }
}


    public void IncreaseFood(int amount)
    {
        m_FoodAmount += amount;

        if (m_FoodLabel != null)
        {
            m_FoodLabel.text = "Food : " + m_FoodAmount;
        }
    }
}