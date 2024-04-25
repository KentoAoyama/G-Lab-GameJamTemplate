using UnityEngine;

public class DestroyFallingObject : MonoBehaviour
{
    // オブジェクトがカメラの外に行ったら削除する
    private void Update()
    {
        // オブジェクトがカメラの外に行ったら削除する
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
