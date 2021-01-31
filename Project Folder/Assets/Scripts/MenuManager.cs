using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MenuManager : MonoBehaviour // Entirely Writen by Phen Valentia (Nicholas Salter)
{



    // Editer looks for this Function ( See Play Button OnCick() )
    public void StartGame()
    {
        Application.LoadLevel(1);
    }

    // Quits the game
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
        
    }
}
