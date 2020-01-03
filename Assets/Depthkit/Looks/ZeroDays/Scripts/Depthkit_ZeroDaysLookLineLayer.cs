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
    /// ZeroDaysLookLineLayer adds a layer of line based effect to the model
    /// <summary>
    [System.Serializable]
    public class Depthkit_ZeroDaysLookLineLayer : Depthkit_ZeroDaysLookLayer
    {
        public enum LineOrientation
        {
            Vertical,
            Horizontal
        };

        /// <summary>
        /// Orientation switches between horizontal and vertical lines.
        /// <summary>
        public LineOrientation _orientation = LineOrientation.Vertical;
        private LineOrientation _prevOrientation = LineOrientation.Horizontal;

        /// <summary>
        /// Line Count controls the number of lines in this layer
        /// floating point type to expose to Unity timeline.
        /// <summary>
        [Range(0, 255)]
        public float _lineCount = 100;
        private float _prevLineCount = -1;

        /// <summary>
        /// Line Density controls how many individual line elements make up each line.
        /// Higher numbers means smoother lines but more expensive rendering
        /// floating point type to expose to Unity timeline.
        /// <summary>
        [Range(1, 255)]
        public float _lineDensity = 100;
        private float _prevLineDensity = -1;

        /// <summary>
        /// Opacity if the overall fade of the layer
        /// floating point type to expose to Unity timeline.
        /// <summary>
        [Range(0.0f, 1.0f)]
        public float _opacity = .75f;

        private const float LINE_WIDTH_MAX = 50.0f;
        private const float LINE_MM_TO_METERS = 1000.0f;

        /// <summary>
        /// Width thickness of the lines on screen
        /// floating point type to expose to Unity timeline.
        /// <summary>
        [Range(0.0f, LINE_WIDTH_MAX)]
        public float _width = 6.0f;

        /// <summary>
        /// Line Sprite is a texture that is spread across the line
        /// <summary>
        public Texture2D _lineSprite;

        /// <summary>
        /// Submits the mesh for rendering
        /// <summary>
        public override void Draw(Matrix4x4 transform, Material material)
        {
            if (_mesh == null ||
                _geometryDirty ||
                _prevLineCount != _lineCount ||
                _prevLineDensity != _lineDensity ||
                _prevOrientation != _orientation)
            {
                BuildMesh();
                _geometryDirty = false;
                _prevLineCount = _lineCount;
                _prevLineDensity = _lineDensity;
                _prevOrientation = _orientation;
            }

            //Can't set stride variables without parent texture
            if (ParentRenderer.Texture == null)
            {
                return;
            }

            if (isActiveAndEnabled && _opacity > 0.0f)
            {
                // push all the items into material property block
                // we use this technique in order to the share the material between all the different layers
                if (_materialBlock == null)
                {
                    _materialBlock = new MaterialPropertyBlock();
                }
                else
                {
                    _materialBlock.Clear();
                }

                //For the line layer, the sampling schema for recovery of nil depth values from neighbors needs
                //to match the strides of the lines (density and count) and is swapped dependening on the orientation.
                if (_orientation == LineOrientation.Horizontal)
                {
                    _materialBlock.SetVector("_MeshScalar", new Vector4( (float)ParentRenderer.Texture.width / (int)_lineDensity, 0.0f, 0.0f, 0.0f));
                }
                else
                {
                    _materialBlock.SetVector("_MeshScalar", new Vector4(0.0f, ((float)ParentRenderer.Texture.height * .5f) / (int)_lineDensity, 0.0f, 0.0f));
                }

                if (_lineSprite != null)
                {
                    _materialBlock.SetTexture("_Sprite", _lineSprite);
                    _materialBlock.SetFloat("_UseSprite", 1.0f);
                }
                else
                {
                    _materialBlock.SetFloat("_UseSprite", 0.0f);
                }

                //Apply exponential curve
                float lineWidth = _width / LINE_WIDTH_MAX;
                lineWidth = Mathf.Pow(lineWidth, 2.0f);
                lineWidth *= LINE_WIDTH_MAX;

                //lines need to scale with the object based on their orientation.
                float scaledWidth = lineWidth / LINE_MM_TO_METERS * (_orientation == LineOrientation.Horizontal ? gameObject.transform.localScale.y : gameObject.transform.localScale.x);
                _materialBlock.SetFloat("_Width", scaledWidth);
                _materialBlock.SetFloat("_Opacity", _opacity);
                _materialBlock.SetFloat("_LineOrientation", (float)_orientation);

                Graphics.DrawMesh(_mesh, transform, material, 0, null, 0, _materialBlock);
            }
        }

        /// <summary>
        /// Rebuilds the mesh with the current settings
        /// <summary>
        protected void BuildMesh()
        {
            if (_mesh == null)
            {
                _mesh = new Mesh();
            }
            else
            {
                _mesh.Clear();
            }

            ///////// Build Lines /////////
            _mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            int lineDensity = (int)_lineDensity;
            int lineCount = (int)_lineCount;

            int vertsPerLine = lineDensity + 1;
			int totalVerts = vertsPerLine * lineCount;
			int indicesPerLine = vertsPerLine * 2 - 2;

			Vector3[] verts = new Vector3[totalVerts];
            
			int[] indices = new int[ indicesPerLine * lineCount ];

			// How to compute the indexing on each line
			/// eg:
			/// line count = 3
			/// line density = 3
			/// line verts = 4
			/// indices per line = (4 * 2 - 2) = 6

			// ln  0
			// vt  0-----1-----2----3
			// ix  0    1,2   3,4   5

			// ln  1   
			// vt  4-----5-----6----7
			// ix  6    7,8   9,10 11

			// ln  2  
			// vt  8-----9----10----11
			// ix 12   13,14 15,16  17

            // swap texture step size based on density
            Vector2 textureStep = new Vector2(1.0f / lineDensity, 1.0f / lineCount);

            for (int line = 0; line < lineCount; line++)
            {

				//add first line index
				int lineVertexStart = line * vertsPerLine;
				int lineIndexStart  = line * indicesPerLine;
				indices[lineIndexStart] = lineVertexStart;

				for (int step = 0; step < vertsPerLine; step++)
                {
                    Vector2 segment = new Vector2((float)step * textureStep.x, (float)line * textureStep.y);
					int curIndex = lineVertexStart+step;
					verts[curIndex].x = _orientation == LineOrientation.Vertical ? segment.y : segment.x;
                    verts[curIndex].y = _orientation == LineOrientation.Vertical ? segment.x : segment.y;
                    verts[curIndex].z = 0;

					//only add indices in the loop when we are not on the line edges
					if(step != 0 && step != vertsPerLine-1) {
						indices[lineIndexStart+step*2-1] = curIndex;
						indices[lineIndexStart+step*2  ] = curIndex;
					}
                }

				//add end index
				int nextLine = line+1;
				indices[nextLine * indicesPerLine - 1] = nextLine * vertsPerLine - 1;

            }

            _mesh.vertices = verts;
            _mesh.SetIndices(indices, MeshTopology.Lines, 0);
            _mesh.bounds = ParentRenderer.Bounds;
        }

        /// <summary>
        /// Gets the default shader name for this layer
        /// <summary>
        protected override string DefaultShaderString()
        {
            return "Depthkit/ZeroDaysLookLines";
        }
    }
}