using System.Text.RegularExpressions;
using GameEngine.Core;
using GameEngine.Mathematics;
using GameEngine.Utilities;
using Silk.NET.OpenGL;

namespace GameEngine.Rendering;

public class ShaderProgram
{
    private GL _gl = Engine.GL;

    private uint _program;

    // O construtor gera o shader dinamicamente.
    // --------------------------------------------------

    public ShaderProgram(string path)
    {
        string vertexPath = $"res/Shaders/{path}/vertex.glsl";
        string fragmentPath = $"res/Shaders/{path}/fragment.glsl";

        SetupShader(vertexPath, fragmentPath);
    }

    public ShaderProgram(string vertexPath, string fragmentPath)
    {
        SetupShader(vertexPath, fragmentPath);
    }

    // ativar o shader
    // --------------------------------------------------

    public void Use()
    {
        _gl.UseProgram(_program);
    }

    // funções uniformes de utilidade
    // --------------------------------------------------
    
    public void SetUniform(string name, bool value)
    {
        int location = _gl.GetUniformLocation(_program, name);
        _gl.Uniform1(location, value ? 1 : 0);
    }

    public void SetUniform(string name, float value)
    {
        int location = _gl.GetUniformLocation(_program, name);
        _gl.Uniform1(location, value);
    }

    public void SetUniform(string name, int value)
    {
        int location = _gl.GetUniformLocation(_program, name);
        _gl.Uniform1(location, value);
    }

    // Set Uniform2
    // --------------------------------------------------

    public void SetUniform(string name, float x, float y)
    {
        int location = _gl.GetUniformLocation(_program, name);
        _gl.Uniform2(location, x, y);
    }

    public void SetUniform(string name, Vector2 vector)
    {
        int location = _gl.GetUniformLocation(_program, name);
        _gl.Uniform2(location, vector);
    }

    public void SetUniform(string name, int x, int y)
    {
        int location = _gl.GetUniformLocation(_program, name);
        _gl.Uniform2(location, x, y);
    }

    public void SetUniform(string name, Vector2Int vector)
    {
        int location = _gl.GetUniformLocation(_program, name);
        _gl.Uniform2(location, vector);
    }

    // Set Uniform3
    // --------------------------------------------------

    public void SetUniform(string name, float x, float y, float z)
    {
        int location = _gl.GetUniformLocation(_program, name);
        _gl.Uniform3(location, x, y, z);
    }

    public void SetUniform(string name, Vector3 vector)
    {
        int location = _gl.GetUniformLocation(_program, name);
        _gl.Uniform3(location, vector);
    }

    public void SetUniform(string name, int x, int y, int z)
    {
        int location = _gl.GetUniformLocation(_program, name);
        _gl.Uniform3(location, x, y, z);
    }

    public void SetUniform(string name, Vector3Int vector)
    {
        int location = _gl.GetUniformLocation(_program, name);
        _gl.Uniform3(location, vector);
    }

    // Set Uniform4
    // --------------------------------------------------

    public void SetUniform(string name, float x, float y, float z, float w)
    {
        int location = _gl.GetUniformLocation(_program, name);
        _gl.Uniform4(location, x, y, z, w);
    }

    public void SetUniform(string name, Vector4 vector)
    {
        int location = _gl.GetUniformLocation(_program, name);
        _gl.Uniform4(location, vector);
    }

    public void SetUniform(string name, int x, int y, int z, int w)
    {
        int location = _gl.GetUniformLocation(_program, name);
        _gl.Uniform4(location, x, y, z, w);
    }

    public void SetUniform(string name, Vector4Int vector)
    {
        int location = _gl.GetUniformLocation(_program, name);
        _gl.Uniform4(location, vector);
    }

    // Set UniformMatrix4
    // --------------------------------------------------

    public void SetUniform(string name, Matrix4x4 matrix)
    {
        int location = _gl.GetUniformLocation(_program, name);
        unsafe
        {
            _gl.UniformMatrix4(location, 1, false, (float*)&matrix);
        }
    }

    // 
    // --------------------------------------------------

    private void SetupShader(string vertexPath, string fragmentPath)
    {
        string vShaderCode = string.Empty;
        string fShaderCode = string.Empty;

        try
        {
            vShaderCode = LoadShaderSource(vertexPath);
            fShaderCode = LoadShaderSource(fragmentPath);
        }
        catch (Exception ex)
        {
            Debug.LogError(
                "ERROR::SHADER::FILE_NOT_SUCCESSFULLY_READ: "
                + "\n\n" + ex
                + "\n\n" + " -- --------------------------------------------------- -- "
            );
        }

        uint vertex, fragment;

        // vertex shader
        vertex = _gl.CreateShader(ShaderType.VertexShader);
        _gl.ShaderSource(vertex, vShaderCode);
        _gl.CompileShader(vertex);
        CheckCompileErrors(vertex, "VERTEX");

        // fragment Shader
        fragment = _gl.CreateShader(ShaderType.FragmentShader);
        _gl.ShaderSource(fragment, fShaderCode);
        _gl.CompileShader(fragment);
        CheckCompileErrors(fragment, "FRAGMENT");

        // shader Program
        _program = _gl.CreateProgram();
        _gl.AttachShader(_program, vertex);
        _gl.AttachShader(_program, fragment);
        _gl.LinkProgram(_program);
        CheckCompileErrors(_program, "PROGRAM");

        // Exclua os shaders, pois agora estão integrados ao nosso programa e não são mais necessários.
        _gl.DeleteShader(vertex);
        _gl.DeleteShader(fragment);
    }

    // Remove comentarios inteiros na hora de carregar os shaders, pois estava dando erro nos shaders quando fazia comentarios com acentuação/caracteres especiais
    // ´ ` ^ ~ ç
    // --------------------------------------------------

    private string LoadShaderSource(string path)
    {
        string content = File.ReadAllText(path);

        // Remove comentários de linha // (qualquer lugar, mas cuidado com strings)
        content = Regex.Replace(content, @"//.*", "");

        // Remove comentários de bloco /* ... */
        content = Regex.Replace(content, @"/\*.*?\*/", "", RegexOptions.Singleline);

        return content;
    }

    // Função utilitária para verificar erros de compilação/vinculação de shaders.
    // --------------------------------------------------

    private void CheckCompileErrors(uint shader, string type)
    {
        int success;
        string infoLog;

        if (type != "PROGRAM")
        {
            _gl.GetShader(shader, ShaderParameterName.CompileStatus, out success);
            if (success == 0)
            {
                infoLog = _gl.GetShaderInfoLog(shader);
                Debug.LogError(
                    "ERROR::SHADER_COMPILATION_ERROR of type: " + type
                    + "\n\n" + infoLog
                    + "\n" + " -- --------------------------------------------------- -- "
                );
            }
        }
        else
        {
            _gl.GetProgram(shader, ProgramPropertyARB.LinkStatus, out success);
            if (success == 0)
            {
                infoLog = _gl.GetProgramInfoLog(shader);
                Debug.LogError(
                    "ERROR::PROGRAM_LINKING_ERROR of type: " + type
                    + "\n\n" + infoLog
                    + "\n" + " -- --------------------------------------------------- -- "
                );
            }
        }
    }
}
