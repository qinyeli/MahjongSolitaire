using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BlockManager : MonoBehaviour
{
    public int numBlockRow = 9;
    public int numBlockCol = 16;
    public GameObject blockPrefab;
    public Sprite[] blockSprites;

    List<int> randomTypeMap = new List<int>();  // Maps logical position to TypeID.
    List<List<GameObject>> blocks = new List<List<GameObject>>();
    Vector3 blocksCenter;
    GameObject clickedBlock;
    LinkAlgorithm linkAlgorithm = new LinkAlgorithm();

    void Start()
    {
        GenerateTypeMap();

        blocksCenter = new Vector3(
          -(float)(numBlockCol - 1) * 0.5f,
          -(float)(numBlockRow - 1) * 0.5f,
          0);

        // Generate all blocks.
        for (int x = 0; x < numBlockCol; x++)
        {
            blocks.Add(new List<GameObject>());
            for (int y = 0; y < numBlockRow; y++)
            {
                GameObject newBlock = GameObject.Instantiate(blockPrefab);
                blocks[x].Add(newBlock);
                newBlock.transform.parent = this.transform;

                Block block = newBlock.GetComponent<Block>();
                int blockType = GetBlockType(x, y);
                Vector3Int logicalPosition = new Vector3Int(x, y, 0);
                block.SetPhysicalPosition(LogicalPositionToPhysicalPosition(logicalPosition))
                    .SetLogicalPosition(logicalPosition)
                    .SetTypeID(blockType)
                    .SetSprite(blockSprites[blockType]);
            }
        }
        linkAlgorithm.Initialize(blocks, numBlockCol, numBlockRow);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject mousedOverBlock = MousedOverBlock();
            if (mousedOverBlock)
            {
                if (LinkableToClickedBlock(mousedOverBlock))
                {
                    Destroy(mousedOverBlock);
                    Destroy(clickedBlock);
                }
                else
                {
                    SetClickedBlock(mousedOverBlock);
                }
            }
            else
            {
                ResetClickedBlock();
            }
        }
    }

    void GenerateTypeMap()
    {
        int numBlockTypes = blockSprites.Length;
        Assert.IsTrue(numBlockRow * numBlockCol % numBlockTypes == 0);
        int perTypeBlockCount = numBlockRow * numBlockCol / numBlockTypes;
        Assert.IsTrue(perTypeBlockCount % 2 == 0);

        for (int type = 0; type < numBlockTypes; type++)
        {
            for (int count = 0; count < perTypeBlockCount; count++)
            {
                randomTypeMap.Add(type);
            }
        }

        Shuffle(randomTypeMap);
    }

    static void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(0, list.Count);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }

    int GetBlockType(int x, int y)
    {
        return randomTypeMap[x * numBlockRow + y];
    }

    void SetClickedBlock(GameObject block)
    {
        if (clickedBlock)
        {
            ResetClickedBlock();
        }
        clickedBlock = block;
        clickedBlock.GetComponent<Block>().SetTransparency(0.5f);
    }

    void ResetClickedBlock()
    {
        if (!clickedBlock)
        {
            return;
        }
        clickedBlock.GetComponent<Block>().SetTransparency(1f);
        clickedBlock = null;
    }

    bool LinkableToClickedBlock(GameObject block)
    {
        if (!clickedBlock || clickedBlock == block || !Block.IsSameType(clickedBlock, block))
        {
            return false;
        }
        return linkAlgorithm.Linkable(clickedBlock, block);
    }

    Vector3 LogicalPositionToPhysicalPosition(Vector3Int logicalPosition)
    {
        return logicalPosition + blocksCenter;
    }

    GameObject MousedOverBlock()
    {
        RaycastHit2D hit = Physics2D.Raycast(
          Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
        if (!hit.collider)
        {
            return null;
        }
        if (hit.collider.gameObject.GetComponent<Block>())
        {
            return hit.collider.gameObject;
        }
        return null;
    }
}
