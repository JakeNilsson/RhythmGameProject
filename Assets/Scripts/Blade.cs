using UnityEngine;
using UnityEngine.InputSystem;

public class Blade : MonoBehaviour
{
    private bool slicing;

    private Collider2D bladeCollider;

    private Camera mainCamera;

    private TrailRenderer bladeTrail;

    public Vector3 direction { get; private set; }

    public float minSliceVelocity = 0.01f;
    public float sliceForce = 5f;

    private void Awake() {
        bladeCollider = GetComponent<Collider2D>();
        bladeTrail = GetComponentInChildren<TrailRenderer>();
        mainCamera = Camera.main;
    }

    private void OnEnable() {
        StopSlicing();
    }

    private void OnDisable() {
        StopSlicing();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Debug.Log("Start Slicing");
            StartSlicing();
        } else if (Mouse.current.leftButton.wasReleasedThisFrame) {
            Debug.Log("Stop Slicing");
            StopSlicing();
        } else if (slicing) {
            Debug.Log("Continue Slicing");
            ContinueSlicing();
        }
    }

    private void StartSlicing() {
        //Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        transform.position = newPosition;
        Vector3 localPosition = transform.localPosition;
        localPosition.z = 0f;
        transform.localPosition = localPosition;

        slicing = true;
        bladeCollider.enabled = true;
        bladeTrail.enabled = true;
        bladeTrail.Clear();
    }

    private void StopSlicing() {
        slicing = false;
        bladeCollider.enabled = false;
        bladeTrail.enabled = false;
    }

    private void ContinueSlicing() {
        //Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        newPosition.z = 0f;

        direction = newPosition - transform.position;

        float velocity = direction.magnitude / Time.deltaTime;
        bladeCollider.enabled = velocity > minSliceVelocity;

        transform.position = newPosition;
        Vector3 localPosition = transform.localPosition;
        localPosition.z = 0f;
        transform.localPosition = localPosition;
    }
}
