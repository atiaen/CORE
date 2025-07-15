// using Gum.Wireframe;
// using RenderingLibrary.Graphics;
using Raylib_cs;
using static Raylib_cs.Raylib;
// using GumTest.Renderables;
// using RaylibGum;
using System.Numerics;
using System.Text.Json;

public class Game
{

    public bool shouldExit = false;

    public int screenWidth = 1280;
    public int screenHeight = 720;

    public Font baseFont;
    public Scene currScene;

    public Camera2D camera2D = new Camera2D();
    public static Game Instance { get; private set; }

    RenderTexture2D bloomTarget;

    RenderTexture2D postProcessTarget;
    float time = 0.0f;
    int resolutionLoc = 0;
    float deltaTime;

    float[] resolution;
    int timeLoc = 0;
    Shader postProcessShader;
    public Shader particleShader;

    public Game()
    {
        if (Instance != null && Instance != this)
        {
            return;
        }

        Instance = this;


    }

    public void Start()
    {

        camera2D.Target = new Vector2();
        camera2D.Offset = new Vector2();
        camera2D.Rotation = 0.0f;
        camera2D.Zoom = 1.0f;
        // This tells Gum to use the entire screen
        // GraphicalUiElement.CanvasWidth = screenWidth;
        // GraphicalUiElement.CanvasHeight = screenHeight;

        resolution = [Instance.screenWidth, Instance.screenHeight];

        // GlobalFader.Fader(() =>
        // {
        //     GlobalFader.Unfade();

        // });

        SetConfigFlags(ConfigFlags.ResizableWindow | ConfigFlags.Msaa4xHint | ConfigFlags.VSyncHint);
        InitWindow(screenWidth, screenHeight, "GAME NAME HERE");
        SetExitKey(KeyboardKey.Null);       // Disable KEY_ESCAPE to close window, X-button still works
        InitAudioDevice();
        // AudioManager.Instance.LoadAssets();
        // GumService.Default.Initialize();
        // ParticleSystem.Initiate();

        postProcessTarget = LoadRenderTexture(screenWidth, screenHeight);
        SetTextureFilter(postProcessTarget.Texture, TextureFilter.Trilinear);

        postProcessShader = LoadShader("assets/shaders/base.vs", "assets/shaders/postprocessing.fs");
        particleShader = LoadShader("assets/shaders/particle.vs", "assets/shaders/particle.fs");
        timeLoc = GetShaderLocation(postProcessShader, "uTime");
        resolutionLoc = GetShaderLocation(postProcessShader, "resolution");

        SetShaderValue(postProcessShader, resolutionLoc, resolution, ShaderUniformDataType.Vec2);

        SetShaderValue(postProcessShader, timeLoc, time, ShaderUniformDataType.Float);


        // GlobalParticles.StartBackground();

        currScene.Start();
        // GlobalFader.Unfade(() => { }, 0.8f);


    }

    public void Draw()
    {

        // Vector2 shakeOffset = screenShake.GetShakeOffset();
        // // float shakeRotation = screenShake.GetShakeRotation();
        Camera2D shakenCamera = camera2D;
        // shakenCamera.Target += shakeOffset;
        // shakenCamera.Rotation += shakeRotation;

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        // Enable drawing to texture
        BeginTextureMode(postProcessTarget);
        ClearBackground(Color.RayWhite);

        BeginMode2D(shakenCamera);
        // ParticleSystem.Draw();
        currScene.Draw();

        // GumService.Default.Draw();
        // GlobalFader.Draw();

        EndMode2D();

        // End drawing to texture (now we have a texture available for next passes)
        EndTextureMode();

        // Render previously generated texture using selected postpro shader
        BeginShaderMode(postProcessShader);

        // NOTE: Render texture must be y-flipped due to default OpenGL coordinates (left-bottom)
        DrawTextureRec(
            postProcessTarget.Texture,
            new Rectangle(0, 0, postProcessTarget.Texture.Width, -postProcessTarget.Texture.Height),
            new Vector2(0, 0),
            Color.White
        );

        EndShaderMode();


        DrawFPS(10, 10);

        EndDrawing();

    }

    public void Update()
    {

        if (IsWindowResized())
        {
            resolution[0] = (float)GetScreenWidth();
            resolution[1] = (float)GetScreenHeight();
            screenWidth = GetScreenWidth();
            screenHeight = GetScreenHeight();
            postProcessTarget = LoadRenderTexture(screenWidth, screenHeight);
            // GraphicalUiElement.CanvasWidth = screenWidth;
            // GraphicalUiElement.CanvasHeight = screenHeight;
            SetShaderValue(postProcessShader, resolutionLoc, resolution, ShaderUniformDataType.Vec2);
        }

        time = (float)GetTime();
        SetShaderValue(postProcessShader, timeLoc, time, ShaderUniformDataType.Float);

        currScene.Update();
        // Root.UpdateLayout();

        // GumService.Default.Update(0);


        deltaTime = GetFrameTime();
        // screenShake.Update(deltaTime);
        // TweenManager.Update(deltaTime);
        // ParticleSystem.Update(deltaTime);
        // AudioManager.Instance.Update();
        // // Apply shake offset to camera position
        // camera2D.Target.X += screenShake.GetShakeOffset().X;
        // camera2D.Target.Y += screenShake.GetShakeOffset().Y;
        // camera2D.Rotation += screenShake.GetShakeRotation();
    }

    public void Run()
    {
        while (!shouldExit)
        {
            Update();
            Draw();

        }
    }

    public void Stop()
    {
        UnloadRenderTexture(bloomTarget);
        // ParticleSystem.Stop();
        currScene.Stop();
        CloseWindow();
    }
}