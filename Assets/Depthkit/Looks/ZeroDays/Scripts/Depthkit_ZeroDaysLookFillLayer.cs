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
    /// <summary>
    /// ZeroDaysLookFillLayer adds a layer to render a transparency enabled DK clip projection.
    /// <summary>
    [System.Serializable]
    [DisallowMultipleComponent]
    public class Depthkit_ZeroDaysLookFillLayer : Depthkit_ZeroDaysLookLayer
    {
        
        /// <summary>
        /// Opacity if the overall fade of the layer
        /// floating point type to expose to Unity timeline.
        /// <summary>
        [Range(0.0f, 1.0f)]
        public float _opacity = .25f;

        /// <summary>
        /// Submits the mesh for rendering
        /// <summary>
        public override void Draw(Matrix4x4 transform, Material material)
        {
            if (_geometryDirty || _mesh == null)
            {
                BuildMesh();
                _geometryDirty = false;
            }

            // even if opacity is 0, render to depth incase layers above require depth occlusion
            if (isActiveAndEnabled && _opacity > 0.0f)
            {
                //Push all the items into material property block
                //We use this technique in order to the share the material between all the different layers
                if (_materialBlock == null)
                {
                    _materialBlock = new MaterialPropertyBlock();
                }
                else
                {
                    _materialBlock.Clear();
                }

                _materialBlock.SetFloat("_Opacity", _opacity);
                Graphics.DrawMesh(_mesh, transform, material, 0, null, 0, _materialBlock);
            }
        }

        /// <summary>
        /// Gets the default shader name for this layer
        /// <summary>
        protected override string DefaultShaderString()
        {
            return "Depthkit/ZeroDaysLookFill";
        }

        /// <summary>
        /// Rebuilds the mesh with the current settings
        /// <summary>
        public void BuildMesh()
        {
            if (_mesh == null) {
                _mesh = new Mesh ();
            }

            ParentRenderer.GetMeshLattice (ref _mesh);
        }
   }
}