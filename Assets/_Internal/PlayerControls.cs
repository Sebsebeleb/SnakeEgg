using System;
using System.Linq;
using System.Net.Http;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;


namespace _Internal
{
    public class PlayerControls : MonoBehaviour
    {


        public Toggle snake, number, delete;
        
        private Vector2 currentMousePos;
        
        
        private void OnEnable()
        {
            //Touch.onFingerDown += finger => { Debug.Log("Finger down"); };
        }

        public void Update()
        {
            
            bool found = false;
            Vector2 position = Vector2.zero;
            /*if (Mouse.current.leftButton.isPressed)
            {
                found = true;
                position = Mouse.current.position.ReadValue();
                //Debug.Log("Before ray: " + p);
            }*/

            if (Input.touchCount > 0) {
                position = Input.GetTouch (0).position;
                Debug.Log (Input.GetTouch (0).position);
                found = true;
            }

            if (TouchSimulation.instance != null && TouchSimulation.instance.simulatedTouchscreen.primaryTouch.isInProgress)
            {
                //Debug.Log("TouchSimulation at: " + TouchSimulation.instance.simulatedTouchscreen.primaryTouch.position.ReadValue());
                found = true;
                position = TouchSimulation.instance.simulatedTouchscreen.primaryTouch.position.ReadValue();
            }

            TouchControl touch;
            touch = Touchscreen.current.primaryTouch;

            if (touch.phase.ReadValue() == TouchPhase.Ended)
            {
                SfxSystem.ResetPitch();
            }
            
            if (Touchscreen.current.primaryTouch.isInProgress)
            {
                touch = Touchscreen.current.primaryTouch;
                //Debug.Log("Touchscreen touch at: " + Touchscreen.current.primaryTouch.position.ReadValue());
                found = true;
                position = Touchscreen.current.primaryTouch.position.ReadValue();

               
                if (touch.phase.ReadValue() == TouchPhase.Ended)
                {
                    Debug.Log("Touch ended");
                }
            }
            
            Touchscreen.current.primaryTouch.position.ReadValue();
            /*foreach (Touch touch in Touch.activeTouches)
            {
                Debug.Log("Looking at touch: " + touch);
                if (touch.valid)
                {
                    found = true;
                    position = touch.screenPosition;
                    Debug.Log("Touch at position: " + touch.screenPosition);
                }
            }*/

            if (found){
                
                Game game = FindObjectOfType<Game>();
                Vector2Int clicked = game.getClicked(position.x, position.y);
                
                Tile tile = game.getTile(clicked.x, clicked.y);

                if (tile == null)
                {
                    return;
                }
                
                if (snake.isOn)
                {
                    tile.Set(Tile.State.Snake);         
                }
                else if (number.isOn)
                {
                    tile.Set(Tile.State.Number);
                }
                else
                {
                    tile.Set(Tile.State.Empty);
                }
            }
        }

        public void OnClick(InputAction.CallbackContext ctx)
        {
            //Debug.Log("On clicked called" + ctx.control.device);
            return;
            Debug.Log(ctx.action);
            Debug.Log("Triggered");
            Debug.Log(ctx.phase);
            Debug.Log(ctx.valueType);



            var mpos = UnityEngine.InputSystem.Mouse.current.position;
            Vector2 position = new Vector2(mpos.x.EvaluateMagnitude(), mpos.y.EvaluateMagnitude());
            Debug.Log("Value is: " + position);
        }
        
        public void SetMousePos(InputAction.CallbackContext ctx)
        {
            this.currentMousePos = ctx.ReadValue<Vector2>();
            Debug.Log("Set mouse pos to: " + this.currentMousePos);
        }


        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}