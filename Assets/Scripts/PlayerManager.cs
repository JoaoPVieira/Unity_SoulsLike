using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ST
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerMovement playerMovement;
        InteractableUI interactableUI;

        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;

        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();

            QualitySettings.vSyncCount = 0;  // VSync must be disabled
            Application.targetFrameRate = 60;
        }

        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
            interactableUI = FindObjectOfType<InteractableUI>();
        }

        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            anim.SetBool("isInAir", isInAir);

            inputHandler.TickInput(delta);
            playerMovement.HandleRollingAndSprinting(delta);

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
                playerMovement.HandleJumping();
            }

            CheckForInteractableObject();
        }

        private void FixedUpdate()
        {
            float fixedDelta = Time.fixedDeltaTime;

            playerMovement.HandleMovement(fixedDelta);
            playerMovement.HandleFalling(fixedDelta, playerMovement.moveDirection);
            
        }

        private void LateUpdate()
        {
            inputHandler.a_Input = false;
            inputHandler.rollFlag = false;
            inputHandler.r1_Input = false;
            inputHandler.r2_Input = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;

            isSprinting = inputHandler.b_Input;

            if (isInAir)
            {
                playerMovement.inAirTimer = playerMovement.inAirTimer + Time.deltaTime;
            }
        }

        public void CheckForInteractableObject()
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 1f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else 
            { 
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractableGameObject != null && inputHandler.a_Input)
                {
                    itemInteractableGameObject.SetActive(false);
                }
            }
        }
    }
}
