using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState
{
    public List<List<bool>> horizontal_lines;
    public List<List<bool>> vertical_lines;
    public List<List<int>> boxes;
    public List<List<int>> box_line_cnt;

    public int number_of_boxes = 0;

    public int freeSpace;

    public BoardState()
    {
        freeSpace = (GameData.Columns - 1) * GameData.Rows + (GameData.Rows - 1) * GameData.Columns;

        horizontal_lines = new List<List<bool>>();
        vertical_lines = new List<List<bool>>();
        boxes = new List<List<int>>();
        box_line_cnt = new List<List<int>>();

        /*Horizontal Lines*/
        for (int i = 0; i < GameData.Rows; i++)
        {
            horizontal_lines.Add(new List<bool>());
            for (int j = 0; j < GameData.Columns - 1; j++)
            {
                horizontal_lines[i].Add(false);
            }
        }

        /*Vertical Lines*/
        for (int i = 0; i < GameData.Rows - 1; i++)
        {
            vertical_lines.Add(new List<bool>());
            for (int j = 0; j < GameData.Columns; j++)
            {
                vertical_lines[i].Add(false);
            }
        }

        for (int i = 0; i < GameData.Rows - 1; i++)
        {
            boxes.Add(new List<int>());
            box_line_cnt.Add(new List<int>());
            for (int j = 0; j < GameData.Columns - 1; j++)
            {
                boxes[i].Add(0);
                box_line_cnt[i].Add(0);
            }
        }
    }

    void set_active_sprite(int i, int j, int player)
    {
        if (player == 1)
        {
            Spawn_Manager.Instance.red_box[i][j].SetActive(true);
            GameData.Player_One_Score++;
            UI_Manager.Instance.update_score();
        }
        else
        {
            Spawn_Manager.Instance.blue_box[i][j].SetActive(true);
            GameData.Player_Two_Score++;
            UI_Manager.Instance.update_score();
        }
    }


    void add_box_up(int i, int j, int player, bool set_sprite)
    {
        box_line_cnt[i - 1][j]++;

        if (box_line_cnt[i - 1][j] == 4)
        {
            boxes[i - 1][j] = player;

            if (set_sprite)
            {
                set_active_sprite(i - 1, j, player);
            }

            number_of_boxes++;
        }
    }

    void add_box_down(int i, int j, int player, bool set_sprite)
    {
        box_line_cnt[i][j]++;

        if (box_line_cnt[i][j] == 4)
        {
            boxes[i][j] = player;

            if (set_sprite)
            {
                set_active_sprite(i, j, player);
            }

            number_of_boxes++;
        }
    }

    void add_box_horizontal_line(int i, int j, int player, bool set_sprite)
    {
        if (i == 0)
        {
            add_box_down(i, j, player, set_sprite);
        } 
        else if (i == (GameData.Rows - 1))
        {
            add_box_up(i, j, player, set_sprite);
        }
        else
        {
            add_box_up(i, j, player, set_sprite);
            add_box_down(i , j, player, set_sprite);
        }
    }

    void add_box_right(int i, int j, int player, bool set_sprite)
    {
        box_line_cnt[i][j]++;

        if (box_line_cnt[i][j] == 4)
        {
            boxes[i][j] = player;

            if (set_sprite)
            {
                set_active_sprite(i, j, player);
            }

            number_of_boxes++;
        }
    }

    void add_box_left(int i, int j, int player, bool set_sprite)
    {
        box_line_cnt[i][j - 1]++;

        if (box_line_cnt[i][j - 1] == 4)
        {
            boxes[i][j - 1] = player;

            if (set_sprite)
            {
                set_active_sprite(i, j - 1, player);
            }

            number_of_boxes++;
        }
    }

    void add_box_vertical_line(int i, int j, int player, bool set_sprite)
    {
        if (j == 0)
        {
            add_box_right(i, j, player, set_sprite);
        }
        else if (j == (GameData.Columns - 1))
        {
            add_box_left(i, j, player, set_sprite);
        }
        else
        {
            add_box_right(i, j, player, set_sprite);
            add_box_left(i, j, player, set_sprite);
        }
    }


    public void add_horizontal_line(int i, int j, int player, bool set_sprite = false)
    {
        horizontal_lines[i][j] = true;
        add_box_horizontal_line(i, j, player, set_sprite);
        freeSpace--;
    }

    public void add_vertical_line(int i, int j, int player, bool set_sprite = false)
    {
        vertical_lines[i][j] = true;
        add_box_vertical_line(i, j, player, set_sprite);
        freeSpace--;
    }


    void remove_box(int i, int j)
    {
        box_line_cnt[i][j]--;

        if (boxes[i][j] != 0)
        {
            boxes[i][j] = 0;
            number_of_boxes--;
        }
    }

    void remove_box_up(int i, int j)
    {
        remove_box(i - 1, j);
    }

    void remove_box_down(int i, int j)
    {
        remove_box(i, j);
    }

    void remove_box_right(int i, int j)
    {
        remove_box(i, j);
    }

    void remove_box_left(int i, int j)
    {
        remove_box(i, j - 1);
    }

    void remove_box_horizontal_line(int i, int j)
    {
        if (i == 0)
        {
            remove_box_down(i, j);
        }
        else if (i == (GameData.Rows - 1))
        {
            remove_box_up(i, j);
        }
        else
        {
            remove_box_up(i, j);
            remove_box_down(i, j);
        }
    }

    void remove_box_vertical_line(int i, int j)
    {
        if (j == 0)
        {
            remove_box_right(i, j);
        }
        else if (j == (GameData.Columns - 1))
        {
            remove_box_left(i, j);
        }
        else
        {
            remove_box_right(i, j);
            remove_box_left(i, j);
        }
    }

    public void remove_horizontal_line(int i, int j)
    {
        horizontal_lines[i][j] = false;
        remove_box_horizontal_line(i, j);
        freeSpace++;
    }

    public void remove_vertical_line(int i, int j)
    {
        vertical_lines[i][j] = false;
        remove_box_vertical_line(i, j);
        freeSpace++;
    }


}
