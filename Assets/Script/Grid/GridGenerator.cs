using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : Singleton<GridGenerator>
{
    public int columnCnt;
    public int rowId;

    //bg 블록 그리드
    public GameObject grid;
    //bg 블록 프리팹
    public GameObject prefabBlock;

    public void Init()
    {
        for(int i = 0; i < columnCnt; ++i)
        {
            for(int j = 0; j < rowId; ++j)
            {
                GameObject cloneGrid = (GameObject)Instantiate(prefabBlock);
                cloneGrid.name = string.Format("BG_GRID{0}-{1}", i + 1, j + 1);
                cloneGrid.transform.SetParent(grid.transform);
                GridData data = cloneGrid.GetComponent<GridData>();
                if(data)
                {
                    data.columnId = i + 1;
                    data.rowId = j + 1;
                    data.blockType = E_BLOCK_TYPE.NONE;
                }
            }
        }
    }
}
