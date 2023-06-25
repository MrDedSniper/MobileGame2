using Game.Car;
using JoostenProductions;
using UnityEngine;

namespace Game.InputLogic
{
    internal class InputKeyboard : BaseInputView
    {
        [SerializeField] private float _inputMultiplier = 0.05f;


       

        protected override void Move()
        {
            float moveValue = Speed * _inputMultiplier * Time.deltaTime;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                OnLeftMove(moveValue);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                OnRightMove(moveValue);
                
            }
        }
    }
}
