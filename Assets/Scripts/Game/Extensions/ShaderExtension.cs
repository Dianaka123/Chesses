
using Cysharp.Threading.Tasks;
using UnityEngine;

public  static class ShaderExtension
{
    private const string DISSOLVE_SURFACE = "Custom/DissolveSurface";
    private const string STANDART = "Standard";

    public static async UniTask DissolveAsync(this Renderer renderer, float duration)
    {
        var _dissolve = Shader.Find(DISSOLVE_SURFACE);

        var dissolveProgress = 0f;

        if (_dissolve != null)
        {
            renderer.material.shader = _dissolve;
            Material sharedMaterial = renderer.sharedMaterial;

            await UniTask.WaitWhile(() =>
            {
                dissolveProgress += Time.fixedDeltaTime / duration;
                sharedMaterial.SetFloat("_Amount", dissolveProgress);

                return dissolveProgress < 1f;
            });
        }
    }

    public static void ResetShaderToStandart(this Renderer renderer)
    {
        if(renderer.material.shader.name == STANDART)
        {
            return;
        }

        var standart = Shader.Find(STANDART);
        renderer.material.shader = standart;
    }
}