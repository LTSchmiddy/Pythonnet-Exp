﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InAudioLeanTween;
using InAudioSystem.ExtensionMethods;
using InAudioSystem.Internal;
using UnityEngine;
using UnityEngine.Audio;

namespace InAudioSystem.Runtime
{

    /// <summary>
    /// InAudio class used to play audio nodes
    /// </summary>
    public class InPlayer : MonoBehaviour
    {
        /// <summary>
        /// Internal InAudio play method. Please use InAudio.Play(...) to play audio
        /// </summary>
        public void _internalPlay(InAudioNode node, GameObject controllingObject, RuntimeInfo playingInfo, float fade,
            LeanTweenType fadeType)
        {
            if (node.IsRootOrFolder)
            {
                Debug.LogWarning("InAudio: Cannot play Folder node \"" + node.Name + "\"");
                return;
            }
            dspPool = InAudioInstanceFinder.DSPTimePool;
            breakLoop = false;

            controlling = controllingObject;
            ParentFolder = TreeWalker.FindParentBeforeFolder(node);

            //This is to queue the next playing node, as the first clip will not yield a waitforseconds
            //firstClip = true;
            runtimeInfo = playingInfo;

            PlayingNode = node;
            DSPTime time = dspPool.GetObject();


            time.CurrentEndTime = AudioSettings.dspTime;
            isActive = true;
            fadeVolume = 1f;
            volume = 1.0f;
            _spatialBlend = 1.0f;
            spread = 0.0f;
            _panStereo = 0.0f;
            if (fade > 0)
            {
                LTDescr tweever = LeanTween.value(controllingObject, f =>
                {
                    fadeVolume = f;
                    SetFadeVolume(f);
                }, 0f, 1f, fade);
                tweever.tweenType = fadeType;
                fadeVolume = 0;
                SetFadeVolume(0);
            }

            StartCoroutine(StartPlay(node, time));
        }

        /// <summary>
        /// To call when the audio is done playing
        /// </summary>
        public Action<GameObject, InAudioNode> OnCompleted;

        /// <summary>
        /// Break any looping sounds
        /// </summary>
        public void Break()
        {
            breakLoop = true;
        }


        /// <summary>
        /// 3D SpatialBlend as in Audio Sources
        /// </summary>
        public float SpatialBlend
        {
            get { return _spatialBlend; }
            set
            {
                for (int i = 0; i < audioSources.Count; i++)
                {
                    audioSources[i].AudioSource.spatialBlend = value;
                }
                _spatialBlend = value;
            }
        }

        /// <summary>
        /// 3D Spread as in Audio Sources
        /// </summary>
        public float Spread
        {
            get { return spread; }
            set
            {
                for (int i = 0; i < audioSources.Count; i++)
                {
                    audioSources[i].AudioSource.spread = value;
                }
                spread = value;
            }
        }

        /// <summary>
        /// 2D Pan as in Audio Sources
        /// </summary>
        public float PanStereo
        {
            get { return _panStereo; }
            set
            {
                for (int i = 0; i < audioSources.Count; i++)
                {
                    audioSources[i].AudioSource.panStereo = value;
                }
                _panStereo = value;
            }
        }

        /// <summary>
        /// Stop the current playing sound
        /// </summary>
        public void Stop()
        {
            //Use a non zero amount to avoid any issues with sound glitching
            StartCoroutine(StopAndMute(0.08f, LeanTweenType.notUsed));
        }

        /// <summary>
        /// Stop with fade out
        /// </summary>
        /// <param name="fadeOutTime"></param>
        public void Stop(float fadeOutTime, LeanTweenType tweenType = LeanTweenType.easeInOutQuad)
        {
            StartCoroutine(StopAndMute(fadeOutTime, tweenType));
        }

        /// <summary>
        /// Private InAudio initialization.
        /// </summary>
        /// <param name="cleanup"></param>
        public void internalInitialize(Action<ObjectAudioList> cleanup)
        {
            this.cleanup = cleanup;
        }

        public void internalUpdateFalloff(Vector3 listenerPos)
        {
            if (audioSources == null)
                return;

            Vector3 pos = transform.position;

            float distance = Vector3.Distance(pos, listenerPos);

            for (int i = 0; i < audioSources.Count; i++)
            {
                var player = audioSources[i];
                if (player != null && player.UsedNode != null)
                {
                    CalcRolloff(player, distance);
                }
            }
        }

        private void CalcRolloff(InRuntimePlayer player, float distance)
        {
            var usedNode = GetRolloffNode(player.UsedNode);
            var data = usedNode._nodeData as InAudioNodeData;
            if (data != null)
            {
                player.Rolloff = 1f;
                if (data.RolloffMode == AudioRolloffMode.Custom)
                {
                    player.Rolloff = data.FalloffCurve.Evaluate(Mathf.Clamp01(distance/data.MaxDistance));
                }
                SetVolume(player);
                //player.AudioSource.SetLoudness(player.OriginalVolume * attachedToBus.RuntimeSelfVolume * volume * player.Rolloff);
            }
        }

        private InAudioNode GetRolloffNode(InAudioNode current)
        {
            var data = current._nodeData as InAudioNodeData;
            if (data.OverrideAttenuation || current._parent.IsRootOrFolder)
                return current;
            else
            {
                return GetRolloffNode(current._parent);
            }
        }

        public void internalDateUpdate(InAudioNode node)
        {
            if (audioSources == null)
                return;

            var data = node._nodeData as InAudioNodeData;
            if (data == null)
                return;

            for (int i = 0; i < audioSources.Count; i++)
            {
                var player = audioSources[i];


                if (player != null && player.UsedNode == node)
                {
                    var audioSource = player.AudioSource;
                    audioSource.rolloffMode = data.RolloffMode;
                    audioSource.minDistance = data.MinDistance;
                    audioSource.maxDistance = data.MaxDistance;
                }
            }
        }

        public float Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                for (int i = 0; i < audioSources.Count; i++)
                {
                    if (audioSources == null)
                        continue;
                    var source = audioSources[i];
                    SetVolume(source);
                }
            }
        }

        public InAudioNode NodePlaying
        {
            get { return PlayingNode; }
        }

        private float SetVolume(InRuntimePlayer source)
        {
            float vol = source.OriginalVolume*volume*source.Rolloff*fadeVolume;
            //Debug.Log(source.OriginalVolume + " " + volume + " " + source.Rolloff + " " + fadeVolume);
            return source.AudioSource.SetLoudness(vol);
        }


        private void SetFadeVolume(float newFadeVolume)
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (audioSources == null)
                    continue;
                var source = audioSources[i];
                float vol = source.OriginalVolume*newFadeVolume;
                source.AudioSource.SetLoudness(vol);
            }
        }

        //public void SetNewMixerGroup(AudioMixerGroup bus)
        //{
        //    //TODO fix
        //    attachedToMixer = bus;
        //}

        private void StopFast()
        {
            Cleanup();
            StopAllCoroutines();
        }


        private IEnumerator StopAndMute(float fadeOutTime, LeanTweenType tweenType)
        {
            if (fadeOutTime < 0.1f)
            {
                float volume = 1.0f;
                while (volume > 0.01)
                {
                    volume -= 13.0f*Time.deltaTime;
                    for (int i = 0; i < audioSources.Count; ++i)
                    {
                        var source = audioSources[i];
                        if (audioSources[i] == null)
                            continue;
                        source.AudioSource.volume = volume;
                    }
                    yield return null;
                }
                FinalCleanup();
            }
            else
            {
                var tween = LeanTween.value(gameObject, (f, o) => (o as InPlayer).Volume = f, volume*fadeVolume, 0.0f,
                    fadeOutTime);
                tween.onUpdateParam = this;
                tween.tweenType = tweenType;
                tween.onComplete = FinalCleanup;
            }


        }

        private void FinalCleanup()
        {
            for (int i = 0; i < audioSources.Count; ++i)
            {
                var source = audioSources[i];
                if (audioSources[i] == null)
                    continue;
                source.AudioSource.Stop();
            }

            Cleanup();
            StopAllCoroutines();
        }

        private float volume = 1.0f;
        private float fadeVolume = 1.0f;
        private float _spatialBlend;
        private float spread;
        private float _panStereo;

        private GameObject controlling;

        private InAudioNode PlayingNode;
        private InAudioNode ParentFolder; //The nearest folder or root

        private readonly List<InRuntimePlayer> audioSources = new List<InRuntimePlayer>(1);

        private InDSPTimePool dspPool;

        //Set from InAudioEventWorker
        private Action<ObjectAudioList> cleanup;

        //private bool firstClip;

        private bool isActive;

        private RuntimeInfo runtimeInfo;

        private bool breakLoop;

        private int currentIndex;

        public ReadOnlyCollection<InRuntimePlayer> PlayingSources
        {
            get { return new ReadOnlyCollection<InRuntimePlayer>(audioSources); }
        }

        private InRuntimePlayer Current
        {
            get { return audioSources[currentIndex]; }
        }


        private bool firstPlay = true;

        private IEnumerator StartPlay(InAudioNode current, DSPTime endTime)
        {
            yield return StartCoroutine(NextNode(current, endTime, 0));

            yield return new WaitForSeconds((float) (endTime.CurrentEndTime - AudioSettings.dspTime));
            endTime.Player = null;
            dspPool.ReleaseObject(endTime);
            StopFast();
        }

        private IEnumerator NextNode(InAudioNode current, DSPTime endTime, float sampleOffset)
        {
            byte loops = 0;
            var nodeData = current._nodeData as InAudioNodeData;
            bool loopInfinite = nodeData.LoopInfinite;
            if (!loopInfinite)
                loops = RuntimeHelper.GetLoops(current);

            endTime.CurrentEndTime += RuntimeHelper.InitialDelay(nodeData);

            if (nodeData.Loop == false)
            {
                loops = 0;
                loopInfinite = false;
            }
            for (int i = 0; i < 1 + loops || loopInfinite; ++i) //For at least once
            {

                if (current._type == AudioNodeType.Audio)
                {
                    NextFreeAudioSource();

                    var audioData = current._nodeData as InAudioData;
                    float preOffset = sampleOffset + RuntimeHelper.Offset(nodeData);

                    if (preOffset > 0)
                    {
                        int sampleCount = audioData._clip.samples;
                        if (sampleOffset < sampleCount)
                        {
                            PlayNode(current, endTime, preOffset, audioData);

                            if (!firstPlay)
                                yield return
                                    new WaitForSeconds((float) (endTime.CurrentEndTime - AudioSettings.dspTime) - 0.5f);
                            else
                            {
                                yield return
                                    new WaitForSeconds((float) (endTime.CurrentEndTime - AudioSettings.dspTime)/2f);
                            }
                            firstPlay = false;
                        }
                    }
                    else
                    {
                        PlayNode(current, endTime, preOffset, audioData);
                        if (!firstPlay)
                        {
                            yield return new WaitForSeconds((float) (endTime.CurrentEndTime - AudioSettings.dspTime) - 0.5f);
                        }
                        else
                        {
                            yield return new WaitForSeconds((float) (endTime.CurrentEndTime - AudioSettings.dspTime)/2f);
                        }
                        firstPlay = false;
                    }
                }
                else if (current._type == AudioNodeType.Random)
                {
                    sampleOffset += RuntimeHelper.Offset(nodeData);
                    var randomData = nodeData as RandomData;
                    if (current._children.Count != randomData.weights.Count)
                    {
                        Debug.LogWarning("InAudio: There is a problem with the random weights in the node \"" +
                                                 current.Name + "\", id=" + current._ID +
                                                 ". \nPlease open the audio window for the node and follow instructions");
                    }

                    if (current._children.Count > 0)
                    {
                        int next = RuntimeHelper.SelectRandom(randomData);
                        if (next != -1)
                        {
                            yield return StartCoroutine(NextNode(current._children[next], endTime, sampleOffset));
                        }
                        else
                        {
                            Debug.LogWarning("InAudio: Cannot pick random as node \"" +
                                                 current.Name + "\", id=" + current._ID +
                                                 " has no children.\n");
                        }
                    }
                }
                else if (current._type == AudioNodeType.Sequence)
                {
                    sampleOffset += RuntimeHelper.Offset(nodeData);
                    for (int j = 0; j < current._children.Count; ++j)
                    {
                        yield return StartCoroutine(NextNode(current._children[j], endTime, sampleOffset));
                    }
                }
                else if (current._type == AudioNodeType.Multi)
                {
                    sampleOffset += RuntimeHelper.Offset(nodeData);
                    int childrenCount = current._children.Count;

                    Coroutine[] toStart = InAudioInstanceFinder.CoroutinePool.GetArray(childrenCount);
                    DSPTime[] childTimes = InAudioInstanceFinder.DSPArrayPool.GetArray(childrenCount);

                    for (int j = 0; j < childrenCount; ++j)
                    {
                        DSPTime dspTime = dspPool.GetObject();
                        dspTime.CurrentEndTime = endTime.CurrentEndTime;
                        childTimes[j] = dspTime;
                    }
                    for (int j = 0; j < childrenCount; ++j)
                    {
                        toStart[j] = StartCoroutine(NextNode(current._children[j], childTimes[j], sampleOffset));
                    }

                    for (int j = 0; j < childrenCount; j++)
                    {
                        yield return toStart[j];
                    }

                    for (int j = 0; j < childrenCount; ++j)
                    {
                        DSPTime dspTime = childTimes[j];
                        if (endTime.CurrentEndTime < dspTime.CurrentEndTime)
                            endTime.CurrentEndTime = dspTime.CurrentEndTime;
                        else
                            dspPool.ReleaseObject(dspTime);
                    }

                    InAudioInstanceFinder.CoroutinePool.Release(toStart);
                    InAudioInstanceFinder.DSPArrayPool.Release(childTimes);
                }

                if (breakLoop && (loops > 0 || loopInfinite))
                {
                    breakLoop = false;
                    loops = 0;
                    loopInfinite = false;
                }
            }
        }

        private void PlayNode(InAudioNode current, DSPTime endTime, float offset, InAudioData audioData)
        {
            float nodeVolume;

            double playTime = endTime.CurrentEndTime;
            double timeLeft = 0;
            if (endTime.Player != null && endTime.Player.clip != null)
            {
                int samples = endTime.Player.timeSamples;
                if (samples > 0 && samples < endTime.Player.clip.samples)
                {
                    timeLeft = TimeLeftOfClip(endTime, samples)/endTime.Player.pitch;
                    double dspTime = AudioSettings.dspTime;
                    playTime = dspTime + timeLeft;
                }
            }

            float length = PlayScheduled(ParentFolder, current, audioData, playTime,
                offset, out nodeVolume);

     

            endTime.CurrentEndTime += length;
            endTime.Player = Current.AudioSource;
        }

        private static double TimeLeftOfClip(DSPTime endTime, int samples)
        {
            return endTime.Player.clip.ExactLength() - (samples/(double) endTime.Player.clip.frequency);
        }

        private float PlayScheduled(InAudioNode startNode, InAudioNode currentNode, InAudioData audioData,
            double playAtDSPTime, float offset, out float nodeVolume)
        {
            float length = 0;
            nodeVolume = 1;
            if (audioData._clip != null)
            {
                var clip = audioData._clip;
                length = clip.ExactLength();

                length -= offset;
                float lengthOffset = offset*clip.frequency;
                var source = Current.AudioSource;


                source.clip = clip;
                nodeVolume = RuntimeHelper.CalcVolume(startNode, currentNode);
                Current.OriginalVolume = nodeVolume;

                SetVolume(Current);

                source.spatialBlend = RuntimeHelper.CalcBlend(startNode, currentNode);
                source.pitch = RuntimeHelper.CalcPitch(startNode, currentNode);
                source.rolloffMode = RuntimeHelper.CalcAttentutation(startNode, currentNode, source);
                source.outputAudioMixerGroup = currentNode.GetMixerGroup();
                length = RuntimeHelper.LengthFromPitch(length, source.pitch);
                Current.EndTime = playAtDSPTime + length;
                Current.StartTime = playAtDSPTime;
                Current.UsedNode = currentNode;

                source.panStereo += _panStereo;
                source.spread *= spread;
                source.spatialBlend *= _spatialBlend;
                source.timeSamples = (int) (lengthOffset);
                source.PlayScheduled(playAtDSPTime);
            }
            else
            {
                Debug.LogWarning("InAudio: Audio clip missing on audio node \"" + currentNode.Name + "\", id=" + currentNode._ID);
            }
            return length;
        }

        private void NextFreeAudioSource()
        {
            double dspTime = AudioSettings.dspTime;

            for (int i = 0; i < audioSources.Count; ++i)
            {
                if (audioSources[i].EndTime < dspTime)
                {
                    currentIndex = i;
                    return;
                }

            }
            var audioObject = InAudioInstanceFinder.InRuntimePlayerPool.GetObject();
            var objectTransform = audioObject.transform;
            objectTransform.parent = transform;
            objectTransform.localPosition = new Vector3();
            audioSources.Add(audioObject);
            currentIndex = audioSources.Count - 1;
        }

        private void OnDisable()
        {
            if (isActive)
                Cleanup();
        }

        private void OnDestroy()
        {
            Cleanup();

            if (cleanup != null && runtimeInfo != null)
                cleanup(runtimeInfo.PlacedIn);
        }

        private void Cleanup()
        {
            if (OnCompleted != null)
            {
                OnCompleted(controlling, NodePlaying);
            }
            isActive = false;
            if (PlayingNode != null && PlayingNode.CurrentInstances != null)
            {
                var instances = PlayingNode.CurrentInstances;
                for (int i = 0; i < instances.Count; i++)
                {
                    if (instances[i].Player == this)
                    {
                        instances.RemoveAt(i);
                        break;
                    }
                }
            }

            for (int i = 0; i < audioSources.Count; i++)
            {
                audioSources[i].AudioSource.clip = null;
            }


            if (InAudioInstanceFinder.Instance != null)
            {
                var pool = InAudioInstanceFinder.InRuntimePlayerPool;
                for (int i = audioSources.Count - 1; i >= 0; i--)
                {
                    pool.QueueRelease(audioSources[i]);
                    audioSources.RemoveAt(i);
                }
            }


            var controllerPool = InAudioInstanceFinder.RuntimePlayerControllerPool;
            if (controllerPool != null)
                controllerPool.QueueRelease(this);

            RuntimeHelper.ReleaseRuntimeInfo(runtimeInfo);
            OnCompleted = null;
        }
    }
}