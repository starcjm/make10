using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 블럭, 그리드 부모 클래스 
/// </summary>
/// 
[Serializable]
public abstract class BlockParentData
{
    public int column { get; set; }

    public int row { get; set; }

    public int key { get; set; }
    public E_BLOCK_TYPE blockType { get; set; }
}
