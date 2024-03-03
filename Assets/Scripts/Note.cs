using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Note : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;

    private Rigidbody2D noteRigidbody;
    private Collider2D noteCollider;

    private Camera mainCamera;

    private bool beenAboveScreen = false;
    private bool aboveScreen = false;

    public int points = 1;

    private void Awake() {
        noteRigidbody = GetComponent<Rigidbody2D>();
        noteCollider = GetComponent<Collider2D>();
        mainCamera = Camera.main;
    }

    private void Update() {
        Vector3 pos = mainCamera.WorldToScreenPoint(transform.position);

        if (!beenAboveScreen) {
            beenAboveScreen = pos.y > -50;

            if (!beenAboveScreen) return;
        }

        aboveScreen = pos.y > -50;

        if (!aboveScreen) {
            if (!sliced.activeSelf)
                FindObjectOfType<GameManager>().ResetCombo();

            Destroy(this.gameObject);
        }
    }

    private void Slice(Vector3 direction, Vector3 position, float force) {
        FindObjectOfType<GameManager>().IncreaseCombo(points);
        
        whole.SetActive(false);
        sliced.SetActive(true);

        noteCollider.enabled = false;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody2D[] slices = sliced.GetComponentsInChildren<Rigidbody2D>();

        foreach (Rigidbody2D slice in slices) {
            slice.velocity = noteRigidbody.velocity;
            Vector2 dir2 = new() {
                x = direction.x,
                y = direction.y
            };

            Vector2 pos2 = new() {
                x = position.x,
                y = direction.y
            };

            slice.AddForceAtPosition(dir2 * force, pos2, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.direction, blade.transform.position, blade.sliceForce);
        }
    }
}
