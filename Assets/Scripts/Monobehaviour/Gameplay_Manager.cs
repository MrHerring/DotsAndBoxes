using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay_Manager : MonoBehaviour
{
    [SerializeField]
    private int _dim_horizontal;
    [SerializeField]
    private int _dim_vertical;

    public BoardState current_state;

    [SerializeField]
    private GameObject _minimax_game_logic_object;

    private int _depth;

    public int player;

    [SerializeField]
    private Sprite _horizontal_line_sprite;

    [SerializeField]
    private Sprite _horizontal_line_sprite_blue;

    [SerializeField]
    private Sprite _vertial_line_sprite;

    [SerializeField]
    private Sprite _vertial_line_sprite_blue;

    [SerializeField]
    private Canvas _canvas_game_over;
    [SerializeField]
    private Canvas _canvas_controlls;
    [SerializeField]
    private Canvas _canvas_next;
    [SerializeField]
    private Canvas _canvas_ui;

    private bool game_over;

    [SerializeField]
    private Image _red_line;

    [SerializeField]
    private Image _blue_line;

    //Singleton Pattern
    private static Gameplay_Manager _instance;
    public static Gameplay_Manager Instance
    {
        get
        {
            if (System.Object.ReferenceEquals(_instance, null))
            {
                _instance = GameObject.FindObjectOfType<Gameplay_Manager>();
                if (System.Object.ReferenceEquals(_instance, null))
                {
                    _instance = new GameObject("Gameplay_Manager").AddComponent<Gameplay_Manager>();
                }
            }

            return _instance;
        }
    }

    private void init_game_manager()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this);

        current_state = new BoardState();
        _canvas_controlls.gameObject.SetActive(true);
        _canvas_ui.gameObject.SetActive(true);
        _canvas_game_over.gameObject.SetActive(false);
        _canvas_next.gameObject.SetActive(false);

        game_over = false;
        player = 1;

        _depth = GameData.Depth;

        GameData.Player_One_Score = 0;
        GameData.Player_Two_Score = 0;

        UI_Manager.Instance.update_score();

        _red_line.gameObject.SetActive(true);
        _blue_line.gameObject.SetActive(false);
    }

    private void Awake()
    {
        Spawn_Manager.Instance.init_spawn_manager();
        UI_Manager.Instance.init_ui_manager();

        init_game_manager();
    }


    public void show_canvas_game_over()
    {
        _canvas_controlls.gameObject.SetActive(false);
        _canvas_next.gameObject.SetActive(false);
        _canvas_ui.gameObject.SetActive(false);
        _canvas_game_over.gameObject.SetActive(true);
    }

    public void replay_game()
    {
        Spawn_Manager.Instance.destroy_game_objects();
        init_game_manager();
        Spawn_Manager.Instance.init_spawn_manager();
    }


    /*while not game over*/
    private void Update()
    {
        if (current_state.freeSpace == 0 && !game_over)
        {
            game_over = true;
            _canvas_next.gameObject.SetActive(true);
        }
    }

    public void change_player()
    {
        if (Gameplay_Manager.Instance.player == 1)
        {
            Gameplay_Manager.Instance.player = 2;
            _red_line.gameObject.SetActive(false);
            _blue_line.gameObject.SetActive(true);
        }
        else
        {
            Gameplay_Manager.Instance.player = 1;
            _red_line.gameObject.SetActive(true);
            _blue_line.gameObject.SetActive(false);
        }
    }

    private void set_horizontal_sprite(Button button)
    {
        if (player == 1)
        {
            button.image.sprite = _horizontal_line_sprite;
        }
        else
        {
            button.image.sprite = _horizontal_line_sprite_blue;
        }
    }

    private void set_vertical_sprite(Button button)
    {
        if (player == 1)
        {
            button.image.sprite = _vertial_line_sprite;
        }
        else
        {
            button.image.sprite = _vertial_line_sprite_blue;
        }
    }
    
    public void showLine(int move_x_cord, int move_y_cord, bool isHorizontal, bool isVertical)
    {
        if (isHorizontal)
        {
            Button button = Spawn_Manager.Instance.horizontal_lines[move_x_cord][move_y_cord].transform.gameObject.GetComponent<Button>();
            button.interactable = false;
            
            set_horizontal_sprite(button);
            current_state.add_horizontal_line(move_x_cord, move_y_cord, player, true);
        }

        if (isVertical)
        {
            Button button = Spawn_Manager.Instance.vertical_lines[move_x_cord][move_y_cord].transform.gameObject.GetComponent<Button>();
            button.interactable = false;
            set_vertical_sprite(button);
            current_state.add_vertical_line(move_x_cord, move_y_cord, player, true);
        }
    }

    public void play_next_move()
    {
        change_player();

        int prev_number_of_boxes = current_state.number_of_boxes;

        Move move = _minimax_game_logic_object.GetComponent<Minimax>().play_next_move(current_state, _depth);
        showLine(move.move_x_cord, move.move_y_cord, move.isHorizontal, move.isVertical);

        while ((current_state.number_of_boxes > prev_number_of_boxes) && (Gameplay_Manager._instance.current_state.freeSpace != 0))
        {
            prev_number_of_boxes = current_state.number_of_boxes;
            move = _minimax_game_logic_object.GetComponent<Minimax>().play_next_move(current_state, _depth);
            showLine(move.move_x_cord, move.move_y_cord, move.isHorizontal, move.isVertical);
        }

        change_player();
    }

    private void OnDestroy()
    {
        _instance = null;
    }
}
