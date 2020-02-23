using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Controller_MainMenu : MonoBehaviour
{

    [SerializeField]
    private Text _text_player_two;

    private void Awake()
    {
        SetPlayerVsPlayer();
    }

    private void InitBoardPlayerVsPlayer()
    {
        GameData.Rows = 7;
        GameData.Columns = 7;
    }

    private void InitBoardPlayerVsAi()
    {
        GameData.Rows = 4;
        GameData.Columns = 4;
    }

    public void SetBasicLevel()
    {
        GameData.Level_Of_Ai = 0;
        GameData.Depth = 2;
        _text_player_two.text = "BASIC AI";
        _text_player_two.transform.localPosition = new Vector3(52.8f, -6.7f, 0);
    }

    public void SetAdvancedLevel()
    {
        GameData.Level_Of_Ai = 1;
        GameData.Depth = 2;
        _text_player_two.text = "ADVANCED AI";
        _text_player_two.transform.localPosition = new Vector3(35.5f, -6.7f, 0);
    }

    public void SetExpertLevel()
    {
        GameData.Level_Of_Ai = 2;
        GameData.Depth = 4;
        _text_player_two.text = "EXPERT AI";
        _text_player_two.transform.localPosition = new Vector3(43.4f, -6.7f, 0);
    }

    public void SetPlayerVsAI()
    {
        InitBoardPlayerVsAi();
        GameData.Mode_Of_Game = 0;
        _text_player_two.text = "BASIC AI";
    }

    public void SetPlayerVsPlayer()
    {
        InitBoardPlayerVsPlayer();
        GameData.Mode_Of_Game = 1;
        _text_player_two.text = "HUMAN";
        _text_player_two.transform.localPosition = new Vector3(52.8f, -6.7f, 0);
    }
}
