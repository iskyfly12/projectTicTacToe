using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBehaviour : MonoBehaviour
{
    [Header("Cells")]
    [SerializeField] private GameObject[] gridObject;

    [Header("References")]
    [SerializeField] private PointerBehaviour pointerBehaviour;

    private const int rows = 3;
    private const int cols = 3;

    private PeaceType currentPlayer;
    private Grid[,] grids = new Grid[rows, cols];
    private AI ai;

    private event Action<PeaceType, int, int> onBoardChange;

    public void Init(Action<PeaceType, int, int> onBoardChangeAction)
    {
        ai = new AI(this, rows, cols);

        currentPlayer = PeaceType.None;
        onBoardChange = onBoardChangeAction;

        int num = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Grid grid = gridObject[num].GetComponent<Grid>();

                grids[i, j] = grid;
                grid.Init(i, j, OnGridClick, OnGridSelect);

                num++;
            }
        }
    }

    //--------------------------------------------------------
    //BOARD BEHAVIOURS
    #region Board Methods
    public void SetCurrentPlayer(PeaceType player)
    {
        currentPlayer = player;
    }

    public void BlockBoard(bool block)
    {
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                grids[i, j].gameObject.SetActive(!block);
    }

    public bool IsBoardFull()
    {
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                if (grids[i, j].isEmpty)
                    return false;

        return true;
    }
    public void ClearBoard()
    {
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                grids[i, j].Clear();
    }

    public bool CheckWon(PeaceType peaceType)
    {
        return HasWonInTheRow(peaceType)
                || HasWonInTheColumn(peaceType)
                || HasWonInTheDiagonal(peaceType)
                || HasWonInTheOppositeDiagonal(peaceType);
    }
    #endregion

    //--------------------------------------------------------
    //VICTORY CONDITIONS
    #region Victory Conditions
    private bool HasWonInTheRow(PeaceType peaceType)
    {
        for (int i = 0; i < rows; i++)
        {
            if (grids[i, 0].HasPeace(peaceType) && 
                grids[i, 1].HasPeace(peaceType) &&
                grids[i, 2].HasPeace(peaceType))
                return true;
        }
        return false;
    }

    private bool HasWonInTheColumn(PeaceType peaceType)
    {
        for (int i = 0; i < cols; i++)
        {
            if (grids[0, i].HasPeace(peaceType) &&
                grids[1, i].HasPeace(peaceType) &&
                grids[2, i].HasPeace(peaceType))
                return true;
        }
        return false;
    }

    private bool HasWonInTheDiagonal(PeaceType peaceType)
    {
        return grids[0, 0].HasPeace(peaceType)
            && grids[1, 1].HasPeace(peaceType)
            && grids[2, 2].HasPeace(peaceType);
    }

    private bool HasWonInTheOppositeDiagonal(PeaceType peaceType)
    {
        return grids[0, 2].HasPeace(peaceType)
            && grids[1, 1].HasPeace(peaceType)
            && grids[2, 0].HasPeace(peaceType);
    }
    #endregion

    //--------------------------------------------------------
    //EVENTS
    #region Interactions
    private void OnGridClick(Grid grid)
    {
        if (currentPlayer != PeaceType.None && grid.isEmpty)
        {
            grid.SetPeace(currentPlayer);

            if (onBoardChange != null)
                onBoardChange(currentPlayer, grid.row, grid.col);

            pointerBehaviour.HidePointer();
        }
    }

    private void OnGridSelect(Grid grid)
    {
        if (!grid.isEmpty)
            pointerBehaviour.HidePointer();
        else
            pointerBehaviour.ShowPointer(grid.transform);
    }
    #endregion

    //--------------------------------------------------------
    //CPU MOVE
    public void CPUMove(GameDifficulty difficulty)
    {
        if (ai == null)
            ai = new AI(this, rows, cols);

        (int, int) move = ai.CPUMove(difficulty, grids);

        OnGridClick(grids[move.Item1, move.Item2]);
    }
}