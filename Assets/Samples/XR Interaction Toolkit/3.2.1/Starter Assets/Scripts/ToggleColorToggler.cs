using UnityEngine.UI;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets
{
    /// <summary>
    ///     Updates the normal color of a toggle based on the state of the toggle.
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public class ToggleColorToggler : MonoBehaviour
    {
        [SerializeField] [Tooltip("Normal color for the toggle in the on state.")]
        private Color m_OnColor = new(32 / 255f, 150 / 255f, 243 / 255f);

        [SerializeField] [Tooltip("Normal color for the toggle in the off state.")]
        private Color m_OffColor = new(46 / 255f, 46 / 255f, 46 / 255f);

        private Toggle m_TargetToggle;

        /// <summary>
        ///     Normal color for the toggle in the on state.
        /// </summary>
        public Color onColor
        {
            get => m_OnColor;
            set => m_OnColor = value;
        }

        /// <summary>
        ///     Normal color for the toggle in the off state.
        /// </summary>
        public Color offColor
        {
            get => m_OffColor;
            set => m_OffColor = value;
        }

        /// <summary>
        ///     See <see cref="MonoBehaviour" />.
        /// </summary>
        private void Awake()
        {
            m_TargetToggle = GetComponent<Toggle>();
        }

        /// <summary>
        ///     See <see cref="MonoBehaviour" />.
        /// </summary>
        private void OnEnable()
        {
            m_TargetToggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        /// <summary>
        ///     See <see cref="MonoBehaviour" />.
        /// </summary>
        private void OnDisable()
        {
            m_TargetToggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool isOn)
        {
            var toggleColors = m_TargetToggle.colors;
            toggleColors.normalColor = isOn ? m_OnColor : m_OffColor;
            m_TargetToggle.colors = toggleColors;
        }
    }
}