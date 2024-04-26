using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour {
    private Collider2D spawnArea;

    [SerializeField] private GameObject[] notePrefabs;

    [SerializeField] private GameObject[] dividers; // blue, red, yellow, green

    [NonSerialized] public static AudioSource audioSource;

    private List<Tuple<GameObject, float, Vector3>> notes;
    
    private int currentNote = 0;

    //public float minSpawnDelay = 0.25f;
    //public float maxSpawnDelay = 1f;

    //public float minAngle = -15f;
    //public float maxAngle = 15f;

    //private float minForce = 10f;
    //private float maxForce = 15f;

    private void Awake() {
        spawnArea = GetComponent<Collider2D>();

        notes = new List<Tuple<GameObject, float, Vector3>>();
    }

    private void OnEnable() {
        LoadNotes();
        StartCoroutine(Spawn());

        //StartCoroutine(testSpawn());
    }

    private void OnDisable() {
        StopAllCoroutines();
    }

    private void LoadNotes() {
        string path = Path.Combine(Application.persistentDataPath, "Levels", audioSource.clip.name, audioSource.clip.name + ".txt");

        if (!File.Exists(path)) {
            Debug.LogError("Notes file not found at " + path);
            return;
        }

        string notesFile = System.IO.File.ReadAllText(path);

        Debug.Log("Loading notes from " + path);
        Debug.Log(notesFile);

        string[] note_strings = notesFile.Split('\n');

        // remove the last empty string
        note_strings = note_strings[..^1];

        foreach (string note_string in note_strings) {
            string[] note_data = note_string.Split(' ');

            foreach (string data in note_data) {
                Debug.Log(data);
            }
            Debug.Log("");

            char color = char.Parse(note_data[0]);
            int index = Array.IndexOf(new char[] { 'B', 'R', 'Y', 'G' }, color);

            Debug.Log("Color: " + color + " Index: " + index);

            GameObject prefab = notePrefabs[index];
            GameObject divider = dividers[index];

            Debug.Log("Prefab: " + prefab.name + " Divider: " + divider.name);

            if (prefab == null) {
                Debug.LogError("Invalid color: " + note_data[0] + " in " + note_string);
                continue;
            }

            float time = float.Parse(note_data[1]);

            float percentage = float.Parse(note_data[2]);

            Debug.Log("Editor Percentage: " + percentage);

            // calculate target position
            Vector3 target = new Vector3(
                divider.transform.position.x + divider.GetComponent<RectTransform>().rect.width * percentage,
                divider.transform.localPosition.y,
                0f
            );

            notes.Add(new Tuple<GameObject, float, Vector3>(prefab, time, target));
            //notes.Add(new Tuple<GameObject, float, float>(prefab, time, percentage));
        }
    }


    public IEnumerator Spawn() {
        while (enabled) {
            // GameObject prefab = notePrefabs[UnityEngine.Random.Range(0, notePrefabs.Length)];

            if (currentNote >= notes.Count) {
                yield break;
            }

            Transform parent = this.transform.parent.GameObject().transform;

            Tuple<GameObject, float, Vector3> noteData = notes[currentNote];
            //Tuple<GameObject, float, float> noteData = notes[currentNote];
            GameObject prefab = noteData.Item1;
            float time = noteData.Item2;
            Vector3 target = parent.TransformPoint(noteData.Item3);


            //Quaternion rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(minAngle, maxAngle));


            float gravity = Physics2D.gravity.magnitude;

            // startPos should be the coordinates of the spawner
            Vector3 start = transform.position;

            Vector3 direction = target - start;

            float timeToTarget = 2.0f;

            float velocityX = direction.x / timeToTarget - 0.05f;
            float velocityY = (direction.y + 0.5f * gravity * timeToTarget * timeToTarget) / timeToTarget - 0.05f;

            // calculate the time to wait before spawning the note
            float waitTime = notes[currentNote].Item2 - audioSource.time - 2.0f;
            if (waitTime < 0) {
                waitTime = 0;
            }

            yield return new WaitForSeconds(waitTime);

            //GameObject note = Instantiate(prefab, position, Quaternion.Euler(0f, 0f, angle), parent);
            GameObject note = Instantiate(prefab, start, Quaternion.identity, parent);

            //note.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            note.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityX, velocityY);

            

            // yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay));
            // float nextTime = notes[currentNote + 1].Item2 - time - 2.0f;
            // if (nextTime < 0) {
            //     nextTime = 0;
            // }
            // yield return new WaitForSeconds(nextTime);
            currentNote++;
        }
    }

    public IEnumerator testSpawn() {
        int index = 0;
        while (enabled) {
            if (index >= notes.Count) {
                yield break;
            }

            Tuple<GameObject, float, Vector3> noteData = notes[index];
            GameObject prefab = noteData.Item1;
            float time = noteData.Item2;
            //Vector3 target = noteData.Item3;

            Vector3 target = transform.parent.GameObject().transform.TransformPoint(noteData.Item3);

            yield return new WaitForSeconds(time - audioSource.time);

            GameObject test = Instantiate(prefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform.parent.GameObject().transform);
            // remove rigidbody2d, collider2d, and script
            Destroy(test.GetComponent<Rigidbody2D>());
            Destroy(test.GetComponent<Collider2D>());
            Destroy(test.GetComponent<Note>());
            test.name = "Test";

            //test.transform.localPosition = target;
            test.transform.position = target;

            Debug.Log("test: " + test.transform.position);

            index++;
        }
    }

    // public IEnumerator Spawn() {
    //     while (enabled) {
    //         GameObject prefab = notePrefabs[UnityEngine.Random.Range(0, notePrefabs.Length)];

    //         Vector3 position = new()
    //         {
    //             x = UnityEngine.Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
    //             y = UnityEngine.Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
    //             z = 0
    //         };

    //         Quaternion rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(minAngle, maxAngle));

    //         GameObject note = Instantiate(prefab, position, rotation, this.transform.parent.GameObject().transform);

    //         if (note.name.Contains("Blue")) {
    //             minForce = 14;
    //             maxForce = 16;
    //         } else if (note.name.Contains("Red")) {
    //             minForce = 12;
    //             maxForce = 14;
    //         } else if (note.name.Contains("Yellow")) {
    //             minForce = 11;
    //             maxForce = 13;
    //         } else {
    //             minForce = 10;
    //             maxForce = 12;
    //         }

    //         float force = UnityEngine.Random.Range(minForce, maxForce);
    //         note.GetComponent<Rigidbody2D>().AddForce(note.transform.up * force, ForceMode2D.Impulse);

    //         yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay));
    //     }
    // }
}
