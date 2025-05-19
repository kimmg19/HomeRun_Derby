using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestInfo
{
    public string questName;
    public int questReward;
}

[CreateAssetMenu(fileName ="Quest",menuName ="HomerunDerby/Quest")]
public class ReteatableQuestSO : ScriptableObject
{
    [SerializeField]QuestInfo quest;
}
