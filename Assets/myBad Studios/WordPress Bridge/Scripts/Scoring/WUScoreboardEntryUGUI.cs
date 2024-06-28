//WordPress For Unity Bridge: Scoring © 2024 by Ryunosuke Jansen is licensed under CC BY-ND 4.0.

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;

namespace MBS {
    public class WUScoreboardEntryUGUI : MonoBehaviour {
		
		static public WUScoreboardEntryUGUI SpawnInstance(Transform parent, CMLData person, string name, WUScoringAgeRating age_rating = WUScoringAgeRating.PG, int avatar_size = 32)
		{
			WUScoreboardEntryUGUI result = Instantiate(Resources.Load<WUScoreboardEntryUGUI>("WUScoreEntryPrefab"));
			if (null != result)
			{
				result.transform.SetParent(parent, false);
				result.age_rating = age_rating;
				result.avatar_size = avatar_size;
				
				result.gravatar = person.String("gravatar");
				result.name_text.text = person.String(name);
				result.score_text.text = person.String("score");
				
				result.score_text.color = result.name_text.color = person.Bool("highlight") ? result.highlighted_color : result.normal_color;
			}
			return result;
		}
		
		public Color
			normal_color = Color.white,
			highlighted_color = Color.yellow;
		
		public Text 
			score_text,
			name_text;
		
		public Image
			icon;
		
		public WULGravatarTypes avatartype;
		
		WUScoringAgeRating age_rating = WUScoringAgeRating.PG;
		int avatar_size = 32;
		string gravatar;

		async void Start()
		{
			icon.color = new Color(1f, 1f, 1f, 0f);
			string URL = string.Format("http://www.gravatar.com/avatar/{0}?r={1}&s={2}&d={3}", gravatar, age_rating, avatar_size, avatartype.ToString().ToLower());
			var avatar = await URL.DownloadTextureTask();
			Sprite avatar_s = Sprite.Create(avatar, new Rect(0, 0, avatar.width, avatar.height), new Vector2(0f, 0.5f));
			icon.rectTransform.sizeDelta = new Vector2(avatar_size, avatar_size);
			icon.rectTransform.anchoredPosition = new Vector2(0, avatar_size / 2f);
			icon.sprite = avatar_s;
			icon.color = Color.white;
		}
	}
}