/*
 * @author Jonathan Weinberger
 * Date: July 2, 2013
 * Time: 9:25pm EST
 * 
 * Main_Menu GUI Script
 * 
 */

using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {

	void OnGUI() 
	{
#region	Boxes	
		
		//
		// This creates the box that our buttons are placed into
		//
		
		GUI.Box(new Rect(400, 50, Screen.width / 3, Screen.height / 2), "ACRO STORM");	
		
#endregion	
		
#region Buttons		
		
		//
		// Creating the button: Story Mode
		//
		
		if (GUI.Button(new Rect(560, 90, 105, 20), "Story Mode"))
		{
			Application.LoadLevel("Story_Mode");
			print ("Story_Mode");
		}
		
		//
		// Creating the button: Arcade Mode
		//
		
		if (GUI.Button(new Rect(560, 120, 105, 20), "Arcade Mode"))
		{
			Application.LoadLevel("Arcade_Mode");	
			print ("Arcade_Mode");
		}
		
		//
		// Creating button: Versus Mode
		//
		
		if (GUI.Button(new Rect(560, 150, 105, 20), "Versus Mode"))
		{
			Application.LoadLevel("Versus_Mode");
			print ("Versus_Mode");	
		}
		
		//
		// Creating button: Time Attack
		//
		
		if (GUI.Button(new Rect(560, 180, 105, 20), "Time Attack"))
		{
			Application.LoadLevel("Time_Attack");
			print ("Time_Attack");	
		}
		
		//
		// Creating button: Challenge Mode
		//
		
		if (GUI.Button(new Rect(560, 210, 105, 20), "Challenge Mode"))
		{
			Application.LoadLevel("Challenge_Mode");
			print ("Challenge_Mode");	
		}
		
		//
		// Creating button: Customize
		//
		
		if (GUI.Button(new Rect(560, 240, 105, 20), "Customize"))
		{
			Application.LoadLevel("Customize");
			print ("Customize");	
		}
		
		//
		// Creating button: Options
		//
		
		if (GUI.Button(new Rect(560, 270, 105, 20), "Options"))
		{
			Application.LoadLevel("Option");
			print ("Option");	
		}
		
#endregion		
	}
}
