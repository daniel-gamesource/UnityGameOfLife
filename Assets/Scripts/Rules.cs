using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Rules : MonoBehaviour {

	private List<Item> itemsToTest; //letf id public to show on debug
	private List<Box> checkedBoxes;
	private GameResourcesManager grm;

	void Start(){
		grm = transform.GetComponent<GameResourcesManager> ();
	}

	//class that will be used to test the rules
	private class Item : IEquatable<Item>{
		public Box itemBox;
		public bool isTrueNextStep = false;

		public Item (Box box){
			this.itemBox = box;
			isTrueNextStep = false;
		}

		//IEquatable override methods
		public override bool Equals(object obj)
		{
			if (obj == null) {
				return false;
			}

			Item objAsItem = obj as Item;

			if (objAsItem == null) {
				return false;
			} else {
				return Equals (objAsItem);
			}
		}

		//copy the hash from the box.
		public override int GetHashCode()
		{
			return itemBox.GetHashCode();
		}

		public bool Equals(Item other)
		{
			if (other == null) {
				return false;
			}
			return this.itemBox.Equals(other.itemBox);
		}
	}



	//Execute Steps


	//Execute one step
	public void doStep(){
		itemsToTest = new List<Item> ();
		findSteps();
		Debug.Log ("counter:" + itemsToTest.Count);
	//	executeSteps();
	}

	private void findSteps (){
		checkedBoxes = grm.checkedBoxes;
		if (checkedBoxes != null) {

			foreach (Box box in checkedBoxes) {
				addItemToList (box);
				getBoxNeighbors (box);
			}
				
		}

	}

	private void addItemToList(Box box){
		Item item = new Item (box);
		if(!itemsToTest.Exists (x => (x.itemBox.positionX == box.positionX &&
									  x.itemBox.positionY == box.positionY))){
			itemsToTest.Add(item);
		}
	}

	private void getBoxNeighbors(Box box){
		int tempPositionX = box.positionX;
		int tempPositionY = box.positionY;

		if(tempPositionY -1 >= 0 && tempPositionX -1 >= 0)
			addItemToList(transform.Find ("BOX" + (tempPositionY -1) + "" + (tempPositionX -1)).GetComponent<Box>());
		if(tempPositionY -1 >= 0)
			addItemToList(transform.Find ("BOX" + (tempPositionY -1) + "" + tempPositionX).GetComponent<Box>());
		if(tempPositionY -1 >= 0 && tempPositionX +1 < grm.quantityOfCollumns)
			addItemToList(transform.Find ("BOX" + (tempPositionY -1) + "" + (tempPositionX + 1)).GetComponent<Box>());
		if(tempPositionX -1 >= 0)
			addItemToList(transform.Find ("BOX" + tempPositionY  + "" + (tempPositionX - 1)).GetComponent<Box>());
		if(tempPositionX +1 < grm.quantityOfCollumns)
			addItemToList(transform.Find ("BOX" + tempPositionY  + "" + (tempPositionX + 1)).GetComponent<Box>());
		if(tempPositionY +1 < grm.quantityOfLines && tempPositionX -1 >= 0)
			addItemToList(transform.Find ("BOX" + (tempPositionY +1) + "" + (tempPositionX - 1)).GetComponent<Box>());
		if(tempPositionY +1 < grm.quantityOfLines)
			addItemToList(transform.Find ("BOX" + (tempPositionY +1) + "" + tempPositionX ).GetComponent<Box>());
		if(tempPositionY +1 < grm.quantityOfLines && tempPositionX +1 < grm.quantityOfCollumns)
			addItemToList(transform.Find ("BOX" + (tempPositionY +1) + "" + (tempPositionX + 1)).GetComponent<Box>());

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