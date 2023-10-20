using System.Collections;
using UnityEngine;

namespace GTSchool.Assessment.KnowledgeJenga
{
    public class CameraController : MonoBehaviour
    {
        private const float MouseRotationSpeed = 2f;
        private const float RotationSpeed = 1f;

        [SerializeField]
        private JengaTable table;
        private bool initialized;
        private Vector3 tableCenter => table.transform.position;
        private Transform cameraHolder => transform.parent;
        private bool rotating;
        private Quaternion targetHolderRotation;
        private Quaternion initialHolderRotation;
        private Vector3 initialHolderPosition;

        private Vector3 initialBricksCenter;

        private Vector3 targetBricksCenter;
        private float time;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => table.Initialized);
            initialized = true;
            transform.LookAt(table.CurrentStack.CalculateBricksCenter());
        }

        private void Update()
        {
            if (!initialized)
            {
                return;
            }

            if (rotating)
            {
                RotateTowardsFocusedStack();
            }
            else
            {
                transform.LookAt(table.CurrentStack.CalculateBricksCenter());
                OrbitAroundFocusedStack();
            }
        }

        /// <summary>
        /// Calculate initial and final (target) positions/rotations to move/rotate 
        /// both Camera transform and its parent (CameraHolder).
        /// </summary>
        private void SetupRotateTowardsFocusedStack(bool clockwise, JengaStack initialStack, JengaStack targetStack)
        {
            rotating = true;

            initialBricksCenter = initialStack.CalculateBricksCenter();
            targetBricksCenter = targetStack.CalculateBricksCenter();

            var currentDirection = initialStack.Position - tableCenter;
            currentDirection.Set(currentDirection.x, 0, currentDirection.z);
            var targetDirection = targetStack.Position - tableCenter;
            targetDirection.Set(targetDirection.x, 0, targetDirection.z);

            // angle between center of table and two stacks:
            var angle = Vector3.Angle(currentDirection, targetDirection);

            targetHolderRotation = Quaternion.AngleAxis((clockwise ? 1 : -1) * angle, Vector3.up);
            initialHolderRotation = cameraHolder.rotation;
            initialHolderPosition = cameraHolder.position;
        }

        /// <summary>
        /// Lerps from/to values calculated in SetupRotateTowardsFocusedStack;
        /// Must be called during Update().
        /// </summary>
        private void RotateTowardsFocusedStack()
        {
            time += Time.deltaTime;
            var percentageDone = RotationSpeed * time;

            var amountingRotation = Quaternion.Lerp(
                Quaternion.identity,
                targetHolderRotation,
                percentageDone);
            
            var rotatedPosition = amountingRotation * (initialHolderPosition - tableCenter) + tableCenter;
            
            var rotationQuaternion = initialHolderRotation * amountingRotation;
            
            // There are two movements for the CameraHolder:
            // - Translation around the table center:
            // - Rotation around its axis:
            cameraHolder.SetPositionAndRotation(rotatedPosition, rotationQuaternion);

            var lerpLookAt = Vector3.Lerp(initialBricksCenter, targetBricksCenter, percentageDone);
            transform.LookAt(lerpLookAt);

            if (percentageDone >= 1f)
            {
                time = 0;
                rotating = false;
            }
        }

        private void OrbitAroundFocusedStack()
        {
            if (rotating)
            {
                return;
            }

            if (Input.GetMouseButton(0))
            {
                float h = MouseRotationSpeed * Input.GetAxis("Mouse X");
                float v = MouseRotationSpeed * Input.GetAxis("Mouse Y");

                var bricksCenter = table.CurrentStack.CalculateBricksCenter();

                // uses parent transform (cameraHolder) to prevent camera flipping flicker 
                cameraHolder.RotateAround(bricksCenter, Vector3.up, h);
                transform.RotateAround(bricksCenter, cameraHolder.right, v);
                transform.LookAt(bricksCenter);
            }
        }

        public void FocusOnNextStack()
        {
            DoFocusOnStack(false);
        }

        public void FocusOnPreviousStack()
        {
            DoFocusOnStack(true);
        }

        private void DoFocusOnStack(bool clockwise)
        {
            if (!initialized || rotating)
            {
                return;
            }

            var lastStack = table.CurrentStack;
            var nextStack = clockwise ? table.SwitchToPreviousStack() : table.SwitchToNextStack();
            SetupRotateTowardsFocusedStack(clockwise, lastStack, nextStack);
        }
    }
}

