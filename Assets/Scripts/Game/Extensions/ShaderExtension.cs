
using Cysharp.Threading.Tasks;
using UnityEngine;

public  static class ShaderExtension
{
    public static async UniTask DissolveAsync(this Renderer renderer, float duration)
    {
        var _dissolve = Shader.Find("Custom/DissolveSurface");

        var dissolveProgress = 0f;

        if (_dissolve != null)
        {
            renderer.material.shader = _dissolve;
            Material sharedMaterial = renderer.sharedMaterial;

            await UniTask.WaitWhile(() =>
            {
                dissolveProgress += Time.fixedDeltaTime / duration;
                sharedMaterial.SetFloat("_Amount", dissolveProgress);
                Debug.Log(dissolveProgress);

                return dissolveProgress < 1f;
            });
        }
    }
}