using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightableMaterial
{
	Renderer renderer;
	Material[] m0;
	Material[] m1;

	public HighlightableMaterial(Renderer r, float highlightFactor) {
		renderer = r;
		m0 = renderer.materials;
		m1 = new Material[m0.Length];
		SetColor(highlightFactor);
	}

	public void SetColor(float highlightFactor) {
		for (int i = 0; i < m0.Length; i++) {
			//m1[i] = new Material(m0[i]) { color = (m0[i].color + c) / 2 };
			m1[i] = new Material(m0[i]) { color = m0[i].color * highlightFactor };
		}
	}

	public void Highlight(bool on) {
		if (on) {
			renderer.materials = m1;
		} else {
			renderer.materials = m0;
		}
	}
}
