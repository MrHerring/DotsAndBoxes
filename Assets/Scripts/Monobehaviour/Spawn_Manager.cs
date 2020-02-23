using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class Spawn_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject _left_up_spawn_point;
    [SerializeField]
    private GameObject _right_up_spawn_point;
    [SerializeField]
    private GameObject _left_down_spawn_point;

    [SerializeField]
    private Canvas _canvas_controlls;

    [SerializeField]
    private GameObject _prefab_dot;
    [SerializeField]
    private GameObject _prefab_horizontal_line;
    [SerializeField]
    private GameObject _prefab_vertical_line;
    [SerializeField]
    private GameObject _prefab_red_box;
    [SerializeField]
    private GameObject _prefab_blue_box;

    public List<List<GameObject>> dots;

    public List<List<GameObject>> horizontal_lines;
    public List<List<GameObject>> vertical_lines;

    public List<List<GameObject>> red_box;
    public List<List<GameObject>> blue_box;


    //Singleton Pattern
    private static Spawn_Manager _instance;
    public static Spawn_Manager Instance
    {
        get
        {
            if (System.Object.ReferenceEquals(_instance, null))
            {
                _instance = GameObject.FindObjectOfType<Spawn_Manager>();
                if (System.Object.ReferenceEquals(_instance, null))
                {
                    _instance = new GameObject("Spawn_Manager").AddComponent<Spawn_Manager>();
                }
            }

            return _instance;
        }
    }


    private void set_scale()
    {
        Vector3 scale_vector_4 = new Vector3(0.6f, 0.6f, 0.6f);
        Vector3 scale_vector_5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 scale_vector_6 = new Vector3(0.4f, 0.4f, 0.4f);
        Vector3 scale_vector_7 = new Vector3(0.3f, 0.3f, 0.3f);

        Vector3 scale_vector_in_use;

        int max = Mathf.Max(GameData.Columns, GameData.Rows);

        if (max >= 7)
        {
            scale_vector_in_use = scale_vector_7;
        }
        else if (max == 6)
        {
            scale_vector_in_use = scale_vector_6;
        }
        else if (max == 5)
        {
            scale_vector_in_use = scale_vector_5;
        }
        else 
        {
            scale_vector_in_use = scale_vector_4;
        }


        for (int i = 0; i < GameData.Columns; i++)
        {
            for (int j = 0; j < GameData.Rows; j++)
            {
                dots[i][j].transform.localScale = scale_vector_in_use;
            }
        }

        for (int i = 0; i < GameData.Rows - 1; i++)
        {
            for (int j = 0; j < GameData.Columns - 1; j++)
            {
                red_box[i][j].transform.localScale = scale_vector_in_use;
                blue_box[i][j].transform.localScale = scale_vector_in_use;
            }
        }

        for (int i = 0; i < GameData.Rows; i++)
        {
            for (int j = 0; j < GameData.Columns - 1; j++)
            {
                horizontal_lines[i][j].transform.localScale = scale_vector_in_use;
            }
        }


        for (int i = 0; i < GameData.Rows - 1; i++)
        {
            for (int j = 0; j < GameData.Columns; j++)
            {
                vertical_lines[i][j].transform.localScale = scale_vector_in_use;
            }
        }

    }

    public void init_spawn_manager()
    {
        float delta_x = _right_up_spawn_point.transform.position.x - _left_up_spawn_point.transform.position.x;
        float delta_y = _left_up_spawn_point.transform.position.y - _left_down_spawn_point.transform.position.y;
        float offset_x;
        float offset_y;

        if (GameData.Columns > 2)
        {
            offset_x = delta_x / (GameData.Columns - 1);
        }
        else
        {
            offset_x = delta_x;
        }

        if (GameData.Rows > 2)
        {
            offset_y = delta_y / (GameData.Rows - 1);
        }
        else
        {
            offset_y = delta_y;
        }

        float base_x = _left_up_spawn_point.transform.position.x;
        float base_y = _right_up_spawn_point.transform.position.y;
        float z = 0;

        dots = new List<List<GameObject>>();
        horizontal_lines = new List<List<GameObject>>();
        vertical_lines = new List<List<GameObject>>();
        red_box = new List<List<GameObject>>();
        blue_box = new List<List<GameObject>>();

        /*Dots Sprite Spawn*/
        for (int i = 0; i < GameData.Columns; i++)
        {
            dots.Add(new List<GameObject>());
            for (int j = 0; j < GameData.Rows; j++)
            {
                GameObject go = Instantiate(_prefab_dot);
                go.transform.SetParent(_canvas_controlls.transform);
                go.transform.position = new Vector3((base_x + i * offset_x), (base_y - j * offset_y), z);

                dots[i].Add(go);

                //go.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            }
        }

        /*Boxes Sprite Spawn*/
        for (int i = 0; i < GameData.Rows - 1; i++)
        {
            red_box.Add(new List<GameObject>());
            blue_box.Add(new List<GameObject>());

            for (int j = 0; j < GameData.Columns - 1; j++)
            {
                GameObject red_box_go = Instantiate(_prefab_red_box);
                GameObject blue_box_go = Instantiate(_prefab_blue_box);

                red_box_go.SetActive(false);
                blue_box_go.SetActive(false);


                red_box_go.transform.SetParent(_canvas_controlls.transform);
                blue_box_go.transform.SetParent(_canvas_controlls.transform);

                red_box_go.transform.position = new Vector3((base_x + (offset_x / 2) + j * offset_x), (base_y - (offset_y / 2) - i * offset_y), 0);
                blue_box_go.transform.position = new Vector3((base_x + (offset_x / 2) + j * offset_x), (base_y - (offset_y / 2) - i * offset_y), 0);

                red_box[i].Add(red_box_go);
                blue_box[i].Add(blue_box_go);
            }
        }

        /*Horizontal line Sprite Spawn*/
        for (int i = 0; i < GameData.Rows; i++)
        {
            horizontal_lines.Add(new List<GameObject>());

            for (int j = 0; j < GameData.Columns - 1; j++)
            {
                GameObject go_horizontal_line = Instantiate(_prefab_horizontal_line);
                go_horizontal_line.transform.SetParent(_canvas_controlls.transform);
                go_horizontal_line.transform.position = new Vector3((base_x + (offset_x / 2) + j * offset_x), (base_y - i * offset_y), 0);
                go_horizontal_line.GetComponent<Button_Controller>().Index_X = i;
                go_horizontal_line.GetComponent<Button_Controller>().Index_Y = j;
                horizontal_lines[i].Add(go_horizontal_line);
            }
        }

        /*Vertical line Sprite Spawn*/
        for (int i = 0; i < GameData.Rows - 1; i++)
        {
            vertical_lines.Add(new List<GameObject>());
            for (int j = 0; j < GameData.Columns; j++)
            {
                GameObject go_vectical_line = Instantiate(_prefab_vertical_line);
                go_vectical_line.transform.SetParent(_canvas_controlls.transform);
                go_vectical_line.transform.position = new Vector3((base_x + j * offset_x), (base_y - (offset_y / 2) - i * offset_y), 0);
                go_vectical_line.GetComponent<Button_Controller>().Index_X = i;
                go_vectical_line.GetComponent<Button_Controller>().Index_Y = j;
                vertical_lines[i].Add(go_vectical_line);
            }
        }

        set_scale();
    }


    private void Awake()
    {
        _instance = this;
    }


   public void destroy_game_objects()
    {
        for (int i = 0; i < GameData.Columns; i++)
        {
            for (int j = 0; j < GameData.Rows; j++)
            {
                Destroy(dots[i][j]);
            }
        }

        for (int i = 0; i < GameData.Rows - 1; i++)
        {
            for (int j = 0; j < GameData.Columns - 1; j++)
            {
                Destroy(red_box[i][j]);
                Destroy(blue_box[i][j]);
            }
        }

        for (int i = 0; i < GameData.Rows; i++)
        {
            for (int j = 0; j < GameData.Columns - 1; j++)
            {
                Destroy(horizontal_lines[i][j]);
            }
        }


        for (int i = 0; i < GameData.Rows - 1; i++)
        {
            for (int j = 0; j < GameData.Columns; j++)
            {
                Destroy(vertical_lines[i][j]);
            }
        }

    }

    private void OnDestroy()
    {
        _instance = null;
    }




}
