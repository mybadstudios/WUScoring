//WordPress For Unity Bridge: Scoring © 2024 by Ryunosuke Jansen is licensed under CC BY-ND 4.0.

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MBS{
	/// <summary>
	/// An out-of-the-box working GUI to demonstrate one way of displaying high scores in your project. 
	/// One option is to use this as is and simply skin it to fit the look of your project.
	/// Another option is to build a custom GUI entirely and only refer to this for hints if you get stuck
	/// </summary>
	public class WUScoringUGUI : MonoBehaviour {
		
		/// <summary>
		/// Spawns the Demo GUI prefab and places it on a canvas object, ready to be viewed
		/// </summary>
		/// <param name="canvas">The canvas object to use as the parent for this prefab</param>
		/// <returns>The prefab instance</returns>
		static public WUScoringUGUI SpawnInstance(Canvas canvas = null)
		{
			if (null == canvas)
				canvas = FindObjectOfType<Canvas>();
			if (null == canvas) 
				return null;
			
			WUScoringUGUI result = Instantiate(Resources.Load<WUScoringUGUI>("WUScoringUGUI"));
			if (null != result)
				result.transform.SetParent(canvas.transform, false);

			return result;
		}

		/// <summary>
		/// Set this to match your game's rating to make sure avatars are appropriate for your audience
		/// </summary>
		public WUScoringAgeRating		age_rating;
		/// <summary>
		/// The size of the avatar returned from Gravatar.com . Valid values are between 32 and 512, inclusive
		/// </summary>
		public int 						avatar_size = 32;
		/// <summary>
		/// How many scores do you want to display in the scroll area. Set to 0 to make use of the value set on your website
		/// </summary>
		public int 						number_of_scores_to_show = 20;
		/// <summary>
		/// If the GUI is to display text in a header, specify the Text component to use as the header. In this prefab it
		/// is located at the top center of the prefab but in your custom prefabs you can place it anywhere you like
		/// </summary>
		public Text						header_text;
		//public GameObject				sliding_panel_parent;
		/// <summary>
		/// In this demo there is a button that hides the prefab. This button triggers this Action when it is clicked if
		/// you assigned any functions to it. Doing so is optional but it allows you to tell your project when you are
		/// done with the high scores prefab and want to trigger some custom functionality. 
		/// For example: Add a function to load a new scene to this action being triggered.
		/// </summary>
		public System.Action 			onWindowClosed;
		
		//OnEnabled is called before Start so, to make this works in the inspector or when working offline, put this code
		//in OnEnabled. If you place this in Start then you will only be registered to the event AFTER it has already fired.
		//This means you will have missed it and be left with an empty screen to stare at...
		void OnEnable () =>	WUScoring.onFetched += OnFetched;				

		/// <summary>
		/// When the prefab is no longer in use, remove the OnFetched function from this event to avoid adding duplicate entries
		/// the next time the prefab is loaded / spawned / activated
		/// </summary>
		void OnDisable () => WUScoring.onFetched -= OnFetched;		

		/// <summary>
		/// Triggers any custom code you elected to call when the prefab is closed using the provided button, 
		/// then clears the Action to prevent later duplication or scripts not collected by the Garbage Collector.
		/// Finally, it destroys the prefab.
		/// </summary>
		public void HideScores()
		{
			onWindowClosed?.Invoke();
			onWindowClosed = null;			
			Destroy(gameObject);
		}
		
		/// <summary>
		/// Called after the server has sent back it's results.
		/// Loop through all the returned results and, for each entry, spawn a prefab.
		/// This demo GUI spawns a WUScoreBoardEntryGUI component which in turn populates itself with details from the entry passed to it
		/// </summary>
		public void OnFetched(CML results)
		{
			List<CMLData> entries = results.AllNodesOfType("person");
			if (null == entries) 
				return;
						
			if (avatar_size < 0)
				avatar_size = 32;
			else if (avatar_size > 512)
				avatar_size = 512;
			GridLayoutGroup glg = GetComponentInChildren<GridLayoutGroup>();
			glg.cellSize = new Vector2(glg.cellSize.x, avatar_size);
			RectTransform recttransform = glg.GetComponent<RectTransform>();
			recttransform.sizeDelta = new Vector2(recttransform.sizeDelta.x, entries.Count * (glg.cellSize.y + glg.spacing.y));
			
			foreach(CMLData entry in entries)
			{
				WUScoreboardEntryUGUI.SpawnInstance(
					parent: recttransform,
					person: entry,
					name: "dname",
					age_rating: age_rating, 
					avatar_size: avatar_size);
			}
		}
		
	}
}