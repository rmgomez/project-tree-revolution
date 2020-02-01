﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualTileInfo : MonoBehaviour
{
	public Material materialValid;
	public Material materialInvalid;
	public Material materialSelect;
	public Material materialOver;

	private MeshRenderer meshRenderer;

	private void Awake()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		ChangeColor(VisualTileInfos.Hide);
	}

	public void ChangeColor(VisualTileInfos visualTileInfo)
	{
		switch (visualTileInfo)
		{
			case VisualTileInfos.Hide:
				meshRenderer.enabled = false;
				return;

			case VisualTileInfos.Valid:			
				meshRenderer.sharedMaterial = materialValid;
				break;

			case VisualTileInfos.Invalid:
				meshRenderer.sharedMaterial = materialInvalid;
				break;

			case VisualTileInfos.Selected:
				meshRenderer.sharedMaterial = materialSelect;
				break;

			case VisualTileInfos.Over:
				meshRenderer.sharedMaterial = materialOver;
				break;
		}

		meshRenderer.enabled = true;
	}
}
