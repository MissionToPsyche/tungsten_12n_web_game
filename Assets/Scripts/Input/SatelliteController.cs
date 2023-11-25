// using UnityEngine;
// using UnityEngine.InputSystem;
// using Cinemachine;

// public class SpaceshipController : MonoBehaviour
// {
//     // Public variables for editor access.
//     [SerializeField] public CinemachineVirtualCamera spaceshipCamera;
//     [SerializeField]public float moveSpeed = 50.0f;
//     [SerializeField] public float rotationSpeed = 100.0f;
//     private float cameraRotationSpeed = 90.0f;

//     // Private variables for internal state and component caching.
//     private Rigidbody2D rb;
//     private bool isControllerActive = false;
//     private InputAction moveAction;
//     private InputAction rotateLeftAction;
//     private InputAction rotateRightAction;
//     private bool isRotatingLeft = false;
//     private bool isRotatingRight = false;

//     private void Start()
//     {
//         // Cache the Rigidbody2D component for later use.
//         rb = GetComponent<Rigidbody2D>();

//         // Initialize input actions from the action map.
//         var spaceshipActionMap = ContextEngine.Instance.inputActions.FindActionMap("Spaceship");
//         moveAction = spaceshipActionMap.FindAction("Move");
//         rotateLeftAction = spaceshipActionMap.FindAction("RotateLeft");
//         rotateRightAction = spaceshipActionMap.FindAction("RotateRight");

//         // Subscribe to input action events.
//         rotateLeftAction.performed += context => isRotatingLeft = true;
//         rotateLeftAction.canceled += context => isRotatingLeft = false;
//         rotateRightAction.performed += context => isRotatingRight = true;
//         rotateRightAction.canceled += context => isRotatingRight = false;
//     }

//     public void EnableController()
//     {
//         isControllerActive = true;
//         spaceshipCamera.Priority = 100;
        
//         // Enable input actions.
//         moveAction.Enable();
//         rotateLeftAction.Enable();
//         rotateRightAction.Enable();
//     }

//     public void DisableController()
//     {
//         isControllerActive = false;
//         spaceshipCamera.Priority = 0;
        
//         // Disable input actions.
//         moveAction.Disable();
//         rotateLeftAction.Disable();
//         rotateRightAction.Disable();
//     }

//     void Update()
//     {
//         if (isControllerActive)
//         {
//             Quaternion currentRotation = spaceshipCamera.transform.rotation;
//             Quaternion targetRotation = transform.rotation;
            
//             spaceshipCamera.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, cameraRotationSpeed * Time.deltaTime);

//             // Read player input for movement and transform it into the spaceship's local space.
//             Vector2 movementInput = moveAction.ReadValue<Vector2>();
//             Vector3 movement = (transform.up * movementInput.y + transform.right * movementInput.x) * moveSpeed * Time.deltaTime;
//             rb.MovePosition(transform.position + movement);

//             // Check for continuous rotation input.
//             if (isRotatingLeft)
//             {
//                 transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
//             }
//             else if (isRotatingRight)
//             {
//                 transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
//             }
//         };
//     }
// }
