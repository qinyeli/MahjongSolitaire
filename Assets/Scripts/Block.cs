using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    Vector3Int logicalPosition;
    int blockType;

    ///////////////////////////////////////////////////////////////////////////
    // Getters and setters.
    // ///////////////////////////////////////////////////////////////////////////
    public Block SetTypeID(int blockType)
    {
        this.blockType = blockType;
        return this;
    }

    public Block SetSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        return this;
    }

    public Block SetPhysicalPosition(Vector3 physicalPosition)
    {
        transform.position = physicalPosition;
        return this;
    }

    public Block SetLogicalPosition(Vector3Int logicalPosition)
    {
        this.logicalPosition = logicalPosition;
        return this;
    }

    public Vector3Int getLogicalPosition()
    {
        return logicalPosition;
    }

    public void SetTransparency(float transparency)
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = transparency;
        GetComponent<SpriteRenderer>().color = color;
    }

    public static bool IsSameType(GameObject left, GameObject right)
    {
        return left.GetComponent<Block>().blockType == right.GetComponent<Block>().blockType;
    }
}
