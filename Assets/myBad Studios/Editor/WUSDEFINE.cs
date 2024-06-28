using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

/* This script only creates a DEFINE to let other kits know this kit exists
 * It is not included in your final product so you need not concern yourself with it
 * unless you are a plugin developer who needs to test for the existence of this kit
 */
[InitializeOnLoad]
public class WUSDEFINE
{
	static WUSDEFINE()
	{
		BuildTargetGroup btg = EditorUserBuildSettings.selectedBuildTargetGroup;
		string defines_field = PlayerSettings.GetScriptingDefineSymbolsForGroup(btg);
		List<string> defines = new List<string>(defines_field.Split(';'));
		if (!defines.Contains("WUS"))
		{
			defines.Add("WUS");
			PlayerSettings.SetScriptingDefineSymbolsForGroup(btg, string.Join(";", defines.ToArray()));
		}
	}
}


