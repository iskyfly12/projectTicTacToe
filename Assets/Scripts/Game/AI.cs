using System.Collections.Generic;

public enum GameDifficulty { Easy = 1, Normal = 2, Hard = 3}

public class AIMove
{
    public int score;
    public int row;
    public int col;
    public bool possibleVictory;

    public AIMove() { }
    public AIMove(int score) { this.score = score; }
}

public class AI
{
    private BoardBehaviour boardBehaviour;
    private int aiPickPosition = 0;

    private List<AIMove> aiMoves = new List<AIMove>();
    private int rows;
    private int cols;

    public AI(BoardBehaviour board, int row, int col)
    {
        this.boardBehaviour = board;
        this.rows = row;
        this.cols = col;
    }

    public (int, int) CPUMove(GameDifficulty difficulty, Grid[,] grids)
    {
        (int, int) move;
        switch (difficulty)
        {
            case GameDifficulty.Easy:
                move = CPUEasyPlay(grids);
                return (move.Item1, move.Item2);
            case GameDifficulty.Normal:
                move = CPUNormalPlay(grids);
                return (move.Item1, move.Item2);
            case GameDifficulty.Hard:
                move = CPUHardPlay(grids);
                return (move.Item1, move.Item2);
            default:
                return (0, 0);
        }
    }

    //CPU EASY: Get random position 
    private (int, int) CPUEasyPlay(Grid[,] grids)
    {
        bool loop = true;
        int row = 0;
        int col = 0;
        while (loop)
        {
            row = UnityEngine.Random.Range(0, grids.GetLength(0));
            col = UnityEngine.Random.Range(0, grids.GetLength(1));
            if (grids[row, col].isEmpty)
                break;
        }
        return (row, col);
    }

    //CPU NORMAL: Get random position if there is no victory condition or lose condition
    private (int, int) CPUNormalPlay(Grid[,] grids)
    {
        AIMove goodMove = GetGoodPosition(grids);
        if (goodMove.possibleVictory)
            return (goodMove.row, goodMove.col);
        else
            return CPUEasyPlay(grids);
    }

    //CPU HARD: Simulate future moves and try to get the best move based on scores
    private (int, int) CPUHardPlay(Grid[,] grids)
    {
        // X = 1 | O = -1;
        int aiPlayChoice = (int)PeaceType.Nought;  
        aiPickPosition = aiPlayChoice;

        AIMove bestMove = GetBestPosition(grids, aiPickPosition, 0);
        return (bestMove.row, bestMove.col);
    }

    //Go over all possible moves and return the best result 
    private AIMove GetBestPosition(Grid[,] grids, int playerRoute, int depth)
    {
        if (boardBehaviour.CheckWon(PeaceType.Nought))
            return new AIMove(10 - depth);
        else if (boardBehaviour.CheckWon(PeaceType.Cross))
            return new AIMove(depth - 10);
        else if (boardBehaviour.IsBoardFull())
            return new AIMove(0);

        depth++;

        AIMove bestPosition = CreateBestPosition(playerRoute);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grids[i, j].isEmpty)
                {
                    grids[i, j].SimulatePeace((PeaceType)playerRoute);
                    AIMove newPosition = GetBestPosition(grids, playerRoute * (-1), depth);

                    if (aiPickPosition != playerRoute)
                    {
                        if (newPosition.score <= bestPosition.score)
                        {
                            bestPosition.score = newPosition.score;
                            bestPosition.row = i;
                            bestPosition.col = j;
                        }
                    }
                    else
                    {
                        if (newPosition.score >= bestPosition.score)
                        {
                            bestPosition.score = newPosition.score;
                            bestPosition.row = i;
                            bestPosition.col = j;
                        }
                    }
                    grids[i, j].Clear();
                }
            }
        }
        return bestPosition;
    }

    //Go over all the board and return the postion of vitory or defeat
    private AIMove GetGoodPosition(Grid[,] grids)
    {
        AIMove bestPosition = new AIMove();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grids[i, j].isEmpty)
                {
                    grids[i, j].SimulatePeace(PeaceType.Nought);
                    if (boardBehaviour.CheckWon(PeaceType.Nought))
                    {
                        bestPosition.row = i;
                        bestPosition.col = j;
                        bestPosition.possibleVictory = true;

                        grids[i, j].Clear();
                        return bestPosition;
                    }

                    grids[i, j].SimulatePeace(PeaceType.Cross);
                    if (boardBehaviour.CheckWon(PeaceType.Cross))
                    {
                        bestPosition.row = i;
                        bestPosition.col = j;
                        bestPosition.possibleVictory = true;

                        grids[i, j].Clear();
                        return bestPosition;
                    }
                    grids[i, j].Clear();
                }
            }
        }
        return bestPosition;
    }

    private AIMove CreateBestPosition(int playerRoute)
    {
        AIMove bestPosition = new AIMove();

        if (aiPickPosition != playerRoute)
            bestPosition.score = int.MaxValue;
        else
            bestPosition.score = int.MinValue;

        return bestPosition;
    }
}
