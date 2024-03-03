using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour {
    private Collider2D spawnArea;

    public GameObject[] notePrefabs;

    public float minSpawnDelay = 0.25f;
    public float maxSpawnDelay = 1f;

    public float minAngle = -15f;
    public float maxAngle = 15f;

    public float minForce = 10f;
    public float maxForce = 15f;

    //public float maxLifetime = 5f;

    private void Awake() {
        spawnArea = GetComponent<Collider2D>();
    }

    private void OnEnable() {
        StartCoroutine(Spawn());
    }

    private void OnDisable() {
        StopAllCoroutines();
    }

    private IEnumerator Spawn() {
        yield return new WaitForSeconds(2f);

        while (enabled) {
            GameObject prefab = notePrefabs[Random.Range(0, notePrefabs.Length)];

            Vector3 position = new()
            {
                x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
                z = 0
            };

            Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));

            GameObject note = Instantiate(prefab, position, rotation, this.transform.parent.GameObject().transform);
            //Destroy(note, maxLifetime);

            if (note.name.Contains("Blue")) {
                minForce = 14;
                maxForce = 16;
            } else if (note.name.Contains("Red")) {
                minForce = 12;
                maxForce = 14;
            } else if (note.name.Contains("Yellow")) {
                minForce = 11;
                maxForce = 13;
            } else {
                minForce = 10;
                maxForce = 12;
            }

            float force = Random.Range(minForce, maxForce);
            note.GetComponent<Rigidbody2D>().AddForce(note.transform.up * force, ForceMode2D.Impulse);

            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }
}
