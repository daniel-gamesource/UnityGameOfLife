using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Rules : MonoBehaviour {

	private List<Item> itemsToTest; //letf id public to show on debug
	private List<Box> checkedBoxes;
	private GameResourcesManager grm;
	public int stepCounter = 0;
	public Text stepsUI;//this item is initialized on the editor

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

	//Execute one step
	public void doStep(){
		itemsToTest = new List<Item> ();
		findBoxes();
		executeStep();

		stepCounter++;
		stepsUI.text = stepCounter.ToString();
	}

	//Methor to find the boxes that will be verified
	private void findBoxes (){
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

		//add the neighbors positions above
		if (tempPositionY - 1 >= 0) {
			if (tempPositionX - 1 >= 0) {
				addItemToList (transform.Find ("BOX" + (tempPositionY - 1) + "" + (tempPositionX - 1)).GetComponent<Box> ());
			}

			addItemToList(transform.Find ("BOX" + (tempPositionY -1) + "" + tempPositionX).GetComponent<Box>());

			if (tempPositionX + 1 < grm.quantityOfCollumns) {
				addItemToList (transform.Find ("BOX" + (tempPositionY - 1) + "" + (tempPositionX + 1)).GetComponent<Box> ());
			}
		}

		//add the neighbors from the sides
		if(tempPositionX -1 >= 0)
			addItemToList(transform.Find ("BOX" + tempPositionY  + "" + (tempPositionX - 1)).GetComponent<Box>());
		if(tempPositionX +1 < grm.quantityOfCollumns)
			addItemToList(transform.Find ("BOX" + tempPositionY  + "" + (tempPositionX + 1)).GetComponent<Box>());

		//add the neighbors of under line
		if (tempPositionY + 1 < grm.quantityOfLines) {
			if (tempPositionX - 1 >= 0) {
				addItemToList (transform.Find ("BOX" + (tempPositionY + 1) + "" + (tempPositionX - 1)).GetComponent<Box> ());
			}

			addItemToList(transform.Find ("BOX" + (tempPositionY +1) + "" + tempPositionX ).GetComponent<Box>());

			if (tempPositionX + 1 < grm.quantityOfCollumns) {
				addItemToList (transform.Find ("BOX" + (tempPositionY + 1) + "" + (tempPositionX + 1)).GetComponent<Box> ());
			}
		}

	}


	private void executeStep(){
		if (itemsToTest != null && itemsToTest.Count > 0) {
			Debug.Log ("starting execution");
			foreach(Item item in itemsToTest){
				Box box = item.itemBox;
				int tempPositionX = box.positionX;
				int tempPositionY = box.positionY;
				int neighborCounter = 0;

				//test the neighbors positions above
				if (tempPositionY - 1 >= 0) {
					if (tempPositionX - 1 >= 0 &&
						transform.Find ("BOX" + (tempPositionY - 1) + "" + (tempPositionX - 1)).GetComponent<Box> ().isChecked) {
						neighborCounter++;
					}

					if (transform.Find ("BOX" + (tempPositionY - 1) + "" + tempPositionX).GetComponent<Box> ().isChecked) {
						neighborCounter++;
					}

					if (tempPositionX + 1 < grm.quantityOfCollumns &&
						transform.Find ("BOX" + (tempPositionY - 1) + "" + (tempPositionX + 1)).GetComponent<Box> ().isChecked) {
						neighborCounter++;
					}
				}

				//test the neighbors from the sides
				if (tempPositionX - 1 >= 0 && transform.Find ("BOX" + tempPositionY + "" + (tempPositionX - 1)).GetComponent<Box> ().isChecked) {
					neighborCounter++;
				}

				if (tempPositionX + 1 < grm.quantityOfCollumns &&
				   transform.Find ("BOX" + tempPositionY + "" + (tempPositionX + 1)).GetComponent<Box> ().isChecked) {
					neighborCounter++;
				}

				//test the neighbors of under line
				if (tempPositionY + 1 < grm.quantityOfLines) {
					if (tempPositionX - 1 >= 0 &&
						transform.Find ("BOX" + (tempPositionY + 1) + "" + (tempPositionX - 1)).GetComponent<Box> ().isChecked) {
						neighborCounter++;
					}

					if (transform.Find ("BOX" + (tempPositionY + 1) + "" + tempPositionX).GetComponent<Box> ().isChecked) {
						neighborCounter++;
					}

					if (tempPositionX + 1 < grm.quantityOfCollumns &&
						transform.Find ("BOX" + (tempPositionY + 1) + "" + (tempPositionX + 1)).GetComponent<Box> ().isChecked) {
						neighborCounter++;
					}
				}


				item.isTrueNextStep = executeRules (item.itemBox, neighborCounter);
			}

			updateValues ();
		}

	}

	//RULES of the game
	private bool executeRules (Box box, int neighborcounter){
		if (box.isChecked) {
			//Rule 1
			//for a space that is populated, each cell with one or no neighbors dies
			//Rule 2
			//for a space that is populated, with four or more neighbors dies
			if (neighborcounter < 2 || neighborcounter > 3) {
				return false;
			}

			//Rule 3
			//for a space that is populated, with two or three neighbors is keept
			else if (neighborcounter > 1 && neighborcounter < 4) {
				return true;
			}


			//Rule 4
			//for a space that is not populated, each cell with three neighbors becomes populated
		} else if(neighborcounter == 3) {
			return true;		
		}

		return false;
	}

	private void updateValues(){
		Debug.Log ("updating values");
		//erase the list of boxes
		grm.checkedBoxes = new List<Box> ();
		Toggle tempToggle;

		//update the list of boxes
		foreach (Item item in itemsToTest) {
			tempToggle = item.itemBox.GetComponent<Toggle>();

			if (item.isTrueNextStep) {
				tempToggle.isOn = true;
				grm.addCheckedBox (item.itemBox);
			} else {
				tempToggle.isOn = false;
				item.itemBox.isChecked = false;
			}
		}
	}

}