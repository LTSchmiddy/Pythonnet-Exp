using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGamePro;
using RoboRyanTron.QuickButtons;

using LoadSave;
using Python.Runtime;
using PythonEngine;


namespace GameBehaviour {
    public class PythonBehaviourScript : SaveableBehaviour {
        public PythonClassObject pythonClass;
        public SavedPyBehaviourObject instance;

        public override void LinkSaveableInfo(SaveIdRecord record) { 
            instance = PerformLink<SavedPyBehaviourObject>(record.pythonInfo, instance);
        }

        // Private Info:
        private List<string> instanceMethods = new List<string>();

        // Behaviour Message Handler:
        public void DoMessage(string message, params object[] args) {
            // Did we find this message in the python script?
            if (instanceMethods.IndexOf(message) >= 0) {
                PyObject[] pyargs = new PyObject[args.Length + 1];

                pyargs[0] = this.ToPython();
                for (int i = 0; i < args.Length; i++) {
                    pyargs[i] = args[i + 1].ToPython();
                }

                instance.pyObject.InvokeMethod(message, pyargs);
            }
        }

        public void SetupInstance() {
            instanceMethods.Clear();

            foreach (PyObject i in instance.pyObject.Dir()) {
                if (instance.pyObject.GetAttr(i).IsCallable()) {
                    instanceMethods.Add(i.As<string>());
                }
            }
        }

        protected override void Awake() {
            base.Awake();
            instance = pythonClass.NewInstance();
            SetupInstance();

            DoMessage("Awake");
        }

        #region All Messages

        void FixedUpdate() {
            DoMessage("FixedUpdate");
        }
        void LateUpdate() {
            DoMessage("LateUpdate");
        }
        void OnAnimatorIK(int layerIndex) {
            DoMessage("OnAnimatorIK", layerIndex);
        }
        void OnAnimatorMove() {
            DoMessage("OnAnimatorMove");
        }
        void OnApplicationFocus(bool hasFocus) {
            DoMessage("OnApplicationFocus", hasFocus);
        }
        void OnApplicationPause(bool pauseStatus) {
            DoMessage("OnApplicationPause", pauseStatus);
        }
        void OnApplicationQuit() {
            DoMessage("OnApplicationQuit");
        }
        // void OnAudioFilterRead(float[] data, int channels) {
        //     DoMessage("OnAudioFilterRead", data, channels);
        // }
        void OnBecameInvisible() {
            DoMessage("OnBecameInvisible");
        }
        void OnBecameVisible() {
            DoMessage("OnBecameVisible");
        }
        void OnCollisionEnter(Collision collision) {
            DoMessage("OnCollisionEnter", collision);
        }
        void OnCollisionEnter2D(Collision2D collision) {
            DoMessage("OnCollisionEnter2D");
        }
        void OnCollisionExit(Collision collision) {
            DoMessage("OnCollisionExit", collision);
        }
        void OnCollisionExit2D(Collision2D collision) {
            DoMessage("OnCollisionExit2D", collision);
        }
        void OnCollisionStay(Collision collision) {
            DoMessage("OnCollisionStay", collision);
        }
        void OnCollisionStay2D(Collision2D collision) {
            DoMessage("OnCollisionStay2D", collision);
        }
        void OnConnectedToServer() {
            DoMessage("OnConnectedToServer");
        }
        void OnControllerColliderHit(ControllerColliderHit hit) {
            DoMessage("OnControllerColliderHit", hit);
        }
        void OnDestroy() {
            DoMessage("OnDestroy");
        }
        void OnDisable() {
            DoMessage("OnDisable");
        }
        void OnDrawGizmos() {
            DoMessage("OnDrawGizmos");
        }
        void OnDrawGizmosSelected() {
            DoMessage("OnDrawGizmosSelected");
        }
        void OnEnable() {
            DoMessage("OnEnable");
        }
        void OnGUI() {
            DoMessage("OnGUI");
        }
        void OnJointBreak(float breakForce) {
            DoMessage("OnJointBreak", breakForce);
        }
        void OnJointBreak2D(Joint2D brokenJoint) {
            DoMessage("OnJointBreak2D", brokenJoint);
        }
        void OnMouseDown() {
            DoMessage("OnMouseDown");
        }
        void OnMouseDrag() {
            DoMessage("OnMouseDrag");
        }
        void OnMouseEnter() {
            DoMessage("OnMouseEnter");
        }
        void OnMouseExit() {
            DoMessage("OnMouseExit");
        }
        void OnMouseOver() {
            DoMessage("OnMouseOver");
        }
        void OnMouseUp() {
            DoMessage("OnMouseUp");
        }
        void OnMouseUpAsButton() {
            DoMessage("OnMouseUpAsButton");
        }
        void OnParticleCollision(GameObject other) {
            DoMessage("OnParticleCollision", other);
        }
        void OnParticleSystemStopped() {
            DoMessage("OnParticleSystemStopped");
        }
        void OnParticleTrigger() {
            DoMessage("OnParticleTrigger");
        }
        void OnParticleUpdateJobScheduled() {
            DoMessage("OnParticleUpdateJobScheduled");
        }
        void OnPostRender() {
            DoMessage("OnPostRender");
        }
        void OnPreCull() {
            DoMessage("OnPreCull");
        }
        void OnPreRender() {
            DoMessage("OnPreRender");
        }
        void OnRenderImage(RenderTexture src, RenderTexture dest) {
            DoMessage("OnRenderImage", src, dest);
        }
        void OnRenderObject() {
            DoMessage("OnRenderObject");
        }
        void OnTransformChildrenChanged() {
            DoMessage("OnTransformChildrenChanged");
        }
        void OnTransformParentChanged() {
            DoMessage("OnTransformParentChanged");
        }
        void OnTriggerEnter(Collider other) {
            DoMessage("OnTriggerEnter", other);
        }
        void OnTriggerEnter2D(Collider2D other) {
            DoMessage("OnTriggerEnter2D", other);
        }
        void OnTriggerExit(Collider other) {
            DoMessage("OnTriggerExit", other);
        }
        void OnTriggerExit2D(Collider2D other) {
            DoMessage("OnTriggerExit2D", other);
        }
        void OnTriggerStay(Collider other) {
            DoMessage("OnTriggerStay", other);
        }
        void OnTriggerStay2D(Collider2D other) {
            DoMessage("OnTriggerStay2D", other);
        }
        void OnValidate() {
            DoMessage("OnValidate");
        }
        void OnWillRenderObject() {
            DoMessage("OnWillRenderObject");
        }
        void Reset() {
            DoMessage("Reset");
        }
        void Start() {
            DoMessage("Start");
        }
        void Update() {
            DoMessage("Update");
        }
        #endregion
    }
}
