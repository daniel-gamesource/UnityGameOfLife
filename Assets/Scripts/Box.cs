using System;
using UnityEngine;

//implements iequatable to make the comparations
public class Box : MonoBehaviour, IEquatable<Box> {

	public int positionX;
	public int positionY;
	public bool isChecked = false;

	//Constructor
	public Box(int positionX, int positionY){
		this.positionX = positionX;
		this.positionY = positionY;
		isChecked = false;
	}

	//IEquatable override methods
	public override bool Equals(object obj)
	{
		if (obj == null) {
			return false;
		}

		Box objAsBox = obj as Box;

		if (objAsBox == null) {
			return false;
		} else {
			return Equals (objAsBox);
		}
	}

	//create the hash transforming position X and Position Y into a asc value.
	public override int GetHashCode()
	{
		String hash = (int)Convert.ToChar (positionX) + "" + (int)Convert.ToChar (positionY);
		return int.Parse (hash);
	}

	public bool Equals(Box other)
	{
		if (other == null) {
			return false;
		}
		return (
			this.positionX.Equals(other.positionX) && 
			this.positionY.Equals(other.positionY)
		);
	}
}
