using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class Create_Quest : ScriptableObject
{


        public TypeOfQuest Type;
        new public string name = "New quest";
        new public string description = "Description";
        new public string alreadyTakenQuest = "Taken Quest";
        new public string completedQuest = "Completed Quest";
        
    new public string Target = "Name Of target";
        new public int count;
        new public bool Finished;
        new public bool AtNpcPlace;
        new public short money;
    //new public int XPgain;
    // public bool isDefaultItem = false;


    //Debug.Log("Using " + name);

    public void RemoveFromInventory()
        {
            //  inventory.instance.Remove(this);
        }

}
    public enum TypeOfQuest
    {
        None,
        Kill,
        Gathering,
        interact,
        First,
    }


