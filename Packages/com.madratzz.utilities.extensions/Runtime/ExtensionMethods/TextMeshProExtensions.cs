using TMPro;

namespace ExtensionMethods
{
    public static class TextMeshProExtensions
    {
        public static void SetOpacity(this TextMeshProUGUI textMesh, float alpha)
        {
            if (textMesh == null) return;
            var currentColor = textMesh.color;
            currentColor.a = alpha;
            textMesh.color = currentColor;
        }
    }
}
