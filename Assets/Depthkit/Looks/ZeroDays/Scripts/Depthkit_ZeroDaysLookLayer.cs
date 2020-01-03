/************************************************************************************

Depthkit Unity SDK License v1
Copyright 2016-2018 Scatter All Rights reserved.  

Licensed under the Scatter Software Development Kit License Agreement (the "License"); 
you may not use this SDK except in compliance with the License, 
which is provided at the time of installation or download, 
or which otherwise accompanies this software in either electronic or hard copy form.  

You may obtain a copy of the License at http://www.depthkit.tv/license-agreement-v1

Unless required by applicable law or agreed to in writing, 
the SDK distributed under the License is distributed on an "AS IS" BASIS, 
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
See the License for the specific language governing permissions and limitations under the License. 

************************************************************************************/

using UnityEngine;
using System.Collections;

namespace Depthkit
{
    [ExecuteInEditMode]
    [System.Serializable]
    abstract public class Depthkit_ZeroDaysLookLayer : MonoBehaviour
    {
        protected bool _initialized = false;
        protected Mesh _mesh;
        protected bool _geometryDirty = true;
        public bool GeometryDirty
        {
            get { return _geometryDirty; }
            set { _geometryDirty = value; }
        }

        protected MaterialPropertyBlock _materialBlock;

        [SerializeField]
        [HideInInspector]
        private Depthkit_ZeroDaysLook _parentRenderer;
        protected Depthkit_ZeroDaysLook ParentRenderer
        {
            get {
                if(_parentRenderer == null)
                {
                    _parentRenderer = GetComponent<Depthkit_ZeroDaysLook>();
                    if(_parentRenderer == null)
                    {
                        Debug.LogError("Zero Days Look Layer added to component without Zero Days Look.");
                    }
                }
                return _parentRenderer;
            }
        }
        /// <summary>
        /// Set the shader for this layer. By default this loads built in shader path
        /// implemented by the subclass
        /// <summary>
        [SerializeField]
        protected Shader _shader;
        public Shader Shader
        {
            get { return _shader; }
            set
            {
                _shader = value;
                ParentRenderer.SetMaterialDirty();
            }
        }

        /// <summary>
        /// Blending Mode for this layer
        /// <summary>
        [SerializeField]
        private Depthkit_ZeroDaysLook.BlendMode _blendMode = Depthkit_ZeroDaysLook.BlendMode.Alpha;
        public Depthkit_ZeroDaysLook.BlendMode BlendMode
        {
            get { return _blendMode; }
            set { _blendMode = value; }
        }

        void Update()
        {
            if (!_initialized && ParentRenderer.Texture != null && ParentRenderer.Metadata != null)
            {
                _initialized = true;
                Init();
            }
        }
            
        protected void OnEnable()
        {
            _initialized = false;
        }

        protected void Init()
        {
            #if UNITY_EDITOR
            if (this.Shader == null)
            {
                this.Shader = Shader.Find(DefaultShaderString());
            }
            #endif

            _geometryDirty = true;
            ParentRenderer.SetLayersDirty();
            ParentRenderer.Draw();
        }

        public void SetBounds(Bounds bounds)
        {
            if (_mesh != null)
            {
                _mesh.bounds = bounds;
            }
        }

        public void OnValidate()
        {
            #if UNITY_EDITOR
            if (this.Shader == null)
            {
                this.Shader = Shader.Find(DefaultShaderString());
            }
            #endif

            ParentRenderer.SetMaterialDirty();
        }

        void OnDestroy()
        {
            ParentRenderer.SetLayersDirty();
        }

        abstract public void Draw(Matrix4x4 transform, Material material);
        abstract protected string DefaultShaderString();
    }
}