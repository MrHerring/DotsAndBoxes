using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private Text _player_one_result;
    [SerializeField]
    private Text _player_two_result;

    [SerializeField]
    private Image _player_one_img;
    [SerializeField]
    private Image _player_two_img;

    [SerializeField]
    private Sprite _human_sprite;
    [SerializeField]
    private Sprite _ai_sprite;

    [SerializeField]
    private Text _player_one_name;
    [SerializeField]
    private Text _player_two_name;

    //Singleton Pattern
    private static UI_Manager _instance;
    public static UI_Manager Instance
    {
        get
        {
            if (System.Object.ReferenceEquals(_instance, null))
            {
                _instance = GameObject.FindObjectOfType<UI_Manager>();
                if (System.Object.ReferenceEquals(_instance, null))
                {
                    GameObject container = new GameObject("UI_Manager");
                    _instance = container.AddComponent<UI_Manager>();
                }
            }

            return _instance;
        }
    }

    public void init_ui_manager()
    {
        _player_one_result.text = "0";
        _player_two_result.text = "0";

        if (GameData.Mode_Of_Game == 0 || GameData.Mode_Of_Game == 3)
        {
            _player_one_img.sprite = _human_sprite;
            _player_two_img.sprite = _ai_sprite;
            _player_one_name.text = "Human";
            _player_two_name.text = "AI";
        }
        else if (GameData.Mode_Of_Game == 1)
        {
            _player_one_img.sprite = _human_sprite;
            _player_two_img.sprite = _human_sprite;
            _player_one_name.text = "Human";
            _player_two_name.text = "Human";
        }
    }

    void Awake()
    {
        _instance = this;
    }

    public void update_score()
    {
        _player_one_result.text = GameData.Player_One_Score.ToString();
        _player_two_result.text = GameData.Player_Two_Score.ToString();
    }

    private void OnDestroy()
    {
        _instance = null;
    }

}
