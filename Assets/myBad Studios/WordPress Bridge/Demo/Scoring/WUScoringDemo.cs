using UnityEngine;
using UnityEngine.UI;

namespace MBS {
	/// <summary>
	/// A very simple demonstration of how to create / view multiple leaderboards inone game
	/// </summary>
	public class WUScoringDemo : MonoBehaviour {

		/// <summary>
		/// When you hit play, this will either warn you if you don't have the login prefab in your scene or it will destroy itself
		/// </summary>
		public Text
			Message;

		/// <summary>
		/// This holds a reference to the two buttons used in the scene. 
		/// This is used only to disable them while waiting for the server's response to prevent spam clicking of the button(s)
		/// </summary>
		public GameObject[]
			demo_buttons;

		/// <summary>
		/// The parent canvas the high score GUI prefab will be spawned to
		/// </summary>
		public Canvas
			canvas;

		WUScoringUGUI 
			scores;

		/// <summary>
		/// Here you can manually specify a GameID for the sake of this demo. This value will be used by the Submit and Fetch buttons
		/// Set to blank or a value less than 1 to use the game's ACTUAL ID
		/// </summary>
		public InputField id_input;
		int GameIdi { get { if (int.TryParse(id_input.text, out int result) && result > 0) return result; return WPServer.GameID; } }

		/// <summary>
		/// If there is no login component in the scene, quit.
		/// If there is then delete the on screen message, show the demo buttons and
		/// configure WUScoring to automatically fetch the high scores after a
		/// high score has been submitted. (Optional, of course)
		/// </summary>
		void Start () {
			if (null != FindObjectOfType<WUUGLoginGUI>())
			{
				Destroy (Message.gameObject);
				WULogin.OnLoggedIn += ShowButtons;
				WUScoring.onSubmitted += AutoFetchScores;
			} 
			else
				Message.text = "This demo scene requires the WULoginUGUI prefab to be in the scene.\nPlease add it to the scene";
		}

		/// <summary>
		/// The feature to auto fetch high scores is attached to a static function.
		/// It's always a good idea to clean up event responders when you don't need them
		/// any more but it's an even better idea to make SURE you do so with static events!
		/// </summary>
		void OnDestroy() =>	WUScoring.onSubmitted -= AutoFetchScores;
		
		/// <summary>
		/// This gets triggered by a successful score submission
		/// This fetches the high scores
		/// </summary>
		void AutoFetchScores(CML data) => FetchScores();

		/// <summary>
		/// This gets triggered after a successful login.
		/// The demo starts with the buttons hidden. This just shows them
		/// </summary>
		void ShowButtons(CML _) => ShowDemoButtons();
		
		/// <summary>
		/// Shows or hides the demo buttons.
		/// </summary>
		void ShowHideDemoButtons(bool show)
		{
			for ( int i = 0; i < demo_buttons.Length; i++)
				demo_buttons[i].SetActive(show);
		}
		void ShowDemoButtons() => ShowHideDemoButtons(true);
		void HideDemoButtons() => ShowHideDemoButtons(false);

		/// <summary>
		/// Submits a random score.
		/// Successful submission will trigger FetchScores
		/// </summary>
		public void SubmitRandomScore()
		{
			HideDemoButtons();
			int my_score = Random.Range(1, 10000);
			Debug.Log("Submitting score: "+ my_score);
			WUScoring.SubmitScore(my_score, GameIdi);
		}

		/// <summary>
		/// This hides the demo buttons to prevent multiple clicking.
		/// Spawn the high score window and request the high scores from the server.
		/// also set up the button on the high scores window to show the demo buttons again
		/// </summary>
		public void FetchScores()
		{
			HideDemoButtons();
			scores = WUScoringUGUI.SpawnInstance(canvas);
			scores.onWindowClosed += ShowDemoButtons;
			WUScoring.FetchScores(scores.number_of_scores_to_show, GameIdi);
		}
	
	}
}
