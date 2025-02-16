using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform localTrans;

    public Camera _camera;

    private void Start()
    {
        localTrans = GetComponent<Transform>();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_camera)
        {
            localTrans.LookAt(2 * localTrans.position - _camera.transform.position);
        }
    }
}
