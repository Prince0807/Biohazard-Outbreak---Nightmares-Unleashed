using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Weapon Inputs
    public static Action switchWeaponInput;
    public static Action reloadInput;
    public static Action shootInput;
    public static Action oneShootInput;
    public static int weaponIndex = 0;

    // Player Movement Inputs
    public static Action jumpInput;
    public static Action crouchInput;
    [HideInInspector] public Vector2 movementInput = new Vector2();
    [HideInInspector] public Vector2 mouseInput = new Vector2();
    [HideInInspector] public bool isSprinting;
    
    private void Update()
    {
        #region Weapon Inputs
        
        if (Input.GetButtonDown("Weapon1"))
        {
            weaponIndex = 0;
            switchWeaponInput?.Invoke();
        }
        if (Input.GetButtonDown("Weapon2"))
        {
            weaponIndex = 1;
            switchWeaponInput?.Invoke();
        }
        if (Input.GetButtonDown("Weapon3"))
        {
            weaponIndex = 2;
            switchWeaponInput?.Invoke();
        }

        if (Input.GetButtonDown("Reload"))
            reloadInput?.Invoke();

        if (Input.GetButton("Shoot"))
            shootInput?.Invoke();

        if (Input.GetButtonDown("Shoot"))
            oneShootInput?.Invoke();

        #endregion

        #region Player Movement Inputs

        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");
        mouseInput.x = Input.GetAxis("Mouse X");
        mouseInput.y = Input.GetAxis("Mouse Y");

        if (Input.GetButtonDown("Crouch"))
            crouchInput?.Invoke();
        
        if(Input.GetButtonDown("Jump"))
            jumpInput?.Invoke();

        isSprinting = Input.GetButton("Sprint");

        #endregion
    }
}
