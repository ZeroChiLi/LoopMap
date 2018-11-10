using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoopMgr : MonoBehaviour
{
    public GameObject player;
    public int maxLayerRange = 3;       //层循环次数，超过回退，避免数据越界
    public Vector2 blockSize = new Vector2(10, 10);
    public Vector2 blockCount = new Vector2(4, 1);
    public List<GameObject> blockList = new List<GameObject>();

    private Vector2 lastBlockOffset;    //标记上一次更新的块偏移
    private int blockListCount = 0;
    private Vector2 startBlockIdx;
    private Vector2 layerSize;
    private Vector3 startPos;

    void Start()
    {
        blockListCount = blockList.Count;
        startBlockIdx = GetBlockIndex(player);
        layerSize = blockSize * blockCount;
        startPos = player.transform.position;
    }

    private Vector2 blockOffset;
    private Vector2 layerOffset;
    private Vector2 blockInLayerOffset;
    private GameObject temGO;
    private Vector2 temVec2;
    void Update()
    {
        //优化，避免玩家跑太远，数据越界
        ClampPlayerInRange();

        //玩家当前块和最开始块的偏移量
        blockOffset = GetBlockIndex(player) - startBlockIdx;
        if (lastBlockOffset == blockOffset)         // 优化，避免重复更新
            return;
        lastBlockOffset = blockOffset;

        //块在这个层的内的偏移量
        blockInLayerOffset = new Vector2(blockOffset.x % blockCount.x, blockOffset.y % blockCount.y);
        //层的偏移量
        layerOffset = new Vector2((int)(blockOffset.x / blockCount.x), (int)(blockOffset.y / blockCount.y));

        Debug.Log("blockOffset :" + blockOffset + "   layerOffset:" + layerOffset + "  blockInLayerOffset:" + blockInLayerOffset);

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
                newZ = layerSize.y * (layerOffset.y);

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

    private Vector3 temVec3;
    private float temFloatX;
    private float temFloatZ;
    private void ClampPlayerInRange()
    {
        temVec3 = player.transform.position;
        temFloatX = temVec3.x;
        temFloatZ = temVec3.z;
        if (temVec3.x > layerSize.x * maxLayerRange + startPos.x ||
            temVec3.x < -layerSize.x * maxLayerRange + startPos.x)
        {
            temFloatX = startPos.x;
        }
        if (temVec3.z > layerSize.y * maxLayerRange + startPos.z ||
            temVec3.z < -layerSize.y * maxLayerRange + startPos.z)
        {
            temFloatZ = startPos.z;
        }
        player.transform.position = new Vector3(temFloatX, player.transform.position.y, temFloatZ);
    }


}
