using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[ExecuteInEditMode]
public class AnimatedProjector : MonoBehaviour 
{
    public bool runEditor = true;
    public float fps = 30.0f;
    public bool reload;
    public string TextureToReload;
    public Texture2D[] frames;    
	
    private int frameIndex;
    private Light projector;
    
//Used for loading the textures all at once using the resource folder, saves a lot of time not having to manually assign the textures.
#if UNITY_EDITOR
    void Update()
    {
        if (!Application.isPlaying)
        {
            if (runEditor)
            {
                NextFrame();
            }
            if (reload == true)
            {
                projector.cookie = Resources.Load(TextureToReload + "0") as Texture2D;
                for (int i = 0; i < frames.Length; i++)
                {
                    frames[i] = Resources.Load(TextureToReload + (i)) as Texture2D;
                }
            }
            reload = false;
        }
 
    }
#endif
 
    //Starts the actuall function
    void Awake()
	{
            projector = GetComponent<Light>();
            NextFrame();
            InvokeRepeating("NextFrame", 1 / fps, 1 / fps);
	}
	
	void NextFrame()
	{
            //projector.coook.SetTexture("_ShadowTex", frames[frameIndex]);
            projector.cookie = frames[frameIndex];
            frameIndex = (frameIndex + 1) % frames.Length;
	}
}