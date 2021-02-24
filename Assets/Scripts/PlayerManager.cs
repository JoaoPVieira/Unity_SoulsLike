using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ST
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        Animator anim;

        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;

        CameraHandler cameraHandler;

        PlayerMovement playerMovement;

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
        }

        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");

            inputHandler.TickInput(delta);
            playerMovement.HandleRollingAndSprinting(delta);

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        private void FixedUpdate()
        {
            float fixedDelta = Time.fixedDeltaTime;

            playerMovement.HandleMovement(fixedDelta);
            playerMovement.HandleFalling(fixedDelta, playerMovement.moveDirection);
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            inputHandler.r1_Input = false;
            inputHandler.r2_Input = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;

            isSprinting = inputHandler.b_Input;

            if (isInAir)
            {
                playerMovement.inAirTimer = playerMovement.inAirTimer + Time.deltaTime;
            }
        }
    }
}
