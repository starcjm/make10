using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 그리드 생성 클래스
/// </summary>
public class GridGenerator : Singleton<GridGenerator>
{
    //bg 블록 그리드
    public GameObject BgGrid;
    //bg 블록 프리팹
    public GameObject prefabBlock;
    public Queue<int> nema;

    public void Init()
    {
        for(int i = 0; i < Const.GRID_COLUMN_COUNT; ++i)
        {
            for(int j = 0; j < Const.GRID_ROW_COUNT; ++j)
            {
                GameObject cloneGrid = Instantiate(prefabBlock);
                cloneGrid.name = string.Format("GRID{0}-{1}", j + 1, i + 1);
                cloneGrid.transform.SetParent(BgGrid.transform);
                cloneGrid.transform.localScale = Vector3.one;
                Grid grid = cloneGrid.GetComponent<Grid>();
                if(grid)
                {
                    grid.data.column = j + 1;
                    grid.data.row = i + 1;
                    grid.data.blockType = E_BLOCK_TYPE.NONE;
                    int key = BlockDefine.GetGridKey(grid.data.column, grid.data.row);
                    grid.data.key = key;
                    GameManager.Instance.AddGridData(key, cloneGrid);
                }
            }
        }
    }
}
