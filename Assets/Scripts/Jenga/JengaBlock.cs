using System;
using UnityEngine;

namespace GTSchool.Assessment.KnowledgeJenga
{
    public enum MasteryLevel
    {
        Glass = 0,
        Wood = 1,
        Stone = 2
    }
    public class JengaBlock : MonoBehaviour
    {
        private const float Offset = 0.005f;
        [SerializeField]
        private GameObject block;
        [SerializeField]
        private Material glassMaterial;
        [SerializeField]
        private Material woodMaterial;
        [SerializeField]
        private Material stoneMaterial;

        private KnowledgeBlock knowledgeBlock;
        public JengaStack JengaStack { get; private set; }
        public float Width => block.transform.localScale.x + Offset;
        public float Height => block.transform.localScale.y;
        public float Depth => block.transform.localScale.z;
        public MasteryLevel Mastery => (MasteryLevel)Enum.ToObject(
            typeof(MasteryLevel), knowledgeBlock.Mastery);
        public string Grade => knowledgeBlock.Grade;
        public string Domain => knowledgeBlock.Domain;
        public string Cluster => knowledgeBlock.Cluster;
        public string StandardId => knowledgeBlock.StandardId;
        public string StandardDescription => knowledgeBlock.StandardDescription;
        public Vector3 Position => block.transform.position;
        public event Action<JengaBlock> Destroying;
        public void Initialize(JengaStack stack, KnowledgeBlock knowledgeBlock)
        {
            JengaStack = stack;
            this.knowledgeBlock = knowledgeBlock;
            var mr = block.GetComponent<MeshRenderer>();
            switch (Mastery)
            {
                case MasteryLevel.Glass:
                    mr.sharedMaterial = glassMaterial;
                    break;
                case MasteryLevel.Wood:
                    mr.sharedMaterial = woodMaterial;
                    break;
                case MasteryLevel.Stone:
                    mr.sharedMaterial = stoneMaterial;
                    break;
                default:
                    Debug.LogError($"Unkown mastery level {knowledgeBlock.Mastery}, setting block id {knowledgeBlock.Id} to glass.");
                    mr.sharedMaterial = glassMaterial;
                    break;
            }
        }
        private void OnDestroy()
        {
            Destroying?.Invoke(this);
        }
    }
}