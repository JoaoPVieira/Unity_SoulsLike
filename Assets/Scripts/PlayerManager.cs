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

        CameraHandler cameraHandler;

        PlayerMovement playerMovement;

        private void Awake()
        {
            cameraHandler = CameraHandler.singleton;
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

            inputHandler.TickInput(delta);

            playerMovement.HandleMovement(delta);
            playerMovement.HandleRollingAndSprinting(delta);

            float fixedDelta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(fixedDelta);
                cameraHandler.HandleCameraRotation(fixedDelta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;

            isSprinting = inputHandler.b_Input;
        }
    }
}
