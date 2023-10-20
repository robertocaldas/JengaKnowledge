using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace GTSchool.Assessment.KnowledgeJenga
{
    public class MainHUDController : MonoBehaviour
    {
        [SerializeField]
        private Button nextButton;
        [SerializeField]
        private Button previousButton;
        [SerializeField]
        private Button testMyStackButton;
        [SerializeField]
        private TextMeshProUGUI detailsText;
        [SerializeField]
        CameraController cameraController;
        [SerializeField]
        GameManager gameManager;
        [SerializeField]
        JengaTable jengaTable;
        private new Camera camera;
        private int jengaBlockLayer;

        private void Awake()
        {
            jengaBlockLayer = 1 << LayerMask.NameToLayer("JengaBlock");
            nextButton.onClick.AddListener(cameraController.FocusOnNextStack);
            previousButton.onClick.AddListener(cameraController.FocusOnPreviousStack);
            testMyStackButton.onClick.AddListener(gameManager.TestMyStack);
            camera = cameraController.GetComponent<Camera>();
        }

        private void Update()
        {
            detailsText.text = string.Empty;
            
            if(!jengaTable.Initialized)
            {
                return;
            }

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, jengaBlockLayer))
            {
                var block = hit.transform.parent.GetComponent<JengaBlock>();
                if(block.JengaStack == jengaTable.CurrentStack)
                {
                    detailsText.text = $"{block.Grade}: {block.Domain}\n\n{block.Cluster}\n\n{block.StandardId}: {block.StandardDescription}";
                }
            }
        }
    }
}
