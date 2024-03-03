using UnityEngine;

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
        if (Input.GetMouseButtonDown(0)) {
            StartSlicing();
        } else if (Input.GetMouseButtonUp(0)) {
            StopSlicing();
        } else if (slicing) {
            ContinueSlicing();
        }
    }

    private void StartSlicing() {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;

        transform.position = newPosition;

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
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;

        direction = newPosition - transform.position;

        float velocity = direction.magnitude / Time.deltaTime;
        bladeCollider.enabled = velocity > minSliceVelocity;

        transform.position = newPosition;
    }
}
