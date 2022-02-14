using UnityEngine;

public class Level : MonoBehaviour
{
    public string levelName;
    public Transform finalPos;

    public Vector3 GetFinalPos()
    {
        return finalPos.position;
    }
    public void DestroyLevel()
    {
        Destroy(gameObject);
    }
}
