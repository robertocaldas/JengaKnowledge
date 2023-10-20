using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GTSchool.Assessment.KnowledgeJenga
{
    public class Main : MonoBehaviour
    {
        [SerializeField]
        private JengaTable jengaTable;
        private const string stackServiceURL = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";
        private IEnumerator Start()
        {
            void failureCallback(string error)
            {
                Debug.LogError(error);
            }
            JsonFetcher fetcher = new();
            yield return fetcher.FetchDataList<KnowledgeBlock>(stackServiceURL, OnFetched, failureCallback);
        }

        private void OnFetched(List<KnowledgeBlock> knowledgeBlocks)
        {
            var groupedByGrade = knowledgeBlocks
                .GroupBy(block => block.Grade)
                .ToDictionary(group => group.Key, group => group.ToList());

            // The requirements ask for only 3 stacks max
            groupedByGrade = groupedByGrade
                .Take(Mathf.Min(3, groupedByGrade.Count))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            jengaTable.Initialize(groupedByGrade);
        }
    }
}