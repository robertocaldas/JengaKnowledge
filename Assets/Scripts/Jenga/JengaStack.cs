using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GTSchool.Assessment.KnowledgeJenga
{
    public class JengaStack : MonoBehaviour
    {
        [SerializeField]
        private JengaBlock jengaBlockPrefab;
        public bool Initialized { get; private set; }
        public string Name { get; private set; }
        public readonly IList<JengaBlock> Blocks = new List<JengaBlock>();
        public Vector3 Position => transform.position;
        
        private void RemoveBlock(JengaBlock block)
        {
            Blocks.Remove(block);
        }
        public void Initialize(List<KnowledgeBlock> knowledgeBlocks)
        {
            // Requirement: Order the blocks in the stack starting from the bottom up, 
            //by domain name ascending, then by cluster name ascending, 
            // then by standard ID ascending
            knowledgeBlocks = knowledgeBlocks
                .OrderBy(b => b.Domain)
                .ThenBy(b => b.Cluster)
                .ThenBy(b => b.Id)
                .ToList();

            var x = jengaBlockPrefab.Width;
            var y = jengaBlockPrefab.Height;
            for (int i = 0; i < knowledgeBlocks.Count; i++)
            {
                var block = Instantiate<JengaBlock>(
                    jengaBlockPrefab,
                    transform);
                block.transform.localPosition = new Vector3(
                    (1 - i % 3) * x,
                    (i / 3 + 0.5f) * y,
                    0);
                if (i / 3 % 2 == 0)
                {
                    block.transform.RotateAround(transform.position, Vector3.up, 90);
                }
                block.name += "_" + knowledgeBlocks[i].Id;
                block.Destroying += RemoveBlock;
                block.Initialize(this, knowledgeBlocks[i]);

                Blocks.Add(block);
            }
            Name = knowledgeBlocks[0].Grade;
            Initialized = true;
        }

        /// <summary>
        /// Calculates average center position of all bricks, in World coordinates.
        /// </summary>
        public Vector3 CalculateBricksCenter()
        {
            if (!Initialized)
            {
                throw new InvalidOperationException(nameof(JengaStack) + " not initialized yet.");
            }
            return Blocks.Aggregate(
                Vector3.zero, (sum, block) => sum + block.Position)
                / Blocks.Count;
        }
    }
}