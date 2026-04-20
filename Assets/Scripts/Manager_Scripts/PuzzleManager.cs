///////////////////////////////////////////////////////////////////////////////
//  Project:    DepthProtocol
//  Script:     PuzzleManager.cs
//
//  Author:     Izaiah Razon
//  Created:    04/19/2026
//  Updated:    04/19/2026
//
//  Description:
//  Manages one puzzle instance in the scene.
//  Tracks linked puzzle switches, checks solve conditions, and fires a solve
//  event when the puzzle is completed.
//
//  Notes:
//  - Attach one PuzzleManager per puzzle instance.
//  - Solve conditions are editable in the Inspector.
//  - Uses UnityEvent so puzzle results can be assigned in the Inspector.
//
///////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : MonoBehaviour
{
    public enum SolveMode
    {
        AllActive,
        AnyActive,
        ExactCount
    }

    [Header("Puzzle Settings")]
    [SerializeField] private string puzzleName = "Puzzle";
    [SerializeField] private SolveMode solveMode = SolveMode.AllActive;
    [SerializeField] private int requiredActiveCount = 1;
    [SerializeField] private bool solveOnlyOnce = true;
    [SerializeField] private bool logPuzzleState = false;

    [Header("Puzzle Pieces")]
    [SerializeField] private List<PuzzleSwitch> puzzleSwitches = new List<PuzzleSwitch>();

    [Header("Puzzle Events")]
    [SerializeField] private UnityEvent onPuzzleSolved;

    private bool isSolved = false;

    public bool IsSolved => isSolved;

    private void Start()
    {
        EvaluatePuzzle();
    }

    public void RegisterSwitch(PuzzleSwitch puzzleSwitch)
    {
        if (puzzleSwitch == null)
        {
            return;
        }

        if (!puzzleSwitches.Contains(puzzleSwitch))
        {
            puzzleSwitches.Add(puzzleSwitch);
        }
    }

    public void UnregisterSwitch(PuzzleSwitch puzzleSwitch)
    {
        if (puzzleSwitch == null)
        {
            return;
        }

        puzzleSwitches.Remove(puzzleSwitch);
    }

    public void NotifySwitchStateChanged()
    {
        if (solveOnlyOnce && isSolved)
        {
            return;
        }

        EvaluatePuzzle();
    }

    public void EvaluatePuzzle()
    {
        int totalSwitches = puzzleSwitches.Count;
        int activeCount = GetActiveSwitchCount();
        bool solvedNow = CheckSolved(totalSwitches, activeCount);

        if (logPuzzleState)
        {
            Debug.Log(
                $"Puzzle [{puzzleName}] | Mode: {solveMode} | " +
                $"Active: {activeCount}/{totalSwitches} | Solved: {solvedNow}"
            );
        }

        if (solvedNow && !isSolved)
        {
            isSolved = true;

            if (logPuzzleState)
            {
                Debug.Log($"Puzzle solved: {puzzleName}");
            }

            onPuzzleSolved?.Invoke();
            return;
        }

        if (!solveOnlyOnce)
        {
            isSolved = solvedNow;
        }
    }

    private int GetActiveSwitchCount()
    {
        int activeCount = 0;

        for (int i = 0; i < puzzleSwitches.Count; i++)
        {
            if (puzzleSwitches[i] != null && puzzleSwitches[i].IsActive)
            {
                activeCount++;
            }
        }

        return activeCount;
    }

    private bool CheckSolved(int totalSwitches, int activeCount)
    {
        switch (solveMode)
        {
            case SolveMode.AllActive:
                return totalSwitches > 0 && activeCount == totalSwitches;

            case SolveMode.AnyActive:
                return activeCount > 0;

            case SolveMode.ExactCount:
                return activeCount == requiredActiveCount;

            default:
                return false;
        }
    }
}