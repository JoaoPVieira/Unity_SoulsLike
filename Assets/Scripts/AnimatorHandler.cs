using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ST
{
    public class AnimatorHandler : MonoBehaviour
    {
        public PlayerManager playerManager;

        public Animator anim;
        InputHandler inputHandler;
        PlayerMovement playerMovement;
        int vertical;
        int horizontal;

        public bool canRotate;

        public void Initialize()
        {
            anim = GetComponent<Animator>();
            playerManager = GetComponentInParent<PlayerManager>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerMovement = GetComponentInParent<PlayerMovement>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else
            {
                v = 0; 
            }
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion

            if (isSprinting && (v > 0 || h > 0))
            {
                v = 2;
                h = horizontalMovement;
            }

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);

            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);

            Debug.Log("Animation: " + targetAnim);
            //Might need a check for falling!
            anim.CrossFade(targetAnim, 0.2f);
        }

        public void CanRotate() 
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }

        public void EnableCombo()
        {
            anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            anim.SetBool("canDoCombo", false);
        }

        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false)
                return;
               
            float delta = Time.deltaTime;
            playerMovement.rigidbody.drag = 0;

            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerMovement.rigidbody.velocity = velocity;

        }

    }
}
