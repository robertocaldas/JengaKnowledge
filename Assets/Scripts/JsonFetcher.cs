
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace GTSchool.Assessment.KnowledgeJenga
{
    public class JsonFetcher
    {
        public IEnumerator FetchData<T>(string url, Action<T> onSuccess, Action<string> onFailure = null)
        {
            yield return DoFetchData<T>(url, onSuccess, onFailure);
        }

        public IEnumerator FetchDataList<T>(string url, Action<List<T>> onSuccess, Action<string> onFailure = null)
        {
            void callback(JsonList<T> list)
            {
                onSuccess?.Invoke(list.objects);
            }
            yield return DoFetchData<JsonList<T>>(url, callback, onFailure, "{\"objects\":");
        }

        private IEnumerator DoFetchData<T>(string url, Action<T> onSuccess, Action<string> onFailure = null, string wrapper = null)
        {
            using UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                    var text = wrapper == null ? request.downloadHandler.text
                        : wrapper + request.downloadHandler.text + "}";
                    var jsonObject = JsonUtility.FromJson<T>(text);
                    onSuccess?.Invoke(jsonObject);
                    break;
                default:
                    onFailure?.Invoke(request.error);
                    break;
            }
        }

        [Serializable]
        private class JsonList<T>
        {
            public List<T> objects;
        }
    }
}