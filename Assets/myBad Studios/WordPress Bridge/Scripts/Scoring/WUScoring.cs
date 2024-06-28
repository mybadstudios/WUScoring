//WordPress For Unity Bridge: Scoring © 2024 by Ryunosuke Jansen is licensed under CC BY-ND 4.0.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MBS {
	/// <summary>
	/// Possible age ratings you are able to pick when requesting the Gravatar. Pick One that is most suited to your target audience
	/// </summary>
	public enum WUScoringAgeRating		{G, PG, R, X}
	internal enum WUScoringAction			{SubmitScore, FetchScores, SubmitScoreForUser}

	/// <summary>
	/// Implements the functions to trigger the High Score actions on the server. 
	/// Possible actions include: 
	/// 1. Submit a new score for the currently playing / logged in user
	/// 2. When acting as a server, submit scores on behalf of clients
	/// 3. fetch existing scores for the game 
	/// </summary>
	static public class WUScoring {

        #region internal variables
        static readonly string scoring_filepath = "wub_scoring/unity_functions.php";
		static public readonly string ASSET = "SCORING";
		#endregion

		/// <summary>
		/// Subscribe to this action to trigger custom code once a score has been submitted to the server and the server's response has been recieved back
		/// </summary>
		static public Action<CML> onSubmitted;
		/// <summary>
		/// Subscribe to this actioon to trigger custom code once a request to fetch scores from the server has returned it's results.
		/// NOTE: In the demo GUI this event is used to loop over the server's results and spawn a prefab to display each one's details
		/// </summary>
		static public Action<CML> onFetched;

		/// <summary>
		/// Subscribe to this Action to handle a failed attempt to submit a new score to the server
		/// </summary>
		static public Action<CMLData> onSubmissionFailed;
		/// <summary>
		/// Subscribe to this Action to handle a failed attempt to fetch a list of scores from the server
		/// </summary>
		static public Action<CMLData> onFetchingFailed;
		
		/// <summary>
		/// Submit your current score to the server. Higher scores will replace the score on the server. Lower scores will be ignored
		/// </summary>
		/// <param name="score">The proposed new high score</param>
		/// <param name="game_id">OPTIONAL: Defaults to the current game. HINT: Changing this value will allow you to have multiple leaderboards for a single game. WARNING: Every value you pass here must be unique across all games on your server or else the various games will share the leaderboard</param>
		static public void SubmitScore(int score, int game_id = -1)
		{
			CMLData	data = new CMLData();
			data.Seti ("score", score);
			data.Seti ("gid", game_id);
			WPServer.ContactServer(WUScoringAction.SubmitScore.ToString(), scoring_filepath, ASSET, data, onSubmitted, onSubmissionFailed);
		}

		/// <summary>
		/// FOR USE BY THE SERVER IN MULTIPLAYER GAMES: Submit the score of another player to the server. Higher scores will replace their score on the server. Lower scores will be ignored
		/// </summary>
		/// <param name="user">The ID of the account this score is related to</param>
		/// <param name="score">The proposed new high score</param>
		/// <param name="game_id">OPTIONAL: Defaults to the current game. HINT: Changing this value will allow you to have multiple leaderboards for a single game. WARNING: Every value you pass here must be unique across all games on your server or else the various games will share the leaderboard</param>
		/// <param name="onSubmitted">OPTIONAL: Trigger custom code once the submission completed successfully. Replaces the call to the static onSubmitted Action</param>
		/// <param name="onSubmissionFailed">OPTIONAL: Trigger custom code in the event that the submission failed. Replaces the call to the static onSubmissionFailed Action</param>
		static public void SubmitScoreForUser(int user, int score, int game_id = -1, Action<CML> onSubmitted = null, Action<CMLData> onSubmissionFailed = null)
		{
			CMLData	data = new CMLData();
			data.Seti ("score", score);
			data.Seti ("gid", game_id);
			data.Seti ("uid", user);
			WPServer.ContactServer(WUScoringAction.SubmitScoreForUser.ToString(), scoring_filepath, ASSET, data, onSubmitted, onSubmissionFailed);
		}

		/// <summary>
		/// FOR USE BY THE SERVER IN MULTIPLAYER GAMES: Submit the score of another player to the server. Higher scores will replace their score on the server. Lower scores will be ignored
		/// </summary>
		/// <param name="user">The username of the account this score is related to</param>
		/// <param name="score">The proposed new high score</param>
		/// <param name="game_id">OPTIONAL: Defaults to the current game. HINT: Changing this value will allow you to have multiple leaderboards for a single game. WARNING: Every value you pass here must be unique across all games on your server or else the various games will share the leaderboard</param>
		/// <param name="onSubmitted">OPTIONAL: Trigger custom code once the submission completed successfully. Replaces the call to the static onSubmitted Action</param>
		/// <param name="onSubmissionFailed">OPTIONAL: Trigger custom code in the event that the submission failed. Replaces the call to the static onSubmissionFailed Action</param>
		static public void SubmitScoreForUsername(string user, int score, int game_id = -1, Action<CML> onSubmitted = null, Action<CMLData> onSubmissionFailed = null)
		{
			CMLData	data = new CMLData();
			data.Seti ("score", score);
			data.Seti ("gid", game_id);
			data.Set  ("username", user);
			WPServer.ContactServer(WUScoringAction.SubmitScoreForUser.ToString(), scoring_filepath, ASSET, data, onSubmitted, onSubmissionFailed);
		}

		/// <summary>
		/// Fetch a list of high score holders from the server. Trigger onFetched when the results are recieved or onFetchingFailed if the server could not be reached
		/// </summary>
		/// <param name="limit">OPTIONAL: How many entries to fetch from the server. If 0 it fetches the number of entries specified on the website</param>
		/// <param name="game_id">OPTIONAL: Defaults to the current game. Change only if your project has multiple leaderboards. WARNING: specifying an id belonging to another project will use that project's leaderboard</param>
		static public void FetchScores(int limit = 0, int game_id = -1)
		{
			CMLData	data = new CMLData();
			data.Seti("limit", limit);
			data.Seti("gid", game_id);
			WPServer.ContactServer(WUScoringAction.FetchScores.ToString(), scoring_filepath, ASSET, data, onFetched, onFetchingFailed);
		}
	}
	
}
