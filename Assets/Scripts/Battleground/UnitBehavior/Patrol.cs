using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f; // Adjust the speed of movement

    private bool _movingForward = true;
    private float _timer = 0.0f;
    private float _switchDirectionTime = 5.0f; // Time to switch direction

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _switchDirectionTime)
        {
            _movingForward = !_movingForward;
            _timer = 0.0f;
        }

        if (_movingForward)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
    }
}
