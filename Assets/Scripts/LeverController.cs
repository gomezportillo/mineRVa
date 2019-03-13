namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEngine.UI;
    using VRTK.Controllables;

    public class LeverController : MonoBehaviour
    {
        public enum OptionType
        {
            InteractWithObjects,
            GrabToPointerTip
        }

        public OptionType optionType = OptionType.InteractWithObjects;
        public VRTK_Pointer[] pointers = new VRTK_Pointer[0];
        public VRTK_BaseControllable controllable;
        public Text displayText;
        public string maxText;
        public string minText;

        public Transform door_left;
        public Transform door_right;

        private Vector3 left_open_position = new Vector3(1, 0, -9);
        private Vector3 right_open_position = new Vector3(-1, 0, -9);

        public float open_speed;

        private bool door_opened = false;

        protected virtual void OnEnable()
        {
            controllable = (controllable == null ? GetComponent<VRTK_BaseControllable>() : controllable);
            if (controllable != null)
            {
                controllable.MaxLimitReached += MaxLimitReached;
                controllable.MinLimitReached += MinLimitReached;
            }
        }

        protected virtual void OnDisable()
        {
            if (controllable != null)
            {
                controllable.MaxLimitReached -= MaxLimitReached;
                controllable.MinLimitReached -= MinLimitReached;
            }
        }

        protected virtual void MaxLimitReached(object sender, ControllableEventArgs e)
        {
            SetOption(true, maxText);
            door_opened = true;
        }

        protected virtual void MinLimitReached(object sender, ControllableEventArgs e)
        {
            SetOption(false, minText);
        }

        protected virtual void SetOption(bool value, string text)
        {
            if (displayText != null)
            {
                displayText.text = text;

            }

            foreach (VRTK_Pointer pointer in pointers)
            {
                pointer.enabled = false;
                pointer.pointerRenderer.enabled = false;
                switch (optionType)
                {
                    case OptionType.InteractWithObjects:
                        pointer.interactWithObjects = value;
                        break;
                    case OptionType.GrabToPointerTip:
                        pointer.grabToPointerTip = value;
                        break;
                }
                pointer.pointerRenderer.enabled = true;
                pointer.enabled = true;
            }
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                door_opened = true;
            }

            if (door_opened)
            {
                door_left.position = Vector3.Lerp(door_left.position,
                                                  left_open_position,
                                                  Time.deltaTime * open_speed);

                door_right.position = Vector3.Lerp(door_right.position,
                                                   right_open_position,
                                                   Time.deltaTime * open_speed);
            }

        }
    }
}