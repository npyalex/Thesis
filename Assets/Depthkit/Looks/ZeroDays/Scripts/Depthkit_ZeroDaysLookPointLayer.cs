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
    /// ZeroDaysLookPointLayer adds a layer of point based effect to the model
    /// <summary>
    [System.Serializable]
    public class Depthkit_ZeroDaysLookPointLayer : Depthkit_ZeroDaysLookLayer
    {

        /// <summary>
        /// How many columns of points are drawn
        /// floating point type to expose to Unity timeline.
        /// <summary>
        [Range(0, 255)]
        public float _pointColumns = 200;
        private float _prevPointColumns = -1;

        /// <summary>
        /// How many rows of points are drawn
        /// <summary>
        [Range(0, 255)]
        public float _pointRows = 100;
        private float _prevPointRows = -1;

        /// <summary>
        /// Opacity if the overall fade of the layer
        /// <summary>
        [Range(0.0f, 1.0f)]
        public float _opacity = 1.0f;

        private const float POINT_SIZE_MAX = 50.0f;
        private const float POINT_MM_TO_METERS = 1000.0f;

        /// <summary>
        /// Base size of the points before aspect bias is applied
        /// <summary>

        [Range(0.0f, POINT_SIZE_MAX)]
        public float _pointSize = 12.0f;

        /// <summary>
        /// Aspect ratio of the points. Bias towards -1 makes them tall and skinny, bias towards 1 makes them long and flat 
        /// <summary>
        [Range(-1.0f, 1.0f)]
        public float _pointAspect = 0.0f;

        /// <summary>
        /// Line Sprite is a texture that is spread across the line
        /// <summary>
        public Texture2D _pointSprite;

        /// <summary>
        /// Submits the mesh for rendering
        /// <summary>
        public override void Draw(Matrix4x4 transform, Material material)
        {
            if (_geometryDirty ||
                _mesh == null ||
                _pointColumns != _prevPointColumns ||
                _pointRows != _prevPointRows)
            {
                BuildMesh();
                _geometryDirty = false;
                _prevPointColumns = _pointColumns;
                _prevPointRows = _pointRows;
            }

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

                if (_pointSprite != null)
                {
                    _materialBlock.SetTexture("_Sprite", _pointSprite);
                    _materialBlock.SetFloat("_UseSprite", 1.0f);
                }
                else
                {
                    _materialBlock.SetFloat("_UseSprite", 0.0f);
                }

                //Apply exponential curve
                float pointSize = _pointSize / POINT_SIZE_MAX;
                pointSize = Mathf.Pow(pointSize, 2.0f);
                pointSize *= POINT_SIZE_MAX;

                float pointWidth  = pointSize * (_pointAspect < 0 ? Mathf.Pow(1.0f + _pointAspect, 2.0f) : 1.0f);
                float pointHeight = pointSize * (_pointAspect > 0 ? Mathf.Pow(1.0f - _pointAspect, 2.0f) : 1.0f);
                //No need to check neighbors
                _materialBlock.SetVector("_MeshScalar", new Vector4(0f, 0f, 0f, 0f));
                _materialBlock.SetFloat("_Width",  (pointWidth  / POINT_MM_TO_METERS) * gameObject.transform.localScale.x);
                _materialBlock.SetFloat("_Height", (pointHeight / POINT_MM_TO_METERS) * gameObject.transform.localScale.y);
                _materialBlock.SetFloat("_Opacity", _opacity);

                Graphics.DrawMesh(_mesh, transform, material, 0, null, 0, _materialBlock);
            }
        }

        /// <summary>
        /// Rebuilds the mesh with the current settings
        /// <summary>
        void BuildMesh()
        {
            if (_mesh == null)
            {
                _mesh = new Mesh();
            }
            else
            {
                _mesh.Clear();
            }

            ///////// Build Points /////////
            _mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            Vector3[] verts = new Vector3[(int)_pointColumns * (int)_pointRows];
            int[] indices = new int[(int)_pointColumns * (int)_pointRows];

            int curIndex = 0;
            Vector2 textureStep = new Vector2(1.0f / _pointColumns, 1.0f / _pointRows);

            for (int y = 0; y < (int)_pointRows; y++)
            {
                for (int x = 0; x < (int)_pointColumns; x++)
                {
                    indices[curIndex] = curIndex;

                    verts[curIndex].x = x * textureStep.x;
                    verts[curIndex].y = y * textureStep.y;
                    verts[curIndex].z = 0;

                    curIndex++;
                }
            }

            _mesh.vertices = verts;
            _mesh.SetIndices(indices, MeshTopology.Points, 0);
            _mesh.bounds = ParentRenderer.Bounds;
        }

        /// <summary>
        /// Gets the default shader name for this layer
        /// <summary>
        protected override string DefaultShaderString()
        {
            return "Depthkit/ZeroDaysLookPoints";
        }
    }
}
