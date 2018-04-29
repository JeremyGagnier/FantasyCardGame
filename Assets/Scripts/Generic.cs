using UnityEngine;

public class Generic
{
    public static GameObject Instantiate(GameObject original)
    {
        GameObject copy = GameObject.Instantiate(original);
        copy.name = original.name;
        return copy;
    }

    public static Vector2 RotateVector(Vector2 v, float angle)
    {
        float vx = v.x * Mathf.Cos(angle) - v.y * Mathf.Sin(angle);
        float vy = v.x * Mathf.Sin(angle) + v.y * Mathf.Cos(angle);
        return new Vector2(vx, vy);
    }

    public static void DestroyAllChildren(Transform transform)
    {
        foreach (Transform child in transform)
        {
            // You have to set the objects as inactive otherwise they will contribute components to
            // GetComponentsInChildren calls in the same frame.
            child.gameObject.SetActive(false);
            GameObject.Destroy(child.gameObject);
        }
    }
}
