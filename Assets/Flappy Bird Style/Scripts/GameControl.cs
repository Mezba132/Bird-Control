using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour 
{
	public static GameControl instance;			//A reference to our game control script so we can access it statically.
	public Text scoreText;						//A reference to the UI text component that displays the player's score.
	public GameObject gameOverUi;				//A reference to the object that displays the text which appears when the player dies.
	public GameObject pauseBtn;

	private int score = 0;						//The player's score.
	private bool paused=false;
	public bool gameOver = false;				//Is the game over?
	public float scrollSpeed = -1.5f;


	void Awake()
	{
		//If we don't currently have a game control...
		if (instance == null)
			//...set this one to be it...
			instance = this;
		//...otherwise...
		else if(instance != this)
			//...destroy this one because it is a duplicate.
			Destroy (gameObject);
	}

	void Update()
	{
//		//If the game is over and the player has pressed some input...
//		if (gameOver && Input.GetMouseButtonDown(0)) 
//		{
//			//...reload the current scene.
//			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//		}
	}

	public void BirdScored()
	{
		//The bird can't score if the game is over.
		if (gameOver)	
			return;
		//If the game is not over, increase the score...
		score++;
		//...and adjust the score text.
		scoreText.text = "Score: " + score.ToString();
	}

	public void BirdDied()
	{
		//Activate the game over text.
		gameOverUi.SetActive (true);
		pauseBtn.SetActive (false);
		//Set the game to be over.
		gameOver = true;
	}

	public void back()
	{
		//If the game is over and the player has pressed some input...
		if (gameOver || paused) 
		{ 
			//...reload the current scene.
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			Time.timeScale = 1;
		}
	}

	public void pause()
	{
		paused = !paused;

		if (paused) 
		{
			Time.timeScale = 0;
		}
		else if (!paused) 
		{
			Time.timeScale = 1;
		}
	}

	public void Quit()
	{
		Application.Quit ();
	}
}
