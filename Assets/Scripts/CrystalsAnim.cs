using UnityEngine;

public class CrystalsAnim : MonoBehaviour
{
    private Vector3 startPos;
    private float timer;
    private float currentSpeed;
    private float amplitude = 0.2f;

    void Start()
    {
        startPos = transform.position;
        SetNewSpeed();
    }

    void Update()
    {
        timer += Time.deltaTime * currentSpeed;
        float offsetY = Mathf.Sin(timer) * amplitude;
        transform.position = startPos + Vector3.up * offsetY;
        transform.rotation = Quaternion.Euler(0f, timer * 90f, 0f);

        if (timer >= 2 * Mathf.PI)
        {
            timer = 0f;
            SetNewSpeed();
        }
    }

    void SetNewSpeed()
    {
        currentSpeed = Random.Range(1.5f, 2f);
    }
}
