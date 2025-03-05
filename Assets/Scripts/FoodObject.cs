using UnityEngine;

public class FoodObject : CellObject
{
    public override void PlayerEntered()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.IncreaseFood(6);
        }

        Destroy(gameObject);
        Debug.Log("Food increased");
    }
}
