using UnityEngine;
namespace GTSchool.Assessment.KnowledgeJenga
{
    // TODO: There isn't a big reason for this to be a MonoBehaviour, 
    // only the convinience of assigning things via Editor.
    // It can be refactored if necessary.
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        JengaTable table;
        public void TestMyStack()
        {
            var stack = table.CurrentStack;
            foreach(var block in stack.Blocks)
            {
                if(block.Mastery == MasteryLevel.Glass)
                    Destroy(block.gameObject);
            }
        }
    }
}
