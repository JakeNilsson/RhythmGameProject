using UnityEngine;
using System.Collections;
using System.IO;
using SimpleFileBrowser;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class MappingMenu : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private GameObject mappingMenuCanvas;

    [SerializeField] private GameObject editorCanvas;

    public void OpenMappingMenu()
    {
        mappingMenuCanvas.SetActive(true);
        editorCanvas.SetActive(false);
    }

    public void OpenEditor()
    {
        mappingMenuCanvas.SetActive(false);
        editorCanvas.SetActive(true);
    }

	public static void ReturnToMainMenu() 
	{
		SceneManager.LoadScene("Main Menu");
	}

    public void OpenFileBrowser()
	{
		// Set filters (optional)
		// It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
		// if all the dialogs will be using the same filters
		FileBrowser.SetFilters( false, new FileBrowser.Filter( "Music File", ".mp3" ) );

		// Set default filter that is selected when the dialog is shown (optional)
		// Returns true if the default filter is set successfully
		// In this case, set Images filter as the default filter
		FileBrowser.SetDefaultFilter( ".mp3" );

		// Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
		// Note that when you use this function, .lnk and .tmp extensions will no longer be
		// excluded unless you explicitly add them as parameters to the function
		FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );

		// Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
		// It is sufficient to add a quick link just once
		// Name: Users
		// Path: C:\Users
		// Icon: default (folder icon)
		FileBrowser.AddQuickLink( "Users", "C:\\Users", null );
		
		// !!! Uncomment any of the examples below to show the file browser !!!

		// Example 2: Show a select folder dialog using callback approach
		// onSuccess event: print the selected folder's path
		// onCancel event: print "Canceled"
		// Load file/folder: folder, Allow multiple selection: false
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Select Folder", Submit button text: "Select"
		// FileBrowser.ShowLoadDialog( ( paths ) => { Debug.Log( "Selected: " + paths[0] ); },
		// 						   () => { Debug.Log( "Canceled" ); },
		// 						   FileBrowser.PickMode.Files, false, null, null, "Select Midi File", "Select" );

		// Example 3: Show a select file dialog using coroutine approach
		StartCoroutine( ShowLoadDialogCoroutine() );
	}

    IEnumerator ShowLoadDialogCoroutine()
	{
		// Show a load file dialog and wait for a response from user
		// Load file/folder: file, Allow multiple selection: true
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Load File", Submit button text: "Load"
		yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.Files, false, null, null, "Select Midi File", "Select" );

		// Dialog is closed
		// Print whether the user has selected some files or cancelled the operation (FileBrowser.Success)
		Debug.Log( FileBrowser.Success );

		if( FileBrowser.Success )
			OnFileSelected( FileBrowser.Result ); // FileBrowser.Result is null, if FileBrowser.Success is false
	}
	
	void OnFileSelected( string[] filePaths )
	{
		// Print paths of the selected files
		// for( int i = 0; i < filePaths.Length; i++ )
		// 	Debug.Log( filePaths[i] );

		// Get the file path of the first selected file
		string filePath = filePaths[0];

		Debug.Log(filePath);

        string destinationPath = Path.Combine( Application.persistentDataPath, "Levels", Path.GetFileNameWithoutExtension(filePath) );

		if (!Directory.Exists(destinationPath))
		{
			Directory.CreateDirectory(destinationPath);
		}

		destinationPath = Path.Combine( destinationPath, Path.GetFileName(filePath) );

		FileBrowserHelpers.CopyFile( filePath, destinationPath );

        // add the midi file to the audio source
        UnityWebRequest request = GetAudioFromFile(destinationPath);

        audioSource.clip = DownloadHandlerAudioClip.GetContent(request);
		audioSource.clip.name = Path.GetFileNameWithoutExtension(destinationPath);
		audioSource.pitch = 1.0f;
		audioSource.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", 1.0f/audioSource.pitch);

        // switch to editing 
		OpenEditor();
	}

    private UnityWebRequest GetAudioFromFile(string path)
    {
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.MPEG);
        request.SendWebRequest();

        // wait for the request to finish
        while (!request.isDone) { }

        return request;
    }

    public void QuitGame()
    {
        Debug.Log("Quit Successfully.");
        Application.Quit();
    }
}
