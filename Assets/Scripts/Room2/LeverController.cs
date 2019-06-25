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
        public string openText;
        public string closedText;

        public GameObject TextDescriptorOpen;
        public GameObject TextDescriptorClosed;

        public Transform doorLeft;
        public Transform doorRight;

        private Vector3 leftOpenPosition = new Vector3(1, 0, -9);
        private Vector3 rightOpenPosition = new Vector3(-1, 0, -9);

        private Vector3 leftClosePosition = new Vector3(0, 0, -9);
        private Vector3 rightClosePosition = new Vector3(0, 0, -9);

        public float openSpeed;

        private bool doorOpen = false;

        protected virtual void OnEnable()
        {
            controllable = (controllable == null ? GetComponent<VRTK_BaseControllable>() : controllable);
            if (controllable != null)
            {
                controllable.MaxLimitReached += MaxLimitReached;
                controllable.MinLimitReached += MinLimitReached;
            }

            if (TextDescriptorOpen && TextDescriptorClosed)
            {
                TextDescriptorClosed.SetActive(true);
                TextDescriptorOpen.SetActive(false);
                ToggleTextDescriptors();
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
            SetOption(true, openText);
            doorOpen = true;
            ToggleTextDescriptors();
        }

        protected virtual void MinLimitReached(object sender, ControllableEventArgs e)
        {
            SetOption(false, closedText);
            doorOpen = false;
            ToggleTextDescriptors();
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
                doorOpen = !doorOpen;

                ToggleTextDescriptors();
            }

            Vector3 leftPosition;
            Vector3 rightPosition;

            if (doorOpen)
            {
                leftPosition = leftOpenPosition;
                rightPosition = rightOpenPosition;
            }
            else
            {
                leftPosition = leftClosePosition;
                rightPosition = rightClosePosition;
            }

            doorLeft.position = Vector3.Lerp(doorLeft.position,
                                              leftPosition,
                                              Time.deltaTime * openSpeed);

            doorRight.position = Vector3.Lerp(doorRight.position,
                                               rightPosition,
                                               Time.deltaTime * openSpeed);

        }

        private void ToggleTextDescriptors()
        {
            if (TextDescriptorOpen && TextDescriptorClosed)
            {
                TextDescriptorClosed.SetActive(!TextDescriptorClosed.activeSelf);
                TextDescriptorOpen.SetActive(!TextDescriptorOpen.activeSelf);
            }
        }
    }
}