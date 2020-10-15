using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkAlgorithm
{
    List<List<GameObject>> blockMatrix;
    int maxX;  // x can't reach maxX.
    int maxY;  // y can't reach maxY.
    Vector3Int[] neighborDirections = {
        Vector3Int.up,
        Vector3Int.right,
        Vector3Int.down,
        Vector3Int.left
    };

    public void Initialize(List<List<GameObject>> blockMatrix, int maxX, int maxY)
    {
        this.blockMatrix = blockMatrix;
        this.maxX = maxX;
        this.maxY = maxY;
    }

    public List<Vector3Int> Linkable(GameObject left, GameObject right)
    {
        Vector3Int leftPosition = left.GetComponent<Block>().getLogicalPosition();
        Vector3Int rightPosition = right.GetComponent<Block>().getLogicalPosition();

        List<Vector3Int> turns = new List<Vector3Int>();
        if (LinkableWithTwoTurns(turns, leftPosition, rightPosition))
        {
            return turns;
        }
        return null;
    }

    bool LinkableWithTwoTurns(List<Vector3Int> turns, Vector3Int left, Vector3Int right)
    {
        if (LinkableWithOneTurn(turns, left, right))
        {
            return true;
        }

        int maxDistance = Math.Max(
          Math.Max(right.x, maxX - right.x),
          Math.Max(right.y, maxY - right.y)
        );
        for (int distance = 0; distance < maxDistance; distance++)
        {
            foreach (var neighborDirection in neighborDirections)
            {
                Vector3Int candidateTurn = right + distance * neighborDirection;
                if (candidateTurn.x < -1 || candidateTurn.x > maxX
                    || candidateTurn.y < -1 || candidateTurn.y > maxY)
                {
                    continue;
                }
                if (!IsMatrixEmtpyAt(candidateTurn))
                {
                    continue;
                }
                if (LinkableWithoutTurn(candidateTurn, right)
                     && LinkableWithOneTurn(turns, candidateTurn, left))
                {
                    turns.Add(candidateTurn);
                    return true;
                }
            }
        }

        return false;
    }

    bool LinkableWithOneTurn(List<Vector3Int> turns, Vector3Int left, Vector3Int right)
    {
        if (LinkableWithoutTurn(left, right))
        {
            return true;
        }

        Vector3Int candidateTurn1 = new Vector3Int(left.x, right.y, 0);
        if (IsMatrixEmtpyAt(candidateTurn1)
            && LinkableWithoutTurn(left, candidateTurn1)
            && LinkableWithoutTurn(right, candidateTurn1))
        {
            turns.Add(candidateTurn1);
            return true;
        }

        Vector3Int candidateTurn2 = new Vector3Int(right.x, left.y, 0);
        if (IsMatrixEmtpyAt(candidateTurn2)
            && LinkableWithoutTurn(left, candidateTurn2)
            && LinkableWithoutTurn(right, candidateTurn2))
        {
            turns.Add(candidateTurn2);
            return true;
        }

        return false;
    }

    bool LinkableWithoutTurn(Vector3Int left, Vector3Int right)
    {
        return LinkableHorizontally(left, right) || LinkableVertically(left, right);
    }

    bool LinkableHorizontally(Vector3Int left, Vector3Int right)
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

    bool LinkableVertically(Vector3Int left, Vector3Int right)
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

    bool IsMatrixEmtpyAt(Vector3Int position)
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