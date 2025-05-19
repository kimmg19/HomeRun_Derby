using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecordArray : MonoBehaviour
{
    [SerializeField] List<GameObject> recordArray;
    [SerializeField] GameObject content;
    [SerializeField] GameObject recordBoxPrefab;
    Queue<HitRecord> hitRecords;
    Queue<PitchRecord> pitchRecords;
    void OnEnable()
    {
        EventManager.Instance.OnGameFinished += Record;        
    }
    void OnDisable()
    {
        EventManager.Instance.OnGameFinished -= Record;

    }
    void Record()
    {
        hitRecords = ScoreManager.Instance.GetHitRecord();
        pitchRecords = ScoreManager.Instance.GetPitchRecord();

        int pitchRecordCnt = pitchRecords.Count;
        int hitRecordCnt = hitRecords.Count;
        
        
        int dif = pitchRecordCnt - recordArray.Count;
        if (recordArray.Count < hitRecordCnt)
        {
            for (int i = 0; i < dif; i++)
            {
                GameObject array = Instantiate(recordBoxPrefab, content.transform);
                recordArray.Add(array);
            }
        }
        for (int i = 0; i < hitRecordCnt; i++)
        {
            recordArray[i].SetActive(true);
            HitRecord hitRecord = hitRecords.Dequeue();
            PitchRecord pitchRecord = pitchRecords.Dequeue();

            TextMeshProUGUI[] DataText = recordArray[i].GetComponentsInChildren<TextMeshProUGUI>();

            DataText[0].text = pitchRecord.GetEPitchType().ToString();
            DataText[1].text = pitchRecord.GetSpeed().ToString();
            DataText[2].text = pitchRecord.GetPitchLocation().ToString();
            DataText[3].text = hitRecord.GetDistance().ToString();
            DataText[4].text = hitRecord.GetTiming().ToString();
            DataText[5].text = hitRecord.GetScore().ToString();
            DataText[6].text=(i+1).ToString();
        }
    }
}
