using System.Collections.Generic;
using UnityEngine;

namespace GTSchool.Assessment.KnowledgeJenga
{
    public class JengaTable : MonoBehaviour
    {
        [SerializeField]
        private JengaStack jengaStackPrefab;
        [SerializeField]
        private float tableRadius;

        private readonly List<JengaStack> stacks = new();
        private int stackIndex;
        public bool Initialized { get; private set; }
        public JengaStack CurrentStack => stacks[stackIndex];


        public void Initialize(Dictionary<string, List<KnowledgeBlock>> blocksDictionary)
        {

            var step = 2 * Mathf.PI / blocksDictionary.Count;
            var i = 0;
            foreach (var pair in blocksDictionary)
            {
                var stack = Instantiate<JengaStack>(jengaStackPrefab,
                    transform);
                stack.transform.localPosition = new Vector3(
                    tableRadius * Mathf.Cos(step * i),
                    0f,
                    tableRadius * Mathf.Sin(step * i++));
                stack.transform.LookAt(transform.position);
                stack.name += "_" + pair.Key;
                stack.Initialize(pair.Value);
                stacks.Add(stack);
            }
            Initialized = true;
        }

        public JengaStack SwitchToNextStack()
        {
            stackIndex = (stackIndex + 1) % stacks.Count;
            return stacks[stackIndex];
        }

        public JengaStack SwitchToPreviousStack()
        {
            stackIndex = (stackIndex - 1 + stacks.Count) % stacks.Count;
            return stacks[stackIndex];
        }
    }
}