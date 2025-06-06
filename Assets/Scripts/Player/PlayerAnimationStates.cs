using System;
using UnityEngine;

namespace Player
{
    public class PlayerAnimationStates : MonoBehaviour
    {
        public enum States
        {
            Start,
            Idle,
            Walk,
            Dash,
            Jump,
            Fall,
            Land
        }

        public static string ConvertToString(States state)
        {
            return Enum.GetName(typeof(States), state);
        }
    }
}