using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static int _rows;
    private static int _columns;

    private static int _player_one_score;
    private static int _player_two_score;

    /*
     * 0 : basic
     * 1 : advanced
     * 2 : expert
     */
    private static int _level_of_ai;

    /*
     * 0 : player vs ai
     * 1 : player vs player
     */

    private static int _mode_of_game;

    /*
     * _depth = 2 : basic and advanced
     * _depth = 4 : expert
     */
    private static int _depth;

    public static int Rows
    {
        get => _rows;
        set => _rows = value;
    }

    public static int Columns
    {
        get => _columns;
        set => _columns = value;
    }

    public static int Player_One_Score
    {
        get => _player_one_score;
        set => _player_one_score = value;
    }

    public static int Player_Two_Score
    {
        get => _player_two_score;
        set => _player_two_score = value;
    }

    public static int Level_Of_Ai
    {
        get => _level_of_ai;
        set => _level_of_ai = value;
    }

    public static int Mode_Of_Game
    {
        get => _mode_of_game;
        set => _mode_of_game = value;
    }

    public static int Depth
    {
        get => _depth;
        set => _depth = value;
    }
}
