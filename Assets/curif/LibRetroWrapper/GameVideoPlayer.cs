/*
This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
You should have received a copy of the GNU General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.
*/
//#define DISABLE_VIDEO
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


[RequireComponent(typeof(UnityEngine.Video.VideoPlayer))]
public class GameVideoPlayer : MonoBehaviour {

    Renderer display;
    VideoPlayer videoPlayer;
    bool invertx, inverty;
    ShaderScreenBase shader;

    //Video data
    public string videoPath;
    private bool isPreparing = false;

    // Start is called before the first frame update
    void Start() {
      display = GetComponent<Renderer>();
      videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();
      videoPlayer.prepareCompleted += PrepareCompleted;
      videoPlayer.errorReceived += ErrorReceived;
      isPreparing = false;
    }

    public GameVideoPlayer setVideo(string path, ShaderScreenBase shader, bool invertx, bool inverty) {
#if ! DISABLE_VIDEO
      if (string.IsNullOrEmpty(path)) 
          return this;

      this.videoPath = path;
      this.invertx = invertx;
      this.inverty = inverty;
      this.shader = shader;

      videoPlayer.playOnAwake = true;
      videoPlayer.isLooping = true;
      videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.APIOnly;
      //videoPlayer.targetMaterialRenderer = display;
      //VideoPlayer.targetMaterialProperty = shader.TargetMaterialProperty;

      ConfigManager.WriteConsole($"[videoPlayer] Start {videoPath} ====");
#endif
      return this;
    }

    public Texture texture {
      get {
        return videoPlayer.texture;
      }
    }

    private void PrepareVideo()
    {
      isPreparing = true;
      videoPlayer.Prepare();
    }

    public GameVideoPlayer Play() {
#if ! DISABLE_VIDEO
      if (videoPlayer == null || string.IsNullOrEmpty(videoPath) || isPreparing)
          return this;

      if (videoPlayer.url != videoPath)
      {
        videoPlayer.url = videoPath;
      }
      
      if (! videoPlayer.isPrepared) 
      {
        ConfigManager.WriteConsole($"[videoPlayer] prepare {videoPath} ====");
        PrepareVideo();
      }
      else if (videoPlayer.isPaused) 
      {
        videoPlayer.isLooping = true;
        shader.Invert(invertx, inverty);
        videoPlayer.Play();
      }
#endif
      return this;
    }

    public GameVideoPlayer Pause() {
#if ! DISABLE_VIDEO
      if (videoPlayer == null || string.IsNullOrEmpty(videoPath) || ! videoPlayer.isPrepared || videoPlayer.isPaused || isPreparing)
          return this;

      //is is necessary because the VideoPlayer.Pause method only works if isLooping is set to false. If isLooping is set to true, the Pause method will have no effect and the video will continue to play.
      //ConfigManager.WriteConsole($"[videoPlayer] pause {videoPath} ====");
      videoPlayer.isLooping = false;
      videoPlayer.Pause();
#endif
      return this;
    }

    public GameVideoPlayer Stop() {
#if ! DISABLE_VIDEO
      if (string.IsNullOrEmpty(videoPath) || ! videoPlayer.isPrepared)
          return this;

      ConfigManager.WriteConsole($"[videoPlayer] stop {videoPath} ====");
      videoPlayer.Stop();
#endif
      return this;
    }
    
    void PrepareCompleted(VideoPlayer vp)
    {
      ConfigManager.WriteConsole($"[videoPlayer] PrepareCompleted play {videoPath} ====");
      vp.Play();
      // The video is ready to play
      isPreparing = false;
      // try it in API mode: shader.Texture = videoPlayer.texture;
      shader.Texture = videoPlayer.texture;
      shader.Invert(invertx, inverty);
    }

    void ErrorReceived(VideoPlayer vp, string message)
    {
      ConfigManager.WriteConsole($"[videoPlayer] ERROR {videoPath} - {message}");
    }

}
