using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : Singleton<GridGenerator>
{
    

    //bg 블록 그리드
    public GameObject grid;
    //bg 블록 프리팹
    public GameObject prefabBlock;

    public void Init()
    {
        for(int i = 0; i < Const.GRID_COLUMN_COUNT; ++i)
        {
            for(int j = 0; j < Const.GRID_ROW_COUNT; ++j)
            {
                GameObject cloneGrid = (GameObject)Instantiate(prefabBlock);
                cloneGrid.name = string.Format("BG_GRID{0}-{1}", i + 1, j + 1);
                cloneGrid.transform.SetParent(grid.transform);
                cloneGrid.transform.localScale = Vector3.one;
                GridData data = cloneGrid.GetComponent<GridData>();
                if(data)
                {
                    data.column = j + 1;
                    data.row = i + 1;
                    data.blockType = E_BLOCK_TYPE.NONE;
                    int key = BlockDefine.GetGridKey(data.column, data.row);
                    GameManager.Instance.AddGridData(key, cloneGrid);
                }
            }
        }
    }
}
