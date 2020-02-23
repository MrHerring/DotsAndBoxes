using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public int move_x_cord;
    public int move_y_cord;
    public bool isHorizontal;
    public bool isVertical;

    public Move()
    {
        move_x_cord = -1;
        move_y_cord = -1;
        isHorizontal = false;
        isVertical = false;
    }
}

public class Minimax : MonoBehaviour
{
    /* Moves for expert player */
    List<Move> vertical_optimal_moves = new List<Move>();
    List<Move> horizontal_optimal_moves = new List<Move>();

    List<Move> optimal_moves = new List<Move>();

    int evaluate(BoardState board, bool isMax)
    {
        int diff = 0;

        for (int i = 0; i < GameData.Rows - 1; i++)
        {
            for (int j = 0; j < GameData.Columns - 1; j++)
            {
                if (board.boxes[i][j] == 1)
                {
                    diff -= 1;
                }
                else if (board.boxes[i][j] == 2)
                {
                    diff += 1;
                }
            }
        }

        return diff;
    }

    int evaluate_advance(BoardState board, bool isMax)
    {
        int diff = 0;

        for (int i = 0; i < GameData.Rows - 1; i++)
        {
            for (int j = 0; j < GameData.Columns - 1; j++)
            {
                if (board.boxes[i][j] == 1)
                {
                    diff -= 100;
                }
                else if (board.boxes[i][j] == 2)
                {
                    diff += 100;
                }
                else if (board.box_line_cnt[i][j] == 3 && isMax)
                {
                    diff--;
                }
                else if (board.box_line_cnt[i][j] == 3 && !isMax)
                {
                    diff++;
                }
            }
        }

        return diff;
    }

    int evaluate_board(BoardState board, bool isMax)
    {
        if (GameData.Level_Of_Ai == 0)
        {
            return evaluate(board, isMax);
        }
        else
        {
            return evaluate_advance(board, isMax);
        }
    }

    void put_a_move(List<Move> bestMoves, int i, int j, bool isHorizontal, bool isVertical)
    {
        Move bestMove = new Move();
        bestMove.move_x_cord = i;
        bestMove.move_y_cord = j;
        bestMove.isHorizontal = isHorizontal;
        bestMove.isVertical = isVertical;
        bestMoves.Add(bestMove);
    }

    void try_to_take_box(BoardState board, List<Move> bestMoves)
    { 
        bool isMax = (Gameplay_Manager.Instance.player == 2);
        
        int score = evaluate_board(board, isMax);

        for (int i = 0; i < GameData.Rows; i++)
        {
            for (int j = 0; j < GameData.Columns - 1; j++)
            {
                if (board.horizontal_lines[i][j] == false)
                {
                    board.add_horizontal_line(i, j, Gameplay_Manager.Instance.player);

                    int newScore = evaluate_board(board, isMax);

                    if ((newScore > score && isMax) || (newScore < score && !isMax))
                    {
                        put_a_move(bestMoves, i, j, true, false);
                    }

                    board.remove_horizontal_line(i, j);
                }
            }
        }

        for (int i = 0; i < GameData.Rows - 1; i++)
        {
            for (int j = 0; j < GameData.Columns; j++)
            {
                if (board.vertical_lines[i][j] == false)
                {
                    board.add_vertical_line(i, j, Gameplay_Manager.Instance.player);

                    int newScore = evaluate_board(board, isMax);

                    if ((newScore > score && isMax) || (newScore < score && !isMax))
                    {
                        put_a_move(bestMoves, i, j, false, true);
                    }

                    board.remove_vertical_line(i, j);
                }
            }
        }
    }

    void take_random_move(BoardState board, List<Move> bestMoves)
    {
        for (int i = 0; i < GameData.Rows; i++)
        {
            for (int j = 0; j < GameData.Columns - 1; j++)
            {
                if (board.horizontal_lines[i][j] == false)
                {
                    put_a_move(bestMoves, i, j, true, false);
                }
            }
        }

        for (int i = 0; i < GameData.Rows - 1; i++)
        {
            for (int j = 0; j < GameData.Columns; j++)
            {
                if (board.vertical_lines[i][j] == false)
                {
                    put_a_move(bestMoves, i, j, false, true);
                }
            }
        }
    }

    int minimax(BoardState board, int depth, bool isMax, int maxDepth, int alpha, int beta)
    {
        int score = evaluate_board(board, isMax);
        int player;

        if (board.freeSpace == 0)
        {
            return score;
        }

        if (depth == maxDepth)
        {
            return score;
        }

        int best;

        if (isMax)
        {
            player = 2;
            best = -5000;
        }
        else
        {
            player = 1;
            best = 5000;
        }

        for (int i = 0; i < GameData.Rows; i++)
        {
            for (int j = 0; j < GameData.Columns - 1; j++)
            {
                if (board.horizontal_lines[i][j] == false)
                {
                    board.add_horizontal_line(i, j, player);

                    int new_score = evaluate(board, isMax);

                    if ((new_score > score && isMax) || (new_score < score && !isMax))
                    {
                        if (isMax)
                        {
                            best = Mathf.Max(best, minimax(board, depth + 1, isMax, maxDepth, alpha, beta));
                            alpha = Mathf.Max(alpha, best);
                        }
                        else
                        {
                            best = Mathf.Min(best, minimax(board, depth + 1, isMax, maxDepth, alpha, beta));
                            beta = Mathf.Min(beta, best);
                        }
                    }
                    else
                    {
                        if (isMax)
                        {
                            best = Mathf.Max(best, minimax(board, depth + 1, !isMax, maxDepth, alpha, beta));
                            alpha = Mathf.Max(alpha, best);
                        }
                        else
                        {
                            best = Mathf.Min(best, minimax(board, depth + 1, !isMax, maxDepth, alpha, beta));
                            beta = Mathf.Min(beta, best);                 
                        }
                    }

                    board.remove_horizontal_line(i, j);

                    if (beta <= alpha)
                        break;
                }
            }
        }

        for (int i = 0; i < GameData.Rows - 1; i++)
        {
            for (int j = 0; j < GameData.Columns; j++)
            {
                if (board.vertical_lines[i][j] == false)
                {
                    board.add_vertical_line(i, j, player);

                    int new_score = evaluate(board, isMax);

                    if ((new_score > score && isMax) || (new_score < score && !isMax))
                    {
                        if (isMax)
                        {
                            best = Mathf.Max(best, minimax(board, depth + 1, isMax, maxDepth, alpha, beta));
                            alpha = Mathf.Max(alpha, best);
                        }
                        else
                        {
                            best = Mathf.Min(best, minimax(board, depth + 1, isMax, maxDepth, alpha, beta));
                            beta = Mathf.Min(beta, best);
                        }
                    }
                    else
                    {
                        if (isMax)
                        {
                            best = Mathf.Max(best, minimax(board, depth + 1, !isMax, maxDepth, alpha, beta));
                            alpha = Mathf.Max(alpha, best);
                        }
                        else
                        {
                            best = Mathf.Min(best, minimax(board, depth + 1, !isMax, maxDepth, alpha, beta));
                            beta = Mathf.Min(beta, best);
                        }
                    }

                    board.remove_vertical_line(i, j);

                    if (beta <= alpha)
                        break;
                }
            }
        }




        return best;
    }

    void take_minimax_move(BoardState board, List<Move> bestMoves,int maxDepth)
    {
        bool isMax = (Gameplay_Manager.Instance.player == 2);

        int best;
        int player;

        if (isMax)
        {
            player = 2;
            best = -5000;
        }
        else
        {
            player = 1;
            best = 5000;
        }

        for (int i = 0; i < GameData.Rows; i++)
        {
            for (int j = 0; j < GameData.Columns - 1; j++)
            {
                if (board.horizontal_lines[i][j] == false)
                {
                    board.add_horizontal_line(i, j, player);

                    int move_val = minimax(board, 0, !isMax, maxDepth, -5000, 5000);

                    if ((move_val > best && isMax) || (move_val < best && !isMax))
                    {
                        best = move_val;
                        bestMoves.Clear();
                    }

                    if (move_val == best)
                    {
                        put_a_move(bestMoves, i, j, true, false);
                    }

                    board.remove_horizontal_line(i, j);
                }
            }
        }

        for (int i = 0; i < GameData.Rows - 1; i++)
        {
            for (int j = 0; j < GameData.Columns; j++)
            {
                if (board.vertical_lines[i][j] == false)
                {
                    board.add_vertical_line(i, j, player);

                    int move_val = minimax(board, 0, !isMax, maxDepth, -5000, 5000);

                    if ((move_val > best && isMax) || (move_val < best && !isMax))
                    {
                        best = move_val;
                        bestMoves.Clear();
                    }

                    if (move_val == best)
                    {
                        put_a_move(bestMoves, i, j, false, true);
                    }

                    board.remove_vertical_line(i, j);
                }
            }
        }
    }

    void add_optimal_moves_vertical()
    {

        for (int i = GameData.Columns - 2; i > 1; i--)
        {
            Move move = new Move();

            move.move_x_cord = 0;
            move.move_y_cord = i;
            move.isHorizontal = false;
            move.isVertical = true;

            vertical_optimal_moves.Add(move);
        }

        for (int i = 1; i < GameData.Columns - 2; i++)
        {
            Move move = new Move();

            move.move_x_cord = GameData.Rows - 2;
            move.move_y_cord = i;
            move.isHorizontal = false;
            move.isVertical = true;

            vertical_optimal_moves.Add(move);
        }
    }

    void add_optimal_moves_horizontal()
    {
        for (int i = GameData.Rows - 2; i > 1; i--)
        {
            Move move = new Move();

            move.move_x_cord = i;
            move.move_y_cord = GameData.Columns - 2;
            move.isHorizontal = true;
            move.isVertical = false;

            horizontal_optimal_moves.Add(move);
        }

        for (int i = 1; i < GameData.Rows - 2; i++)
        {
            Move move = new Move();

            move.move_x_cord = i;
            move.move_y_cord = 0;
            move.isHorizontal = true;
            move.isVertical = false;

            horizontal_optimal_moves.Add(move);
        }
    }

    void Start()
    {
        add_optimal_moves_vertical();
        add_optimal_moves_horizontal();
    }

    void check_optimal_move(Move move, bool is_horizontal)
    {
        if (is_horizontal)
        {
            foreach (Move iterator in horizontal_optimal_moves)
            {
                if (iterator.move_x_cord == move.move_x_cord && iterator.move_y_cord == move.move_y_cord)
                {
                    optimal_moves.Add(move);
                    break;
                }
            }
        }
        else
        {
            foreach (Move iterator in vertical_optimal_moves)
            {
                if (iterator.move_x_cord == move.move_x_cord && iterator.move_y_cord == move.move_y_cord)
                {
                    optimal_moves.Add(move);
                    break;
                }
            }
        }
    }

    public Move play_next_move(BoardState board, int maxDepth)
    {
        List<Move> bestMoves = new List<Move>();

        optimal_moves.Clear();

        try_to_take_box(board, bestMoves);

        if (bestMoves.Count == 0)
        {
            if (GameData.Level_Of_Ai == 0)
            {
                take_random_move(board, bestMoves);
            }
            else
            {
                take_minimax_move(board, bestMoves, maxDepth);
            }
        }


        if (GameData.Level_Of_Ai == 2)
        {
            foreach (Move bestMove in bestMoves)
            {
                check_optimal_move(bestMove, bestMove.isHorizontal);
            }

            if (optimal_moves.Count > 0)
            {
                int randomInt = Random.Range(0, optimal_moves.Count);

                return optimal_moves[randomInt];
            }
            else
            {
                int randomInt = Random.Range(0, bestMoves.Count);

                return bestMoves[randomInt];
            }
        }
        else
        {
            int randomInt = Random.Range(0, bestMoves.Count);

            return bestMoves[randomInt];
        }
    }
}
