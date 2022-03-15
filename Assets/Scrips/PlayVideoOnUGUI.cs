using System.Collections;
using System.Collections.Generic;
using Nexweron.Core.MSK;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayVideoOnUGUI : MonoBehaviour
{
    private Texture texture;
    
    private VideoPlayer videoPlayer;
    private RawImage rawImage;
    private ChromaKey_Alpha_Simple _chromaKeyAlphaSimple;

    public double nowTime;
    public double videoLength;
    public float DChroma;
    public float DChromaT;
    private void Awake()
    {
        rawImage = this.GetComponent<RawImage>();
        texture = rawImage.texture;
        videoPlayer = this.GetComponent<VideoPlayer>();
        _chromaKeyAlphaSimple = this.GetComponent<ChromaKey_Alpha_Simple>();
        nowTime = videoPlayer.time;
        videoLength = videoPlayer.length;
    }
    private void OnDisable()
    {
        rawImage.texture = texture;
        // rawImage.color= Color.clear;
    }
    private void Update()
    {
        //如果videoPlayer没有对应的视频texture，则返回
        if (videoPlayer.texture == null)
        {
            return;
        }
        nowTime = videoPlayer.time;

        _chromaKeyAlphaSimple.dChroma =DChroma- (float)(nowTime / videoLength)*DChroma;
        _chromaKeyAlphaSimple.dChromaT = DChromaT + (float)(nowTime / videoLength) * (1 - DChromaT);
        //把VideoPlayerd的视频渲染到UGUI的RawImage
        rawImage.texture = videoPlayer.texture;
    }
}
