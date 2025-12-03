#if TEXT_MESH_PRO_PRESENT || (UGUI_2_0_PRESENT && UNITY_6000_0_OR_NEWER)
using System;
using System.Collections.Generic;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.SpatialKeyboard
{
    /// <summary>
    ///     Scriptable object that defines key mappings to support swapping <see cref="XRKeyboardLayout" />. There should be
    ///     one
    ///     instance of the <see cref="XRKeyboardConfig" /> for each layout (i.e. alphanumeric, symbols, etc.).
    /// </summary>
    public class XRKeyboardConfig : ScriptableObject
    {
        [SerializeField] [Tooltip("Default key function for each key in this mapping.")]
        private KeyFunction m_DefaultKeyFunction;

        /// <summary>
        ///     List of each key mapping in this layout.
        /// </summary>
        [SerializeField] [Tooltip("List of each key mapping in this layout.")]
        private List<KeyMapping> m_KeyMappings;

        /// <summary>
        ///     Default key function for each key in this mapping.
        /// </summary>
        /// <remarks>
        ///     This is a utility feature that reduces the authoring needed when most key mappings share the same
        ///     functionality (i.e. value keys that append characters).
        /// </remarks>
        public KeyFunction defaultKeyFunction
        {
            get => m_DefaultKeyFunction;
            set => m_DefaultKeyFunction = value;
        }

        /// <summary>
        ///     List of each key mapping in this layout.
        /// </summary>
        public List<KeyMapping> keyMappings
        {
            get => m_KeyMappings;
            set => m_KeyMappings = value;
        }

        /// <summary>
        ///     Class representing the data needed to populate keys.
        /// </summary>
        [Serializable]
        public class KeyMapping
        {
            [SerializeField] private string m_Character;

            [SerializeField] private string m_DisplayCharacter;

            [SerializeField] private Sprite m_DisplayIcon;

            [SerializeField] private string m_ShiftCharacter;

            [SerializeField] private string m_ShiftDisplayCharacter;

            [SerializeField] private Sprite m_ShiftDisplayIcon;

            [SerializeField] private bool m_OverrideDefaultKeyFunction;

            [SerializeField] private KeyFunction m_KeyFunction;

            [SerializeField] private KeyCode m_KeyCode;

            [SerializeField] private bool m_Disabled;

            /// <summary>
            ///     Character for this key in non-shifted state. This string will be passed to the keyboard and appended to the
            ///     keyboard text string or processed as a keyboard command.
            /// </summary>
            public string character
            {
                get => m_Character;
                set => m_Character = value;
            }

            /// <summary>
            ///     Display character for this key in a non-shifted state. This string will be displayed on the key text field.
            ///     If empty, character will be used as a fallback.
            /// </summary>
            public string displayCharacter
            {
                get => m_DisplayCharacter;
                set => m_DisplayCharacter = value;
            }

            /// <summary>
            ///     Display icon for this key in a non-shifted state. This icon will be displayed on the key image field.
            ///     If empty, the display character or character will be used as a fallback.
            /// </summary>
            public Sprite displayIcon
            {
                get => m_DisplayIcon;
                set => m_DisplayIcon = value;
            }

            /// <summary>
            ///     Character for this key in a shifted state. This string will be passed to the keyboard and appended to
            ///     the keyboard text string or processed as a keyboard command.
            /// </summary>
            public string shiftCharacter
            {
                get => m_ShiftCharacter;
                set => m_ShiftCharacter = value;
            }

            /// <summary>
            ///     Display character for this key in a shifted state. This string will be displayed on the key
            ///     text field. If empty, shift character will be used as a fallback.
            /// </summary>
            public string shiftDisplayCharacter
            {
                get => m_ShiftDisplayCharacter;
                set => m_ShiftDisplayCharacter = value;
            }

            /// <summary>
            ///     Display icon for this key in a shifted state. This icon will be displayed on the key image field.
            ///     If empty, the shift display character or shift character will be used as a fallback.
            /// </summary>
            public Sprite shiftDisplayIcon
            {
                get => m_ShiftDisplayIcon;
                set => m_ShiftDisplayIcon = value;
            }

            /// <summary>
            ///     If true, this will expose a key function property to override the default key function of this config.
            /// </summary>
            public bool overrideDefaultKeyFunction
            {
                get => m_OverrideDefaultKeyFunction;
                set => m_OverrideDefaultKeyFunction = value;
            }

            /// <summary>
            ///     <see cref="KeyFunction" /> used for this key. The function callback will be called on key press
            ///     and used to communicate with the keyboard API.
            /// </summary>
            public KeyFunction keyFunction
            {
                get => m_KeyFunction;
                set => m_KeyFunction = value;
            }

            /// <summary>
            ///     (Optional) <see cref="KeyCode" /> used for this key. Used with <see cref="keyFunction" /> to
            ///     support already defined KeyCode values.
            /// </summary>
            public KeyCode keyCode
            {
                get => m_KeyCode;
                set => m_KeyCode = value;
            }

            /// <summary>
            ///     If true, the key button interactable property will be set to false.
            /// </summary>
            public bool disabled
            {
                get => m_Disabled;
                set => m_Disabled = value;
            }
        }
    }
}
#endif