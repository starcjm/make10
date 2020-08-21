using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : Singleton<GridGenerator>
{
    public struct GridKey
    {
        public int key;
        public GridKey(int x, int y)
        {
            key = x * 1000 + y;
        }
    }

    public int columnCnt;
    public int rowCnt;

    //bg 블록 그리드
    public GameObject grid;
    //bg 블록 프리팹
    public GameObject prefabBlock;

    //key = 그리드 키클래스 해쉬코드  value= 그리드 오브젝트
    public Dictionary<int, GameObject> gridObject = new Dictionary<int, GameObject>();

    public void Init()
    {
        for(int i = 0; i < columnCnt; ++i)
        {
            for(int j = 0; j < rowCnt; ++j)
            {
                GameObject cloneGrid = (GameObject)Instantiate(prefabBlock);
                cloneGrid.name = string.Format("BG_GRID{0}-{1}", i + 1, j + 1);
                cloneGrid.transform.SetParent(grid.transform);
                cloneGrid.transform.localScale = Vector3.one;
                GridData data = cloneGrid.GetComponent<GridData>();
                if(data)
                {
                    data.column = i + 1;
                    data.row = j + 1;
                    data.blockType = E_BLOCK_TYPE.NONE;
                    GridKey key = new GridKey(data.column, data.row);
                    gridObject.Add(key.key, cloneGrid);
                }
            }
        }
    }
}
