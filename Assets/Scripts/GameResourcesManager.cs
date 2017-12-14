using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResourcesManager : MonoBehaviour {

	public int quantityOfCollumns = 10;
	public int quantityOfLines = 10;

	//visual object that will represent the box
	public Toggle box;
	public List<Box> checkedBoxes = null; 

	// Use this for initialization
	void Start () {
		cloneBoxes ();
		checkedBoxes = new List<Box>();
	}

	private void cloneBoxes(){
		float boxHeight = 0f;
		float boxWidth = 0f;

		for (int lineCounter = 0; lineCounter < quantityOfLines; lineCounter++) {

			for (int collCounter = 0; collCounter < quantityOfCollumns; collCounter++) {
				//boxWidth is positive to clone for the right side
				//boxHeight is negative to clone for the down side
				Toggle instanceBox = Instantiate (box, box.transform.position + new Vector3 (boxWidth, boxHeight, 0f), box.transform.rotation);
				instanceBox.name = string.Concat("Y", lineCounter, "X",  collCounter);//using string.Concat instead of "" + "" for peformance
				instanceBox.transform.SetParent (box.transform.parent);

				//add the box info
				Box newBox = instanceBox.GetComponent<Box>();

				newBox.positionX = collCounter; 
				newBox.positionY = lineCounter;

				boxWidth += box.graphic.rectTransform.rect.width;

			}
			//reset the box size
			boxWidth = 0f;
			boxHeight -= box.graphic.rectTransform.rect.height;
		}

		//we need to resize the box holder to align the items on the center of the screen
		resizeBoxHolder(box.graphic.rectTransform.rect.width,  box.graphic.rectTransform.rect.height);

		//disable the box
		box.gameObject.SetActive(false);
	}

	private void resizeBoxHolder(float boxWidth, float boxHeight){
		RectTransform rt = transform.GetComponent (typeof (RectTransform)) as RectTransform;
		rt.sizeDelta = new Vector2 (boxWidth * quantityOfCollumns, boxHeight * quantityOfLines );//multiply with scale to rezise correctly
	}

	public void addCheckedBox(Box box){
		Toggle toggle = box.GetComponent<Toggle>();
	
		if (toggle.isOn && 
			!checkedBoxes.Exists (x => (x.positionX == box.positionX &&
				x.positionY == box.positionY))) {
			box.isChecked = true;
			checkedBoxes.Add (box);
		} else if (!toggle.isOn) {
			box.isChecked = false;
			checkedBoxes.Remove (box);
		}
	}
}
