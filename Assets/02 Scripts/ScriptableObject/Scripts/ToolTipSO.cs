using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="ToolTips",menuName ="HomerunDerby/ToolTips")]
public class ToolTipSO : ScriptableObject

{
    [TextArea]
    [SerializeField]string[] toolTips;
    public string GetToolTip()
    {
        int num=Random.Range(0, toolTips.Length);
        return toolTips[num];
    }
}
