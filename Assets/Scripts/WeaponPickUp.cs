using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ST
{
    public class WeaponPickUp : Interactable
    {
        public WeaponItem weapon;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerMovement playerMovement;
            AnimatorHandler animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerMovement = playerManager.GetComponent<PlayerMovement>();
            animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();

            playerMovement.rigidbody.velocity = Vector3.zero; //Stops the player from moving while picking up item
            animatorHandler.PlayTargetAnimation("Pick Up Item", true); //Plays the animation of looting the item
            playerInventory.weaponsInventory.Add(weapon);
            Destroy(gameObject);
        }
    }
}