using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LinkAlgorithm
{
    List<List<GameObject>> blockMatrix;
    int maxX;  // x can't reach maxX.
    int maxY;  // y can't reach maxY.

    public void Initialize(List<List<GameObject>> blockMatrix, int maxX, int maxY)
    {
        this.blockMatrix = blockMatrix;
        this.maxX = maxX;
        this.maxY = maxY;
    }

    public bool Linkable(GameObject left, GameObject right)
    {
        (int x, int y) leftPosition = left.GetComponent<Block>().getLogicalPosition();
        (int x, int y) rightPosition = right.GetComponent<Block>().getLogicalPosition();

        List<(int x, int y)> turns = new List<(int x, int y)>();
        if (LinkableWithoutTurn(leftPosition, rightPosition))
        {
            return true;
        }

        if (LinkableWithOneTurn(turns, leftPosition, rightPosition))
        {
            return true;
        }

        if (LinkableWithTwoTurns(turns, leftPosition, rightPosition))
        {
            return true;
        }
        return false;
    }

    bool LinkableWithTwoTurns(
      List<(int x, int y)> turns,
      (int x, int y) left,
      (int x, int y) right)
    {
        // Scan vertically along left.
        for (int x = -1; x <= maxX; x++)
        {
            (int x, int y) candidateTurn = (x, left.y);
            if (!IsMatrixEmtpyAt(candidateTurn))
            {
                continue;
            }
            if (LinkableWithoutTurn(candidateTurn, left)
                && LinkableWithOneTurn(turns, candidateTurn, right))
            {
                turns.Add(candidateTurn);
                return true;
            }
        }

        // Scan horizontally along left.
        for (int y = -1; y <= maxY; y++)
        {
            (int x, int y) candidateTurn = (left.x, y);
            if (!IsMatrixEmtpyAt(candidateTurn))
            {
                continue;
            }
            if (LinkableWithoutTurn(candidateTurn, left)
                && LinkableWithOneTurn(turns, candidateTurn, right))
            {
                turns.Add(candidateTurn);
                return true;
            }
        }

        return false;
    }

    bool LinkableWithOneTurn(
      List<(int x, int y)> turns,
      (int x, int y) left,
      (int x, int y) right)
    {
        (int x, int y) candidateTurn1 = (left.x, right.y);
        if (IsMatrixEmtpyAt(candidateTurn1)
            && LinkableWithoutTurn(left, candidateTurn1)
            && LinkableWithoutTurn(right, candidateTurn1))
        {
            turns.Add(candidateTurn1);
            return true;
        }

        (int x, int y) candidateTurn2 = (right.x, left.y);
        if (IsMatrixEmtpyAt(candidateTurn2)
            && LinkableWithoutTurn(left, candidateTurn2)
            && LinkableWithoutTurn(right, candidateTurn2))
        {
            turns.Add(candidateTurn2);
            return true;
        }

        return false;
    }

    bool LinkableWithoutTurn((int x, int y) left, (int x, int y) right)
    {
        return LinkableHorizontally(left, right) || LinkableVertically(left, right);
    }

    bool LinkableHorizontally((int x, int y) left, (int x, int y) right)
    {
        if (left.y != right.y)
        {
            return false;
        }
        int y = left.y;
        if (y <= -1 || y >= maxY)
        {
            return true;
        }

        int leftX = left.x;
        int rightX = right.x;
        if (leftX > rightX)
        {
            (leftX, rightX) = (rightX, leftX);
        }

        for (int i = leftX + 1; i < rightX; i++)
        {
            if (blockMatrix[i][y] != null)
            {
                return false;
            }
        }
        return true;
    }

    bool LinkableVertically((int x, int y) left, (int x, int y) right)
    {
        if (left.x != right.x)
        {
            return false;
        }
        int x = left.x;
        if (x <= -1 || x >= maxX)
        {
            return true;
        }

        int leftY = left.y;
        int rightY = right.y;
        if (leftY > rightY)
        {
            (leftY, rightY) = (rightY, leftY);
        }

        for (int i = leftY + 1; i < rightY; i++)
        {
            if (blockMatrix[x][i] != null)
            {
                return false;
            }
        }
        return true;
    }

    bool IsMatrixEmtpyAt((int x, int y) position)
    {
        if (position.x <= -1 || position.x >= maxX)
        {
            return true;
        }
        if (position.y <= -1 || position.y >= maxY)
        {
            return true;
        }
        return blockMatrix[position.x][position.y] == null;
    }
}