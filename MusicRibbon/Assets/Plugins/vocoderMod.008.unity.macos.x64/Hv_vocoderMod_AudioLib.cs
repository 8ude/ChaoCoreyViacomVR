/**
 * Copyright (c) 2017 Enzien Audio, Ltd.
 * 
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice,
 *    this list of conditions, and the following disclaimer.
 * 
 * 2. Redistributions in binary form must reproduce the phrase "powered by heavy",
 *    the heavy logo, and a hyperlink to https://enzienaudio.com, all in a visible
 *    form.
 * 
 *   2.1 If the Application is distributed in a store system (for example,
 *       the Apple "App Store" or "Google Play"), the phrase "powered by heavy"
 *       shall be included in the app description or the copyright text as well as
 *       the in the app itself. The heavy logo will shall be visible in the app
 *       itself as well.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;
using AOT;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Hv_vocoderMod_AudioLib))]
public class Hv_vocoderMod_Editor : Editor {

  [MenuItem("Heavy/vocoderMod")]
  static void CreateHv_vocoderMod() {
    GameObject target = Selection.activeGameObject;
    if (target != null) {
      target.AddComponent<Hv_vocoderMod_AudioLib>();
    }
  }
  
  private Hv_vocoderMod_AudioLib _dsp;

  private void OnEnable() {
    _dsp = target as Hv_vocoderMod_AudioLib;
  }

  public override void OnInspectorGUI() {
    bool isEnabled = _dsp.IsInstantiated();
    if (!isEnabled) {
      EditorGUILayout.LabelField("Press Play!",  EditorStyles.centeredGreyMiniLabel);
    }
    GUILayout.EndVertical();

    // parameters
    GUI.enabled = true;
    GUILayout.BeginVertical();
    EditorGUILayout.Space();
    EditorGUI.indentLevel++;
    
    // bp_freq_offset
    GUILayout.BeginHorizontal();
    float bp_freq_offset = _dsp.GetFloatParameter(Hv_vocoderMod_AudioLib.Parameter.Bp_freq_offset);
    float newBp_freq_offset = EditorGUILayout.Slider("bp_freq_offset", bp_freq_offset, 0.0f, 8000.0f);
    if (bp_freq_offset != newBp_freq_offset) {
      _dsp.SetFloatParameter(Hv_vocoderMod_AudioLib.Parameter.Bp_freq_offset, newBp_freq_offset);
    }
    GUILayout.EndHorizontal();
    
    // notch_freq_offset
    GUILayout.BeginHorizontal();
    float notch_freq_offset = _dsp.GetFloatParameter(Hv_vocoderMod_AudioLib.Parameter.Notch_freq_offset);
    float newNotch_freq_offset = EditorGUILayout.Slider("notch_freq_offset", notch_freq_offset, 0.0f, 8000.0f);
    if (notch_freq_offset != newNotch_freq_offset) {
      _dsp.SetFloatParameter(Hv_vocoderMod_AudioLib.Parameter.Notch_freq_offset, newNotch_freq_offset);
    }
    GUILayout.EndHorizontal();
    
    // out_dry_gain
    GUILayout.BeginHorizontal();
    float out_dry_gain = _dsp.GetFloatParameter(Hv_vocoderMod_AudioLib.Parameter.Out_dry_gain);
    float newOut_dry_gain = EditorGUILayout.Slider("out_dry_gain", out_dry_gain, 0.0f, 100.0f);
    if (out_dry_gain != newOut_dry_gain) {
      _dsp.SetFloatParameter(Hv_vocoderMod_AudioLib.Parameter.Out_dry_gain, newOut_dry_gain);
    }
    GUILayout.EndHorizontal();
    
    // out_gain
    GUILayout.BeginHorizontal();
    float out_gain = _dsp.GetFloatParameter(Hv_vocoderMod_AudioLib.Parameter.Out_gain);
    float newOut_gain = EditorGUILayout.Slider("out_gain", out_gain, 0.0f, 1000.0f);
    if (out_gain != newOut_gain) {
      _dsp.SetFloatParameter(Hv_vocoderMod_AudioLib.Parameter.Out_gain, newOut_gain);
    }
    GUILayout.EndHorizontal();
    
    // ramp_note
    GUILayout.BeginHorizontal();
    float ramp_note = _dsp.GetFloatParameter(Hv_vocoderMod_AudioLib.Parameter.Ramp_note);
    float newRamp_note = EditorGUILayout.Slider("ramp_note", ramp_note, 0.0f, 127.0f);
    if (ramp_note != newRamp_note) {
      _dsp.SetFloatParameter(Hv_vocoderMod_AudioLib.Parameter.Ramp_note, newRamp_note);
    }
    GUILayout.EndHorizontal();
    
    // vocoder_q
    GUILayout.BeginHorizontal();
    float vocoder_q = _dsp.GetFloatParameter(Hv_vocoderMod_AudioLib.Parameter.Vocoder_q);
    float newVocoder_q = EditorGUILayout.Slider("vocoder_q", vocoder_q, 0.0f, 100.0f);
    if (vocoder_q != newVocoder_q) {
      _dsp.SetFloatParameter(Hv_vocoderMod_AudioLib.Parameter.Vocoder_q, newVocoder_q);
    }
    GUILayout.EndHorizontal();
    EditorGUI.indentLevel--;
  }
}
#endif // UNITY_EDITOR

[RequireComponent (typeof (AudioSource))]
public class Hv_vocoderMod_AudioLib : MonoBehaviour {
  
  // Parameters are used to send float messages into the patch context (thread-safe).
  // Example usage:
  /*
    void Start () {
        Hv_vocoderMod_AudioLib script = GetComponent<Hv_vocoderMod_AudioLib>();
        // Get and set a parameter
        float bp_freq_offset = script.GetFloatParameter(Hv_vocoderMod_AudioLib.Parameter.Bp_freq_offset);
        script.SetFloatParameter(Hv_vocoderMod_AudioLib.Parameter.Bp_freq_offset, bp_freq_offset + 0.1f);
    }
  */
  public enum Parameter : uint {
    Bp_freq_offset = 0xB195E4A9,
    Notch_freq_offset = 0x442545AC,
    Out_dry_gain = 0x4C78D95A,
    Out_gain = 0x118CA30,
    Ramp_note = 0x11F3602D,
    Vocoder_q = 0x2424EE26,
  }
  
  // Delegate method for receiving float messages from the patch context (thread-safe).
  // Example usage:
  /*
    void Start () {
        Hv_vocoderMod_AudioLib script = GetComponent<Hv_vocoderMod_AudioLib>();
        script.RegisterSendHook();
        script.FloatReceivedCallback += OnFloatMessage;
    }

    void OnFloatMessage(Hv_vocoderMod_AudioLib.FloatMessage message) {
        Debug.Log(message.receiverName + ": " + message.value);
    }
  */
  public class FloatMessage {
    public string receiverName;
    public float value;

    public FloatMessage(string name, float x) {
      receiverName = name;
      value = x;
    }
  }
  public delegate void FloatMessageReceived(FloatMessage message);
  public FloatMessageReceived FloatReceivedCallback;
  public float bp_freq_offset = 2000.0f;
  public float notch_freq_offset = 1000.0f;
  public float out_dry_gain = 1.0f;
  public float out_gain = 100.0f;
  public float ramp_note = 30.0f;
  public float vocoder_q = 25.0f;

  // internal state
  private Hv_vocoderMod_Context _context;

  public bool IsInstantiated() {
    return (_context != null);
  }

  public void RegisterSendHook() {
    _context.RegisterSendHook();
  }
  
  // see Hv_vocoderMod_AudioLib.Parameter for definitions
  public float GetFloatParameter(Hv_vocoderMod_AudioLib.Parameter param) {
    switch (param) {
      case Parameter.Bp_freq_offset: return bp_freq_offset;
      case Parameter.Notch_freq_offset: return notch_freq_offset;
      case Parameter.Out_dry_gain: return out_dry_gain;
      case Parameter.Out_gain: return out_gain;
      case Parameter.Ramp_note: return ramp_note;
      case Parameter.Vocoder_q: return vocoder_q;
      default: return 0.0f;
    }
  }

  public void SetFloatParameter(Hv_vocoderMod_AudioLib.Parameter param, float x) {
    switch (param) {
      case Parameter.Bp_freq_offset: {
        x = Mathf.Clamp(x, 0.0f, 8000.0f);
        bp_freq_offset = x;
        break;
      }
      case Parameter.Notch_freq_offset: {
        x = Mathf.Clamp(x, 0.0f, 8000.0f);
        notch_freq_offset = x;
        break;
      }
      case Parameter.Out_dry_gain: {
        x = Mathf.Clamp(x, 0.0f, 100.0f);
        out_dry_gain = x;
        break;
      }
      case Parameter.Out_gain: {
        x = Mathf.Clamp(x, 0.0f, 1000.0f);
        out_gain = x;
        break;
      }
      case Parameter.Ramp_note: {
        x = Mathf.Clamp(x, 0.0f, 127.0f);
        ramp_note = x;
        break;
      }
      case Parameter.Vocoder_q: {
        x = Mathf.Clamp(x, 0.0f, 100.0f);
        vocoder_q = x;
        break;
      }
      default: return;
    }
    if (IsInstantiated()) _context.SendFloatToReceiver((uint) param, x);
  }
  
  public void FillTableWithMonoAudioClip(string tableName, AudioClip clip) {
    if (clip.channels > 1) {
      Debug.LogWarning("Hv_vocoderMod_AudioLib: Only loading first channel of '" +
          clip.name + "' into table '" +
          tableName + "'. Multi-channel files are not supported.");
    }
    float[] buffer = new float[clip.samples]; // copy only the 1st channel
    clip.GetData(buffer, 0);
    _context.FillTableWithFloatBuffer(tableName, buffer);
  }

  public void FillTableWithFloatBuffer(string tableName, float[] buffer) {
    _context.FillTableWithFloatBuffer(tableName, buffer);
  }

  private void Awake() {
    _context = new Hv_vocoderMod_Context((double) AudioSettings.outputSampleRate);
  }
  
  private void Start() {
    _context.SendFloatToReceiver((uint) Parameter.Bp_freq_offset, bp_freq_offset);
    _context.SendFloatToReceiver((uint) Parameter.Notch_freq_offset, notch_freq_offset);
    _context.SendFloatToReceiver((uint) Parameter.Out_dry_gain, out_dry_gain);
    _context.SendFloatToReceiver((uint) Parameter.Out_gain, out_gain);
    _context.SendFloatToReceiver((uint) Parameter.Ramp_note, ramp_note);
    _context.SendFloatToReceiver((uint) Parameter.Vocoder_q, vocoder_q);
  }
  
  private void Update() {
    // retreive sent messages
    if (_context.IsSendHookRegistered()) {
      Hv_vocoderMod_AudioLib.FloatMessage tempMessage;
      while ((tempMessage = _context.msgQueue.GetNextMessage()) != null) {
        FloatReceivedCallback(tempMessage);
      }
    }
  }

  private void OnAudioFilterRead(float[] buffer, int numChannels) {
    Assert.AreEqual(numChannels, _context.GetNumOutputChannels()); // invalid channel configuration
    _context.Process(buffer, buffer.Length / numChannels); // process dsp
  }
}

class Hv_vocoderMod_Context {

#if UNITY_IOS && !UNITY_EDITOR
  private const string _dllName = "__Internal";
#else
  private const string _dllName = "Hv_vocoderMod_AudioLib";
#endif

  // Thread-safe message queue
  public class SendMessageQueue {
    private readonly object _msgQueueSync = new object();
    private readonly Queue<Hv_vocoderMod_AudioLib.FloatMessage> _msgQueue = new Queue<Hv_vocoderMod_AudioLib.FloatMessage>();

    public Hv_vocoderMod_AudioLib.FloatMessage GetNextMessage() {
      lock (_msgQueueSync) {
        return (_msgQueue.Count != 0) ? _msgQueue.Dequeue() : null;
      }
    }

    public void AddMessage(string receiverName, float value) {
      Hv_vocoderMod_AudioLib.FloatMessage msg = new Hv_vocoderMod_AudioLib.FloatMessage(receiverName, value);
      lock (_msgQueueSync) {
        _msgQueue.Enqueue(msg);
      }
    }
  }

  public readonly SendMessageQueue msgQueue = new SendMessageQueue();
  private readonly GCHandle gch;
  private readonly IntPtr _context; // handle into unmanaged memory
  private SendHook _sendHook = null;

  [DllImport (_dllName)]
  private static extern IntPtr hv_vocoderMod_new_with_options(double sampleRate, int poolKb, int inQueueKb, int outQueueKb);

  [DllImport (_dllName)]
  private static extern int hv_processInlineInterleaved(IntPtr ctx,
      [In] float[] inBuffer, [Out] float[] outBuffer, int numSamples);

  [DllImport (_dllName)]
  private static extern void hv_delete(IntPtr ctx);

  [DllImport (_dllName)]
  private static extern double hv_getSampleRate(IntPtr ctx);

  [DllImport (_dllName)]
  private static extern int hv_getNumInputChannels(IntPtr ctx);

  [DllImport (_dllName)]
  private static extern int hv_getNumOutputChannels(IntPtr ctx);

  [DllImport (_dllName)]
  private static extern void hv_setSendHook(IntPtr ctx, SendHook sendHook);

  [DllImport (_dllName)]
  private static extern void hv_setPrintHook(IntPtr ctx, PrintHook printHook);

  [DllImport (_dllName)]
  private static extern int hv_setUserData(IntPtr ctx, IntPtr userData);

  [DllImport (_dllName)]
  private static extern IntPtr hv_getUserData(IntPtr ctx);

  [DllImport (_dllName)]
  private static extern void hv_sendBangToReceiver(IntPtr ctx, uint receiverHash);

  [DllImport (_dllName)]
  private static extern void hv_sendFloatToReceiver(IntPtr ctx, uint receiverHash, float x);

  [DllImport (_dllName)]
  private static extern uint hv_msg_getTimestamp(IntPtr message);

  [DllImport (_dllName)]
  private static extern bool hv_msg_hasFormat(IntPtr message, string format);

  [DllImport (_dllName)]
  private static extern float hv_msg_getFloat(IntPtr message, int index);

  [DllImport (_dllName)]
  private static extern bool hv_table_setLength(IntPtr ctx, uint tableHash, uint newSampleLength);

  [DllImport (_dllName)]
  private static extern IntPtr hv_table_getBuffer(IntPtr ctx, uint tableHash);

  [DllImport (_dllName)]
  private static extern float hv_samplesToMilliseconds(IntPtr ctx, uint numSamples);

  [DllImport (_dllName)]
  private static extern uint hv_stringToHash(string s);

  private delegate void PrintHook(IntPtr context, string printName, string str, IntPtr message);

  private delegate void SendHook(IntPtr context, string sendName, uint sendHash, IntPtr message);

  public Hv_vocoderMod_Context(double sampleRate, int poolKb=10, int inQueueKb=2, int outQueueKb=2) {
    gch = GCHandle.Alloc(msgQueue);
    _context = hv_vocoderMod_new_with_options(sampleRate, poolKb, inQueueKb, outQueueKb);
    hv_setPrintHook(_context, new PrintHook(OnPrint));
    hv_setUserData(_context, GCHandle.ToIntPtr(gch));
  }

  ~Hv_vocoderMod_Context() {
    hv_delete(_context);
    GC.KeepAlive(_context);
    GC.KeepAlive(_sendHook);
    gch.Free();
  }

  public void RegisterSendHook() {
    // Note: send hook functionality only applies to messages containing a single float value
    if (_sendHook == null) {
      _sendHook = new SendHook(OnMessageSent);
      hv_setSendHook(_context, _sendHook);
    }
  }

  public bool IsSendHookRegistered() {
    return (_sendHook != null);
  }

  public double GetSampleRate() {
    return hv_getSampleRate(_context);
  }

  public int GetNumInputChannels() {
    return hv_getNumInputChannels(_context);
  }

  public int GetNumOutputChannels() {
    return hv_getNumOutputChannels(_context);
  }

  public void SendBangToReceiver(uint receiverHash) {
    hv_sendBangToReceiver(_context, receiverHash);
  }

  public void SendFloatToReceiver(uint receiverHash, float x) {
    hv_sendFloatToReceiver(_context, receiverHash, x);
  }

  public void FillTableWithFloatBuffer(string tableName, float[] buffer) {
    uint tableHash = hv_stringToHash(tableName);
    if (hv_table_getBuffer(_context, tableHash) != IntPtr.Zero) {
      hv_table_setLength(_context, tableHash, (uint) buffer.Length);
      Marshal.Copy(buffer, 0, hv_table_getBuffer(_context, tableHash), buffer.Length);
    } else {
      Debug.Log(string.Format("Table '{0}' doesn't exist in the patch context.", tableName));
    }
  }

  public uint StringToHash(string s) {
    return hv_stringToHash(s);
  }

  public int Process(float[] buffer, int numFrames) {
    return hv_processInlineInterleaved(_context, buffer, buffer, numFrames);
  }

  [MonoPInvokeCallback(typeof(PrintHook))]
  private static void OnPrint(IntPtr context, string printName, string str, IntPtr message) {
    float timeInSecs = hv_samplesToMilliseconds(context, hv_msg_getTimestamp(message)) / 1000.0f;
    Debug.Log(string.Format("{0} [{1:0.000}]: {2}", printName, timeInSecs, str));
  }

  [MonoPInvokeCallback(typeof(SendHook))]
  private static void OnMessageSent(IntPtr context, string sendName, uint sendHash, IntPtr message) {
    if (hv_msg_hasFormat(message, "f")) {
      SendMessageQueue msgQueue = (SendMessageQueue) GCHandle.FromIntPtr(hv_getUserData(context)).Target;
      msgQueue.AddMessage(sendName, hv_msg_getFloat(message, 0));
    }
  }
}
