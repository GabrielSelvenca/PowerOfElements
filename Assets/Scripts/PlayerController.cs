using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocity = 10.0f;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3 (x, 0, y) * velocity;

        transform.Translate(dir * Time.deltaTime);
    }
}
