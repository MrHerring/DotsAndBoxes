using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Button_Controller : MonoBehaviour
{
    [SerializeField]
    private int index_x;
    [SerializeField]
    private int index_y;

    public int Index_X
    {
        get 
        { 
            return index_x; 
        }

        set
        {
            index_x = value;
        }
    }

    public int Index_Y
    {
        get
        {
            return index_y;
        }

        set
        {
            index_y = value;
        }
    }


    public void horizontal_button_pressed()
    {
        int prev_number_of_boxes = Gameplay_Manager.Instance.current_state.number_of_boxes;

        Gameplay_Manager.Instance.showLine(this.index_x, this.index_y, true, false);

        if (prev_number_of_boxes == Gameplay_Manager.Instance.current_state.number_of_boxes)
        {
            if (GameData.Mode_Of_Game == 0)
            {
                Gameplay_Manager.Instance.play_next_move();
            }
            else
            {
                Gameplay_Manager.Instance.change_player();
            }     
        }
    }

    public void vertical_button_pressed()
    {
        int prev_number_of_boxes = Gameplay_Manager.Instance.current_state.number_of_boxes;

        Gameplay_Manager.Instance.showLine(this.index_x, this.index_y, false, true);

        if (prev_number_of_boxes == Gameplay_Manager.Instance.current_state.number_of_boxes)
        {
            if (GameData.Mode_Of_Game == 0)
            {
                Gameplay_Manager.Instance.play_next_move();
            }
            else
            {
                Gameplay_Manager.Instance.change_player();
            }
        }

    }
}
