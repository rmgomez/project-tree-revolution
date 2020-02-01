using System.Collections;
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
		ChangeColor(VisualTileInfos.Hide, false);
	}

	private VisualTileInfos visualTileStateSaved = VisualTileInfos.Hide;

	public void ChangeColor(VisualTileInfos visualTileInfo, bool isMouse)
	{
		switch (visualTileInfo)
		{
			case VisualTileInfos.Hide:

				if (isMouse)
				{
					ChangeColor(visualTileStateSaved, false);
				}
				else
				{
					meshRenderer.enabled = false;
					visualTileStateSaved = visualTileInfo;
				}

				return;

			case VisualTileInfos.Valid:			
				meshRenderer.sharedMaterial = materialValid;
				visualTileStateSaved = visualTileInfo;
				break;

			case VisualTileInfos.Invalid:
				meshRenderer.sharedMaterial = materialInvalid;
				visualTileStateSaved = visualTileInfo;
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
