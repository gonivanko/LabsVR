#if TEXT_MESH_PRO_PRESENT || (UGUI_2_0_PRESENT && UNITY_6000_0_OR_NEWER)
using TMPro;
using UnityEngine.Events;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.SpatialKeyboard
{
    /// <summary>
    ///     Utility class to help facilitate input field relationship with <see cref="XRKeyboard" />
    /// </summary>
    public class XRKeyboardDisplay : MonoBehaviour
    {
        [SerializeField] [Tooltip("Input field linked to this display.")]
        private TMP_InputField m_InputField;

        // The script requires setter property logic to be run, so disable when playing
        [SerializeField]
        [Tooltip(
            "Keyboard for this display to monitor and interact with. If empty this will default to the GlobalNonNativeKeyboard keyboard.")]
        private XRKeyboard m_Keyboard;

        [SerializeField]
        [Tooltip(
            "If true, this display will use the keyboard reference. If false or if the keyboard field is empty, this display will use global keyboard.")]
        private bool m_UseSceneKeyboard;

        [SerializeField]
        [Tooltip(
            "If true, this display will update with each key press. If false, this display will update on OnTextSubmit.")]
        private bool m_UpdateOnKeyPress = true;

        [SerializeField]
        [Tooltip(
            "If true, this display will always subscribe to the keyboard updates. If false, this display will subscribe to keyboard when the input field gains focus.")]
        private bool m_AlwaysObserveKeyboard;

        [SerializeField]
        [Tooltip(
            "If true, this display will use the input field's character limit to limit the update text from the keyboard and will pass this into the keyboard when opening.")]
        public bool m_MonitorInputFieldCharacterLimit;

        [SerializeField]
        [Tooltip("If true, this display will clear the input field text on text submit from the keyboard.")]
        public bool m_ClearTextOnSubmit;

        [SerializeField] [Tooltip("If true, this display will clear the input field text when the keyboard opens.")]
        public bool m_ClearTextOnOpen;

        [SerializeField]
        [Tooltip("If true, this display will close the keyboard it is observing when this GameObject is disabled.")]
        public bool m_HideKeyboardOnDisable = true;

        [SerializeField]
        [Tooltip(
            "The event that is called when this display receives a text submitted event from the keyboard. Invoked with the keyboard text as a parameter.")]
        private UnityEvent<string> m_OnTextSubmitted = new();

        [SerializeField] [Tooltip("The event that is called when this display opens a keyboard.")]
        private UnityEvent m_OnKeyboardOpened = new();

        [SerializeField] [Tooltip("The event that is called when the keyboard this display is observing is closed.")]
        private UnityEvent m_OnKeyboardClosed = new();

        [SerializeField]
        [Tooltip("The event that is called when the keyboard changes focus and this display is not focused.")]
        private UnityEvent m_OnKeyboardFocusChanged = new();

        // Active keyboard for this display
        private XRKeyboard m_ActiveKeyboard;

        private bool m_IsActivelyObservingKeyboard;

        /// <summary>
        ///     Input field linked to this display.
        /// </summary>
        public TMP_InputField inputField
        {
            get => m_InputField;
            set
            {
                if (inputField != null)
                    m_InputField.onSelect.RemoveListener(OnInputFieldGainedFocus);

                m_InputField = value;

                if (inputField != null)
                {
                    m_InputField.resetOnDeActivation = false;
                    m_InputField.onSelect.AddListener(OnInputFieldGainedFocus);
                }
            }
        }

        /// <summary>
        ///     Keyboard for this display to monitor and interact with. If empty this will default to the
        ///     <see cref="GlobalNonNativeKeyboard" /> keyboard.
        /// </summary>
        public XRKeyboard keyboard
        {
            get => m_Keyboard;
            set => SetKeyboard(value);
        }

        /// <summary>
        ///     If true, this display will use the keyboard reference. If false or if the keyboard field is empty,
        ///     this display will use global keyboard.
        /// </summary>
        public bool useSceneKeyboard
        {
            get => m_UseSceneKeyboard;
            set => m_UseSceneKeyboard = value;
        }

        /// <summary>
        ///     If true, this display will update with each key press. If false, this display will update on OnTextSubmit.
        /// </summary>
        public bool updateOnKeyPress
        {
            get => m_UpdateOnKeyPress;
            set => m_UpdateOnKeyPress = value;
        }

        /// <summary>
        ///     If true, this display will always subscribe to the keyboard updates. If false, this display will subscribe
        ///     to keyboard when the input field gains focus.
        /// </summary>
        public bool alwaysObserveKeyboard
        {
            get => m_AlwaysObserveKeyboard;
            set => m_AlwaysObserveKeyboard = value;
        }

        /// <summary>
        ///     If true, this display will use the input field's character limit to limit the update text from the keyboard
        ///     and will pass this into the keyboard when opening if.
        /// </summary>
        public bool monitorInputFieldCharacterLimit
        {
            get => m_MonitorInputFieldCharacterLimit;
            set => m_MonitorInputFieldCharacterLimit = value;
        }

        /// <summary>
        ///     If true, this display will clear the input field text on text submit from the keyboard.
        /// </summary>
        public bool clearTextOnSubmit
        {
            get => m_ClearTextOnSubmit;
            set => m_ClearTextOnSubmit = value;
        }

        /// <summary>
        ///     If true, this display will clear the input field text on text submit from the keyboard.
        /// </summary>
        public bool clearTextOnOpen
        {
            get => m_ClearTextOnOpen;
            set => m_ClearTextOnOpen = value;
        }

        /// <summary>
        ///     If true, this display will close the keyboard it is observing when this GameObject is disabled.
        /// </summary>
        /// <remarks>If this display is not observing a keyboard when disabled, this will have not effect on open keyboards.</remarks>
        public bool hideKeyboardOnDisable
        {
            get => m_HideKeyboardOnDisable;
            set => m_HideKeyboardOnDisable = value;
        }

        /// <summary>
        ///     The event that is called when this display receives a text submitted event from the keyboard.
        /// </summary>
        public UnityEvent<string> onTextSubmitted
        {
            get => m_OnTextSubmitted;
            set => m_OnTextSubmitted = value;
        }

        /// <summary>
        ///     The event that is called when this display opens a keyboard.
        /// </summary>
        public UnityEvent onKeyboardOpened
        {
            get => m_OnKeyboardOpened;
            set => m_OnKeyboardOpened = value;
        }

        /// <summary>
        ///     The event that is called when the keyboard this display is observing is closed.
        /// </summary>
        public UnityEvent onKeyboardClosed
        {
            get => m_OnKeyboardClosed;
            set => m_OnKeyboardClosed = value;
        }

        /// <summary>
        ///     The event that is called when the keyboard changes focus and this display is not focused.
        /// </summary>
        public UnityEvent onKeyboardFocusChanged
        {
            get => m_OnKeyboardFocusChanged;
            set => m_OnKeyboardFocusChanged = value;
        }

        /// <summary>
        ///     See <see cref="MonoBehaviour" />.
        /// </summary>
        private void Awake()
        {
            // Set active keyboard to any serialized keyboard
            m_ActiveKeyboard = m_Keyboard;

            if (m_InputField != null)
            {
                // resetOnDeActivation should be false so the caret position does not break with the keyboard interaction
                m_InputField.resetOnDeActivation = false;

                // shouldHideSoftKeyboard should be true so there is no conflict with the spatial keyboard and the system keyboard
                m_InputField.shouldHideSoftKeyboard = true;
            }

            if (m_AlwaysObserveKeyboard && m_ActiveKeyboard != null)
                StartObservingKeyboard(m_ActiveKeyboard);
        }

        /// <summary>
        ///     See <see cref="MonoBehaviour" />.
        /// </summary>
        private void Start()
        {
            // Set active keyboard to global keyboard if needed
            if (m_ActiveKeyboard == null || !m_UseSceneKeyboard)
                m_ActiveKeyboard = GlobalNonNativeKeyboard.instance.keyboard;

            // Observe keyboard if always observe is true
            var observeOnStart = m_AlwaysObserveKeyboard && (m_ActiveKeyboard != null) & !m_IsActivelyObservingKeyboard;
            if (observeOnStart)
                StartObservingKeyboard(m_ActiveKeyboard);
        }

        /// <summary>
        ///     See <see cref="MonoBehaviour" />.
        /// </summary>
        private void OnEnable()
        {
            if (m_InputField != null)
                m_InputField.onSelect.AddListener(OnInputFieldGainedFocus);
        }

        /// <summary>
        ///     See <see cref="MonoBehaviour" />.
        /// </summary>
        private void OnDisable()
        {
            if (m_InputField != null)
                m_InputField.onSelect.RemoveListener(OnInputFieldGainedFocus);

            // Close the keyboard this display is observing
            var isObservingKeyboard = m_ActiveKeyboard != null && m_ActiveKeyboard.gameObject.activeInHierarchy &&
                                      m_IsActivelyObservingKeyboard;
            if (m_HideKeyboardOnDisable && isObservingKeyboard && m_ActiveKeyboard.isOpen)
                m_ActiveKeyboard.Close();
        }

        /// <summary>
        ///     See <see cref="MonoBehaviour" />.
        /// </summary>
        private void OnDestroy()
        {
            StopObservingKeyboard(m_ActiveKeyboard);
        }

        private void SetKeyboard(XRKeyboard updateKeyboard, bool observeKeyboard = true)
        {
            if (ReferenceEquals(updateKeyboard, m_Keyboard))
                return;

            StopObservingKeyboard(m_ActiveKeyboard);

            // Update serialized referenced
            m_Keyboard = updateKeyboard;

            // Update private keyboard
            m_ActiveKeyboard = m_Keyboard;

            if (m_ActiveKeyboard != null && (observeKeyboard || m_AlwaysObserveKeyboard))
                StartObservingKeyboard(m_ActiveKeyboard);
        }

        private void StartObservingKeyboard(XRKeyboard activeKeyboard)
        {
            if (activeKeyboard == null || m_IsActivelyObservingKeyboard)
                return;

            activeKeyboard.onTextUpdated.AddListener(OnTextUpdate);
            activeKeyboard.onTextSubmitted.AddListener(OnTextSubmit);
            activeKeyboard.onClosed.AddListener(KeyboardClosing);
            activeKeyboard.onOpened.AddListener(KeyboardOpening);
            activeKeyboard.onFocusChanged.AddListener(KeyboardFocusChanged);

            m_IsActivelyObservingKeyboard = true;
        }

        private void StopObservingKeyboard(XRKeyboard activeKeyboard)
        {
            if (activeKeyboard == null)
                return;

            activeKeyboard.onTextUpdated.RemoveListener(OnTextUpdate);
            activeKeyboard.onTextSubmitted.RemoveListener(OnTextSubmit);
            activeKeyboard.onClosed.RemoveListener(KeyboardClosing);
            activeKeyboard.onOpened.RemoveListener(KeyboardOpening);
            activeKeyboard.onFocusChanged.RemoveListener(KeyboardFocusChanged);

            m_IsActivelyObservingKeyboard = false;
        }

        private void OnInputFieldGainedFocus(string text)
        {
            // If this display is already observing keyboard, sync, attempt to reposition, and early out
            // Displays that are always observing keyboards call open to ensure they sync with the keyboard
            if (m_IsActivelyObservingKeyboard && !alwaysObserveKeyboard)
            {
                if (!m_UseSceneKeyboard || m_Keyboard == null)
                    GlobalNonNativeKeyboard.instance.RepositionKeyboardIfOutOfView();

                // Sync input field caret position with keyboard caret position
                if (m_InputField.stringPosition != m_ActiveKeyboard.caretPosition)
                    m_InputField.stringPosition = m_ActiveKeyboard.caretPosition;

                return;
            }

            if (m_ClearTextOnOpen)
                m_InputField.text = string.Empty;

            // If not using a scene keyboard, use global keyboard.
            if (!m_UseSceneKeyboard || m_Keyboard == null)
                GlobalNonNativeKeyboard.instance.ShowKeyboard(m_InputField, m_MonitorInputFieldCharacterLimit);
            else
                m_ActiveKeyboard.Open(m_InputField, m_MonitorInputFieldCharacterLimit);

            // Sync input field caret position with keyboard caret position
            if (m_InputField.stringPosition != m_ActiveKeyboard.caretPosition)
                m_InputField.stringPosition = m_ActiveKeyboard.caretPosition;

            // This display is opening the keyboard
            m_OnKeyboardOpened.Invoke();

            StartObservingKeyboard(m_ActiveKeyboard);
        }

        private void OnTextSubmit(KeyboardTextEventArgs args)
        {
            UpdateText(args.keyboardText);
            m_OnTextSubmitted?.Invoke(args.keyboardText);
            if (m_ClearTextOnSubmit) inputField.text = string.Empty;
        }

        private void OnTextUpdate(KeyboardTextEventArgs args)
        {
            if (!m_UpdateOnKeyPress)
                return;

            UpdateText(args.keyboardText);
        }

        private void UpdateText(string text)
        {
            var updatedText = text;

            // Clip updated text to substring
            if (m_MonitorInputFieldCharacterLimit && updatedText.Length >= m_InputField.characterLimit)
                updatedText = updatedText.Substring(0, m_InputField.characterLimit);

            m_InputField.text = updatedText;

            // Update input field caret position with keyboard caret position
            if (m_InputField.stringPosition != m_ActiveKeyboard.caretPosition)
                m_InputField.stringPosition = m_ActiveKeyboard.caretPosition;
        }

        private void KeyboardOpening(KeyboardTextEventArgs args)
        {
            Debug.Assert(args.keyboard == m_ActiveKeyboard);

            if (args.keyboard != m_ActiveKeyboard)
                return;

            if (!m_InputField.isFocused && !m_AlwaysObserveKeyboard)
                StopObservingKeyboard(m_ActiveKeyboard);
        }

        private void KeyboardClosing(KeyboardTextEventArgs args)
        {
            Debug.Assert(args.keyboard == m_ActiveKeyboard);

            if (args.keyboard != m_ActiveKeyboard)
                return;

            if (!m_AlwaysObserveKeyboard)
                StopObservingKeyboard(m_ActiveKeyboard);

            m_OnKeyboardClosed.Invoke();
        }

        private void KeyboardFocusChanged(KeyboardTextEventArgs args)
        {
            Debug.Assert(args.keyboard == m_ActiveKeyboard);

            if (args.keyboard != m_ActiveKeyboard)
                return;

            if (!m_InputField.isFocused && !m_AlwaysObserveKeyboard)
                StopObservingKeyboard(m_ActiveKeyboard);

            // The keyboard changed focus and this input field is no longer in focus
            if (!m_InputField.isFocused)
                m_OnKeyboardFocusChanged.Invoke();
        }
    }
}
#endif