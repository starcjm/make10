using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 블록 데이터
/// </summary>
/// 

[Serializable]
public class BlockData : BlockParentData
{
    //합쳐질때 마지막 블록인지 플래그
    public bool mergeLast = false;
}
