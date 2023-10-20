using System.Collections;
using TMPro;
using UnityEngine;
namespace GTSchool.Assessment.KnowledgeJenga
{
    public class JengaStackUIController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private JengaStack jengaStack;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => jengaStack.Initialized);
            text.text = jengaStack.Name;
        }
    }
}