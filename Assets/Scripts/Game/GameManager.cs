using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode { Player = 1, CPU = 2 }

public class GameManager : MonoBehaviour
{
    [Header("States")]
    public GameMode gameMode;
    public GameDifficulty gameDifficulty;

    [Header("References")]
    public BoardBehaviour boardBehaviour;
    public PointerBehaviour pointerBehaviour;
    public UIBehaviour uiBehaviour;

    void Start()
    {
        PostProcessingSystem.SetDepthField(5, .5f, false);

        boardBehaviour.Init(ChangeTurn);
        boardBehaviour.BlockBoard(true);
    }

    public void SetDifficulty(int difficulty)
    {
        gameDifficulty = (GameDifficulty)difficulty;
    }

    public void SetGameMode(int mode)
    {
        gameMode = (GameMode)mode;
    }

    public void StartNewGame()
    {
        PostProcessingSystem.SetDepthField(0, .5f, false);

        boardBehaviour.ClearBoard();
        boardBehaviour.SetCurrentPlayer(PeaceType.Cross);
        boardBehaviour.BlockBoard(false);
        uiBehaviour.UpdateInfoPlayer(PeaceType.Cross);
        uiBehaviour.HideResults();

        pointerBehaviour.enabled = true;
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    private void EndGame(PeaceType player)
    {
        PostProcessingSystem.SetDepthField(2, .5f, false);

        boardBehaviour.BlockBoard(true);
        uiBehaviour.ShowResults(player);
    }

    public void ChangeTurn(PeaceType player, int row, int col)
    {
        if (boardBehaviour.CheckWon(player))
        {
            EndGame(player);
        }
        else if (boardBehaviour.IsBoardFull())
        {
            EndGame(PeaceType.None);
        }
        else
        {
            PeaceType nextPlayer = player == PeaceType.Cross ? PeaceType.Nought : PeaceType.Cross;
            boardBehaviour.SetCurrentPlayer(nextPlayer);
            uiBehaviour.UpdateInfoPlayer(nextPlayer);

            if (nextPlayer == PeaceType.Nought && gameMode == GameMode.CPU)
                StartCoroutine(CPUTurn());
        }
    }

    IEnumerator CPUTurn()
    {
        boardBehaviour.BlockBoard(true);
        boardBehaviour.SetCurrentPlayer(PeaceType.Nought);

        yield return new WaitForSeconds(.5f);

        boardBehaviour.CPUMove(gameDifficulty);
        boardBehaviour.BlockBoard(false);
    }
}
