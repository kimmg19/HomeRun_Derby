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

    void Start()
    {

        hitRecords = ScoreManager.Instance.GetHitRecord();
        pitchRecords = ScoreManager.Instance.GetPitchRecord();

        int pitchRecordCnt = pitchRecords.Count;
        int hitRecordCnt = hitRecords.Count;
        if (hitRecordCnt != pitchRecordCnt) print("투구 타격 수 다름!!");
        int dif = pitchRecordCnt - recordArray.Count;
        if (recordArray.Count < pitchRecordCnt)
        {
            for (int i = 0; i < dif; i++)
            {
                GameObject array = Instantiate(recordBoxPrefab, content.transform);
                recordArray.Add(array);
            }
        }
        for (int i = 0; i < recordArray.Count; i++)
        {
            recordArray[i].SetActive(true);
            HitRecord record = hitRecords.Dequeue();
            TextMeshProUGUI[] text = recordArray[i].GetComponentsInChildren<TextMeshProUGUI>();

            text[0].text = record.GetDistance().ToString();
            text[1].text = record.GetTiming().ToString();
            text[2].text = record.GetScore().ToString();

        }
    }



}
