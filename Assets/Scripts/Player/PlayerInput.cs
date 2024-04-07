using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
   private Vector2 _moveInput;
   public event Action CatchPressed;
   public event Action CatchRealesed;

   public Vector2 Controls => _moveInput;
   private void Update()
   {
       _moveInput = Vector2.right * Input.GetAxis("Horizontal") + Vector2.up * Input.GetAxis("Vertical");

       if (Input.GetKeyDown(KeyCode.Mouse0))
       {
           CatchPressed?.Invoke();
       }

       if (Input.GetKeyUp(KeyCode.Mouse0))
       {
           CatchRealesed?.Invoke();
       }
   }
}
