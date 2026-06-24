#version 330 core
out vec4 FragColor;

uniform vec4 uColor = vec4(1.0f, 1.0f, 1.0f, 1.0f);

uniform bool hasWireframe = false;

uniform bool hasColor = false;
in vec4 ourColor; 

uniform bool hasTexture = false;
in vec2 TexCoord;
uniform sampler2D ourTexture;

void main()
{
    vec4 result = uColor;
    
    // wireframe
    // --------------------------------------------------

    if (hasWireframe)
    {
        result = vec4(0.0f, 0.0f, 0.0f, 1.0f);
    }

    // color
    // --------------------------------------------------

    if (hasColor) 
    {
        result *= ourColor;
    }

    // texCoord
    // --------------------------------------------------

    if (hasTexture) 
    {
        result *= texture(ourTexture, TexCoord);

        /*
        vec4 texColor = texture(ourTexture, TexCoord);
        if(texColor.a < 0.1f)
        {
            discard;
        }

        result *= texColor;
        */
    }

    // FragColor
    // --------------------------------------------------

    FragColor = result;
}
