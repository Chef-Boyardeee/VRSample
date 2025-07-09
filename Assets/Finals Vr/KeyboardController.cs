using UnityEngine;

public class PlayerKeyboardController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotationSpeed = 100f;
    public bool allowMouseRotation = true;

    private CharacterController controller;
    private Transform cameraTransform;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main != null ? Camera.main.transform : transform;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;

        Vector3 move = (forward.normalized * v + right.normalized * h);
        controller.Move(move * moveSpeed * Time.deltaTime);

        HandleRotation();
    }

    void HandleRotation()
    {
        if (UnityEngine.XR.XRSettings.isDeviceActive)
            return;

        if (allowMouseRotation)
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up * mouseX * rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
