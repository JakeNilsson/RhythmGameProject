using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

public class Editor : MonoBehaviour
{
    private const float displayLeniency = 0.25f;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private Dropdown dropdown;

    [SerializeField] private Slider slider;

    [SerializeField] private Text timeText;

    [SerializeField] private GameObject[] notePrefabs; // blue, red, yellow, green

    private Tuple<GameObject, float>[] notes;

    [SerializeField] private GameObject[] dividers; // blue, red, yellow, green

    [SerializeField] private GameObject editorCanvas;

    [SerializeField] private GameObject OptionsMenu;

    private GraphicRaycaster raycaster;

    PointerEventData clickData;
    List<RaycastResult> clickResults;

    void Start()
    {
        raycaster = editorCanvas.GetComponent<GraphicRaycaster>();
        clickData = new PointerEventData(EventSystem.current);
        clickResults = new List<RaycastResult>();

        notes = new Tuple<GameObject, float>[0];
    }

    void Update()
    {
        if (OptionsMenu.activeSelf) return;

        if (audioSource == null || audioSource.clip == null || timeText == null) return;

        if (audioSource.isPlaying)
        {
            slider.value = audioSource.time / audioSource.clip.length;

            UpdateTimeText();
        }

        // Input System

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Toggle();
        }

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            Rewind(5);
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            Forward(5);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Mouse Clicked");
            
            clickData.position = Mouse.current.position.ReadValue();
            clickResults.Clear();

            raycaster.Raycast(clickData, clickResults);

            foreach (RaycastResult result in clickResults)
            {
                string tag = result.gameObject.tag.ToLower();

                if (tag.Contains("divider")) {
                    Debug.Log("Hit: " + tag.Split(' ')[0] + " at " + audioSource.time);
                    Vector3 position = Mouse.current.position.ReadValue();
                    position.y = result.gameObject.transform.position.y; // center of the divider
                    position.z = 0;
                    Debug.Log("Position: " + position);
                    AddNote(position, tag.Split(' ')[0], audioSource.time);
                }

            }
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Debug.Log("Right Mouse Clicked");

            clickData.position = Mouse.current.position.ReadValue();
            clickResults.Clear();

            raycaster.Raycast(clickData, clickResults);

            foreach (RaycastResult result in clickResults)
            {
                string tag = result.gameObject.tag.ToLower();

                if (tag.Contains("note")) {
                    Debug.Log("Deleting note");

                    notes = notes.Where(note => note.Item1 != result.gameObject).ToArray();

                    Destroy(result.gameObject);
                }

            }
        }

        // end of Input System

        // only display notes that are at the current time +- some amount of time

        foreach (Tuple<GameObject, float> noteData in notes)
        {
            if (audioSource.time - displayLeniency <= noteData.Item2 && noteData.Item2 <= audioSource.time + displayLeniency)
            {
                noteData.Item1.SetActive(true);
            } else
            {
                noteData.Item1.SetActive(false);
            }
        }

    }

    public void Export()
    {
        Debug.Log("Exporting notes");

        string export = "";

        foreach (Tuple<GameObject, float> noteData in notes)
        {
            string color = noteData.Item1.tag.Split(' ')[0][0].ToString().ToUpper();
            float time = noteData.Item2;
            Vector3 position = noteData.Item1.transform.position;

            float percentage = 0;

            int index = Array.IndexOf(dividers, dividers.Where(divider => divider.tag.Contains(color)).First());

            GameObject divider = dividers[index];

            if (divider == null) {
                Debug.LogError("Invalid color");
                return;
            }


            percentage = (position.x - divider.transform.position.x) / divider.GetComponent<RectTransform>().rect.width;

            Debug.Log("editor percentage: " + percentage);
            
            //position = Camera.main.WorldToScreenPoint(position);
            // ignore z because it is always 0

            //export += color + " " + time + " " + position.x + " " + position.y + "\n";
            export += color + " " + time + " " + percentage + "\n";
        }

        Debug.Log(export);

        // save to file in persistent data path
        // string path = Application.persistentDataPath + audioSource.clip.name + ".txt";

        String[] paths = { Application.persistentDataPath, "Levels", audioSource.clip.name, audioSource.clip.name + ".txt"};

        // string path = Path.Combine(Application.persistentDataPath, "Levels");
        // path = Path.Combine(path, audioSource.clip.name);
        // path = Path.Combine(path, audioSource.clip.name + ".txt");
        string path = Path.Combine(paths);
        Debug.Log(path);
        System.IO.File.WriteAllText(path, export);

        Debug.Log("Exported notes to " + path);

        MappingMenu.ReturnToMainMenu();
    }

    public void AddNote(Vector3 position, String color, float time)
    {
        Debug.Log("Adding note at " + time + " with color " + color);
        GameObject prefab = null;

        switch (color)
        {
            case "blue":
                prefab = notePrefabs[0];
                break;
            case "red":
                prefab = notePrefabs[1];
                break;
            case "yellow":
                prefab = notePrefabs[2];
                break;
            case "green":
                prefab = notePrefabs[3];
                break;
        }

        if (prefab == null) {
            Debug.LogError("Invalid color");
            return;
        } else {
            Debug.Log("Adding " + color + " note at " + time);
        }

        GameObject note = Instantiate(prefab, position, Quaternion.identity, editorCanvas.transform);

        string sentence_case = char.ToUpper(color[0]) + color.Substring(1);

        note.tag = sentence_case + " Note";

        Array.Resize(ref notes, notes.Length + 1);

        notes[notes.Length - 1] = new Tuple<GameObject, float>(note, time);

        Debug.Log("Note added");
        Debug.Log("Notes: " + notes.Length);
        foreach (Tuple<GameObject, float> noteData in notes)
        {
            Debug.Log(noteData.Item1.name + " at " + noteData.Item2);
        }
    }


    public void Play()
    {
        audioSource.Play();
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Rewind(float time)
    {
        if (audioSource.time - time < 0)
        {
            audioSource.time = 0;
        } else
        {
            audioSource.time -= time;
        }

        slider.value = audioSource.time / audioSource.clip.length;
        UpdateTimeText();
    }

    public void Forward(float time)
    {
        if (audioSource.time + time > audioSource.clip.length)
        {
            audioSource.time = audioSource.clip.length;
        } else 
        {
            audioSource.time += time;
        }

        slider.value = audioSource.time / audioSource.clip.length;
        UpdateTimeText();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void Toggle()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }
    }

    public void UpdatePitch()
    {
        if (audioSource == null) return;

        String pitch = dropdown.options[dropdown.value].text.Replace("X", "");
        audioSource.pitch = float.Parse(pitch);
        // get pitch shifter pitch value from audio source output audio mixer
        audioSource.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", 1.0f/audioSource.pitch);
    }

    public void UpdatePosition()
    {
        if (audioSource.clip == null) return;

        if (!audioSource.isPlaying)
        {
            audioSource.time = audioSource.clip.length * slider.value;
            UpdateTimeText();
        }
    }

    public void UpdateTimeText()
    {
        timeText.text = TimeSpan.FromSeconds(audioSource.time).ToString(@"mm\:ss");
    }
}
