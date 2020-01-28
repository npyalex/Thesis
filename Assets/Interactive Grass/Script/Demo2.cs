using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo2 : MonoBehaviour
{
	public GameObject m_Grass;
	public GameObject m_Character;
	public KeyCode m_ForwardButton = KeyCode.UpArrow;
	public KeyCode m_BackwardButton = KeyCode.DownArrow;
	public KeyCode m_RightButton = KeyCode.RightArrow;
	public KeyCode m_LeftButton = KeyCode.LeftArrow;
	Renderer m_GrassRd;
	
	[Header("Grass")]
	[Range(4, 16)] public int m_Tess = 9;
	[Range(0.05f, 1f)] public float m_WindStrength = 0.4f;
	public float m_MoveForceIncrease = 0.1f;
	Vector2 m_MoveForce;
	int m_MoveForceState = 0;
	
	void Start ()
	{
		QualitySettings.antiAliasing = 8;
		m_GrassRd = m_Grass.GetComponent<Renderer> ();
	}
	void Update ()
	{
		Vector3 dir = Vector3.zero;
		Move (m_ForwardButton, ref dir, m_Character.transform.forward);
		Move (m_BackwardButton, ref dir, -m_Character.transform.forward);
		Move (m_RightButton, ref dir, m_Character.transform.right);
		Move (m_LeftButton, ref dir, -m_Character.transform.right);
		m_Character.transform.position += dir * 4f * Time.deltaTime;

		m_GrassRd.material.SetFloat ("_TessellationUniform", m_Tess);
		m_GrassRd.material.SetFloat ("_WindStrength", m_WindStrength);

		if (m_MoveForceState == 1)
		{
			if (dir.x < 0f)
			{
				m_MoveForce.x -= 0.1f;
				m_MoveForce.x = (m_MoveForce.x < -1f) ? -1f : m_MoveForce.x;
			}
			if (dir.x > 0f)
			{
				m_MoveForce.x += 0.1f;
				m_MoveForce.x = (m_MoveForce.x > 1f) ? 1f : m_MoveForce.x;
			}
			if (dir.z < 0f)
			{
				m_MoveForce.y -= 0.1f;
				m_MoveForce.y = (m_MoveForce.y < -1f) ? -1f : m_MoveForce.y;
			}
			if (dir.z > 0f)
			{
				m_MoveForce.y += 0.1f;
				m_MoveForce.y = (m_MoveForce.y > 1f) ? 1f : m_MoveForce.y;
			}
		}
		if (m_MoveForceState == 2)
		{
			if (m_MoveForce.x < 0f)
			{
				m_MoveForce.x += 0.02f;
				m_MoveForce.x = (m_MoveForce.x > 0f) ? 0f : m_MoveForce.x;
			}
			else if (m_MoveForce.x > 0f)
			{
				m_MoveForce.x -= 0.02f;
				m_MoveForce.x = (m_MoveForce.x < 0f) ? 0f : m_MoveForce.x;
			}
			if (m_MoveForce.y < 0f)
			{
				m_MoveForce.y += 0.02f;
				m_MoveForce.y = (m_MoveForce.y > 0f) ? 0f : m_MoveForce.y;
			}
			else if (m_MoveForce.y > 0f)
			{
				m_MoveForce.y -= 0.02f;
				m_MoveForce.y = (m_MoveForce.y < 0f) ? 0f : m_MoveForce.y;
			}
		}
		
		float x = m_Character.transform.position.x;
		float z = m_Character.transform.position.z;
		float dirx = m_MoveForce.x;
		float dirz = m_MoveForce.y;
		m_GrassRd.material.SetVector ("_ForceParam", new Vector4 (dirx, dirz, x, z));
	}
	void Move (KeyCode key, ref Vector3 moveTo, Vector3 dir)
	{
		if (Input.GetKey (key))
		{
			moveTo = dir;
			m_MoveForceState = 1;
		}
		if (Input.GetKeyUp (key))
		{
			m_MoveForceState = 2;
		}
	}
}