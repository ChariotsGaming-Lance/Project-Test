using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectExpander : MonoBehaviour
{
	[Header("Customizables")]
	[SerializeField]
	private float maxScaleMultiplier = 1.5f;
    [Header("Monitoring Purpose")]
    [SerializeField]
    private float currentScale = 0f;
	[SerializeField]
	private float maxPoints = 0f;
	[SerializeField]
	private float minScale = 0f;
	[SerializeField]
	private float maxScale = 0f;
	[SerializeField]
	private float totalScale = 0f;
	[SerializeField]
	private float scaleCurrency = 0f;

	public void Init(int maxPoints)
	{
		this.maxPoints = maxPoints;
        currentScale = transform.localScale.x;
        minScale = currentScale;
        maxScale = currentScale * maxScaleMultiplier;
		totalScale = maxScale - minScale;
		scaleCurrency = totalScale / maxPoints;
	}
    
    public void Expand(int points)
	{
        if (currentScale < maxScale)
        {
            currentScale += points * scaleCurrency;
            transform.localScale += new Vector3(points * scaleCurrency, points * scaleCurrency, points * scaleCurrency);
        }
	}

    public void Shrink(int points)
    {
        if (currentScale > minScale)
        {
            currentScale -= points * scaleCurrency;
            transform.localScale -= new Vector3(points * scaleCurrency, points * scaleCurrency, points * scaleCurrency);
        }
    }
}
