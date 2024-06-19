using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Touchable : MonoBehaviour {

	public Sprite normalSprite;
	public Sprite activeSprite;

	[HideInInspector]
	public Vector3 vecScale = Vector3.one;
	[HideInInspector]
	public Image image;
	[HideInInspector]
	public BoxCollider2D _collider;

	void Awake () {
        _collider = GetComponent<BoxCollider2D> ();
		image = GetComponent<Image> ();
	}
	
	public virtual bool checkTouch(Vector2 pos) {
		return (_collider.OverlapPoint (pos));
	}

	public virtual void scaleImage(float scale) {
		vecScale.Set (scale, scale, 1);
		image.rectTransform.localScale = vecScale;
	}

	public virtual void setActive(bool val) {
		if (val) {
			image.sprite = activeSprite;
		} else {
			image.sprite = normalSprite;
		}
        if (image.sprite) image.SetNativeSize();
    }

	public virtual bool isActive() {
		return (image.sprite == activeSprite);
	}
		
}
