using UnityEngine;

public class FoodObject : CellObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PlayerEntered()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.IncreaseFood(5);
        }

        Destroy(gameObject);
        Debug.Log("Food increased");
    }
}
