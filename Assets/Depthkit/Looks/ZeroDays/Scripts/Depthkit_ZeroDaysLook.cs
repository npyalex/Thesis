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
using System.Collections.Generic;

namespace Depthkit
{
    [DisallowMultipleComponent]
    public class Depthkit_ZeroDaysLook : Depthkit_ClipRenderer
    {
        public struct ShaderBlendMode
        {
            public Shader s;
            public Depthkit_ZeroDaysLook.BlendMode m;
            public ShaderBlendMode(Shader s, Depthkit_ZeroDaysLook.BlendMode m)
            {
                this.s = s;
                this.m = m;
            }
        };

        public Dictionary<ShaderBlendMode, Material> _materials;

        //Indicates if the script is being loaded for the first time. Adds default layers
        [SerializeField, HideInInspector]
        private bool _initialized = false;

        private bool _layersDirty = false;

        private Depthkit_ZeroDaysLookOccludeLayer _occludeLayer;
        private Depthkit_ZeroDaysLookFillLayer _fillLayer;
        private Depthkit_ZeroDaysLookLayer[] _layers;

        /// <summary>
        /// Self occlusion property so the Zero Days Look doesn't blend with itself
        /// <summary>
        public bool _selfOcclusion = true;

        /// <summary>
        /// returns the DepthkitSDK version this look was released against
        /// <summary>
        public override Version GetSDKVersion()
        {
            return new Version(0, 2, 6);
        }

        /// <summary>
        /// returns the version of this look
        /// <summary>
        public override Version GetVersion()
        {
            return new Version(0, 1, 0);
        }

        public override RenderType GetRenderType()
        {
#if DK_USING_ZERODAYSLOOK
            return RenderType.ZeroDays;
#else
            return (RenderType)1;
#endif
        }
        
        public void Start()
        {
            SetLayersDirty();
            CreateRequiredLayers();
        }

        public void OnEnable()
        {
            SetLayersDirty();
        }

        public override void SetGeometryDirty()
        {
            base.SetGeometryDirty ();

            if (_layers != null)
            {
                foreach (Depthkit_ZeroDaysLookLayer layer in _layers)
                {
                    if (layer != null)
                    {
                        layer.GeometryDirty = true;
                    }
                }
            }
        }

        public void SetLayersDirty()
        {
            _layersDirty = true;
            _materialDirty = true;
        }

        public override void Draw()
        {
            CreateRequiredLayers();

            if (Texture == null)
            {
                //Don't render if the parent texture is null
                return;
            }

            _occludeLayer.enabled = _selfOcclusion;
            if (_layersDirty || _layers == null)
            {
                _layers = GetComponentsInChildren<Depthkit_ZeroDaysLookLayer>();
                if (_layers == null)
                {
                    Debug.LogError("No Layers Found");
                    return;
                }

                _layersDirty = false;
                _materialDirty = true;
            }

            //build materials if shaders are not set or have changed
            if (_materialDirty || _materials == null)
            {
                BuildMaterials();
                _materialDirty = false;
            }

            //draw the layers
            Matrix4x4 transformmat = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Material m;

            if (_layers != null)
            {
                foreach (Depthkit_ZeroDaysLookLayer layer in _layers)
                {
                    if (layer == null)
                    {
                        continue;
                    }

                    if (_metadataChanged)
                    {
                        layer.SetBounds(_bounds);
                    }

                    ShaderBlendMode materialKey = new ShaderBlendMode(layer.Shader, layer.BlendMode);
                    if (_materials.ContainsKey(materialKey))
                    {
                        m = _materials[materialKey];
                        SetMaterialProperties(m);
                        layer.Draw(transformmat, m);
                    }
                    else
                    {
                        Debug.LogError("Blending Mode / Shader combo not found in Materials: " + layer.Shader + " Blend Mode: " + layer.BlendMode);
                        _materialDirty = true;
                    }
                }

                _metadataChanged = false;

            }

        }

        void Update()
        {
            Draw();
        }

        protected void CreateRequiredLayers()
        {
            _occludeLayer = GetComponent<Depthkit_ZeroDaysLookOccludeLayer>();
            bool addedLayer = false;
            if(_occludeLayer == null)
            {
                _occludeLayer = gameObject.AddComponent<Depthkit_ZeroDaysLookOccludeLayer>();
                _occludeLayer.hideFlags = HideFlags.HideInInspector;
                addedLayer = true;
            }

            _fillLayer = GetComponent<Depthkit_ZeroDaysLookFillLayer>();
            if (_fillLayer == null)
            {
                _fillLayer = gameObject.AddComponent<Depthkit_ZeroDaysLookFillLayer>();

                addedLayer = true;
            }

            //Add default layer look
            if(!_initialized)
            {
                _initialized = true;

                Depthkit_ZeroDaysLookLineLayer lineLayer = gameObject.AddComponent<Depthkit_ZeroDaysLookLineLayer>();
                Depthkit_ZeroDaysLookPointLayer pointLayer = gameObject.AddComponent<Depthkit_ZeroDaysLookPointLayer>();
                lineLayer.BlendMode = BlendMode.Screen;
                pointLayer.BlendMode = BlendMode.Screen;

                addedLayer = true;
            }

            if(addedLayer)
            {
                SetLayersDirty();
            }
        }

        protected void BuildMaterials()
        {

            //Create materials for each blending mode
            //Blending modes cannot be changed by MaterialPropertyBlocks,
            //so we need to have a separate material to pass to each layer based on its setting

            _materials = new Dictionary<ShaderBlendMode, Material>();
            HashSet<Shader> layerShaders = new HashSet<Shader>();
            foreach (Depthkit_ZeroDaysLookLayer layer in _layers)
            {
                if (layer != null && layer.Shader != null)
                {
                    layerShaders.Add(layer.Shader);
                }
            }

            foreach (BlendMode mode in BlendMode.GetValues(typeof(BlendMode)))
            {
                foreach (Shader shader in layerShaders)
                {
                    ShaderBlendMode sbm = new ShaderBlendMode(shader, mode);

                    Material mat = new Material(shader);
                    mat.SetInt("_SrcMode", (int)GetSrcMode(mode));
                    mat.SetInt("_DstMode", (int)GetDstMode(mode));
                    mat.SetInt("_BlendEnum", (int)mode);

                    _materials.Add(sbm, mat);
                }
            }
        }

        public void AddLineLayer()
        {
            gameObject.AddComponent<Depthkit_ZeroDaysLookLineLayer>();
        }

        public void AddPointLayer()
        {
            gameObject.AddComponent<Depthkit_ZeroDaysLookPointLayer>();
        }

        public void OnValidate()
        {
            if (_layers != null)
            {
                foreach (Depthkit_ZeroDaysLookLayer layer in _layers)
                {
                    if (layer != null)
                    {
                        layer.OnValidate();
                    }
                }
            }
        }

        public override void RemoveComponents()
        {
            if (_layers != null)
            {
                foreach (Depthkit_ZeroDaysLookLayer layer in _layers)
                {
                    if (layer != null)
                    {
                        if (!Application.isPlaying)
                        {
                            DestroyImmediate(layer, false);
                        }
                        else
                        {
                            Destroy(layer);
                        }
                    }
                }
            }

            if (!Application.isPlaying)
            {
                DestroyImmediate(this, false);
            }
            else
            {
                Destroy(this);
            }
        }

        //BLENDING MODES
        public enum BlendMode
        {
            Alpha = 0,  // Premultiplied transparency Blend SrcAlpha OneMinusSrcAlpha 
            Add = 1,    // Additive Blend One One 
            Multiply = 2,   // Multiplicative Blend DstColor Zero
            Screen = 3  // Soft Additive Blend One OneMinusDstColor  
        }

        //Fetch appropriate Src/Dst modes for blending modes above
        private static UnityEngine.Rendering.BlendMode GetSrcMode(BlendMode mode)
        {
            switch (mode)
            {
                case BlendMode.Alpha:
                    return UnityEngine.Rendering.BlendMode.SrcAlpha;

                case BlendMode.Add:
                    return UnityEngine.Rendering.BlendMode.One;

                case BlendMode.Multiply:
                    return UnityEngine.Rendering.BlendMode.DstColor;

                case BlendMode.Screen:
                    return UnityEngine.Rendering.BlendMode.One;

                default:
                    return UnityEngine.Rendering.BlendMode.SrcAlpha;
            }
        }

        private static UnityEngine.Rendering.BlendMode GetDstMode(BlendMode mode)
        {
            switch (mode)
            {
                case BlendMode.Alpha:
                    return UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;

                case BlendMode.Add:
                    return UnityEngine.Rendering.BlendMode.One;

                case BlendMode.Multiply:
                    return UnityEngine.Rendering.BlendMode.Zero;

                case BlendMode.Screen:
                    return UnityEngine.Rendering.BlendMode.OneMinusSrcColor;

                default:
                    return UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
            }
        }
    }
}