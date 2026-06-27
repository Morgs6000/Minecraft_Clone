namespace GameEngine.Interfaces;

public class Text : RectTransform
{
    // Text
    // --------------------------------------------------

    public string Txt = "";

    // Character
    // --------------------------------------------------

    // Font

    // Font Style
        // Normal
        // Bold
        // Italic
        // Bold and Italic

    // Font Size
        // padrão do Text Leagace 14
        // padrão do TextMeshPro 36
        // padrão do Word 12

    // Line Spacing

    // Paragraph
    // --------------------------------------------------
    
    // Alignment
        // Left
        // Center
        // Right
        // Justified
        
        // Top
        // Middle
        // Bottom

    // Color
    // --------------------------------------------------

    public Text()
    {
        Width = 200;
        Height = 50;
    }
}
