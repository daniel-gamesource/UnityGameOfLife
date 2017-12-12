using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules : MonoBehaviour {

	public List<Item> itemsToTest; //letf id public to show on debug
	private List<Box> checkedBoxes;
	private GameResourcesManager grm;

	void Start(){
		grm = transform.GetComponent<GameResourcesManager> ();
	}

	//class that will be used to test the rules
	private class Item {
		public Box box;
		public bool isTrueNextStep = false;

		public Item (Box box){
			this.box = box;
			isTrueNextStep = false;
		}
	}

	//Execute one step
	public void doStep(){
		itemsToTest = new List<Item> ();
		findSteps();
	//	executeSteps();
	}

	private void findSteps (){
		if (checkedBoxes != null) {
			checkedBoxes = grm.checkedBoxes;

			foreach (Box box in checkedBoxes) {
				addItemToList (box);
			}
				
		}

	//	checkedItems = 
	}

	private void addItemToList(Box box){
		Item item = new Item (box);
		if(!itemsToTest.Exists (x => (x.box.positionX == box.positionX &&
								      x.box.positionY == box.positionY))){
			itemsToTest.Add(item);
		}
	}

	private List<Box> getBoxNeighbors(Box box){
	}

	//RULES

	//Rule 1
	//for a space that is populated, each cell with one or no neighbors dies

	//Rule 2
	//for a space that is populated, with four or more neighbors dies

	//Rule 3
	//for a space that is populated, with two or three neighbors is keept

	//Rules 4
	//for a space that is not populated, each cell with three neighbors becomes populated
}