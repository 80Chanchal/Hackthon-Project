using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class WebXRBuildSettings : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        if (report.summary.platform == BuildTarget.WebGL)
        {
            SetupWebGLSettings();
        }
    }

    [MenuItem("WebXR/Configure WebGL Settings")]
    public static void SetupWebGLSettings()
    {
        // Enable WebGL 2.0 
        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.WebGL, false);
        PlayerSettings.SetGraphicsAPIs(BuildTarget.WebGL, new[] { UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3 });
        
        // Enable WebXR template
        PlayerSettings.WebGL.template = "PROJECT:WebXR";
        
        // Enable threading
        PlayerSettings.WebGL.threadsSupport = true;
        
        // Set memory settings
        PlayerSettings.WebGL.memorySize = 512;
        
        // Set WebGL 2.0 as the minimum requirement
        PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.None;
        
        // Set compression settings
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
        
        // Enable WebAssembly
        EditorUserBuildSettings.webGLBuildSubtarget = WebGLTextureSubtarget.DXT;
        
        // Set WebGL linker target
        PlayerSettings.WebGL.linkerTarget = WebGLLinkerTarget.Wasm;
        
        // Enable WebGL decompression fallback
        PlayerSettings.WebGL.decompressionFallback = true;
        
        Debug.Log("WebGL settings configured for WebXR");
    }
} 