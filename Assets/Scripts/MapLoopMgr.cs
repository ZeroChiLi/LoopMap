using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoopMgr : MonoBehaviour
{
    public GameObject player;
    public Vector2 blockSize = new Vector2(10, 10);
    public Vector2 blockCount = new Vector2(4, 1);
    public List<GameObject> blockList = new List<GameObject>();
    
    private int blockListCount = 0;
    private Vector2 startBlockIdx;
    private Vector2 layerSize;

    void Start()
    {
        blockListCount = blockList.Count;
        startBlockIdx = GetBlockIndex(player);
        layerSize = blockSize * blockCount;
    }

    private Vector2 blockOffset;
    private Vector2 layerOffset;
    private Vector2 blockInLayerOffset;
    private GameObject temGO;
    private Vector2 temVec2;
    void Update()
    {
        //玩家当前块和最开始块的偏移量
        blockOffset = GetBlockIndex(player) - startBlockIdx;
        //块在这个层的内的偏移量
        blockInLayerOffset = new Vector2(blockOffset.x % blockCount.x, blockOffset.y % blockCount.y);
        //层的偏移量
        layerOffset = new Vector2((int)(blockOffset.x / blockCount.x), (int)(blockOffset.y / blockCount.y));

        //Debug.Log("blockOffset :" + blockOffset + "   sectionOffset:" + sectionOffset + "  blockInSectionOffset:" + blockInSectionOffset);

        float newX, newZ;
        for (int i = 0; i < blockListCount; i++)
        {
            temGO = blockList[i];
            temVec2 = BlockIDToIdx(i);
            if (blockOffset.x > 0 && temVec2.x < blockInLayerOffset.x)  // 偏移量大于0，x正方向，把x靠前的全部移到后面
                newX = layerSize.x * (layerOffset.x + 1);
            else if (blockOffset.x < 0 && temVec2.x > (blockCount.x + blockInLayerOffset.x - 1))    // 偏移量小于0，x反方向，把x靠后的全部移到前面
                newX = layerSize.x * (layerOffset.x - 1);
            else
                newX = layerSize.x * (layerOffset.x);                   // 不在偏移范围内的设置当前值


            if (blockOffset.y > 0 && temVec2.y < blockInLayerOffset.y)
                newZ = layerSize.y * (layerOffset.y + 1);
            else if (blockOffset.y < 0 && temVec2.y > (blockCount.y + blockInLayerOffset.y - 1))
                newZ = layerSize.y * (layerOffset.y - 1);
            else
                newZ = layerSize.y * (layerOffset.y );

            temGO.transform.position = new Vector3(newX, temGO.transform.position.y, newZ);
        }
    }

    public int BlockIdxToID(Vector2 index)
    {
        return (int)(blockCount.x * index.y + index.x);
    }

    public Vector2 BlockIDToIdx(int id)
    {
        return new Vector2(id % blockCount.x, (int)(id / blockCount.x));
    }

    public int GetBlockID(GameObject go)
    {
        return BlockIdxToID(GetBlockIndex(go));
    }

    public Vector2 GetBlockIndex(GameObject go)
    {
        Vector3 pos = player.transform.position;
        Vector2 vec = new Vector2((pos.x / blockSize.x), (pos.z / blockSize.y));
        //小于0时，要-1
        if (vec.x < 0)
            vec.x = vec.x - 1;
        if (vec.y < 0)
            vec.y = vec.y - 1;

        return new Vector2((int)(vec.x), (int)(vec.y));
    }
    

}
