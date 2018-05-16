using System.Collections.Generic;
using Contexts;
using Contexts.Concrete;
using Data;
using States.Concrete;
using UnityEngine;
using UnityEngine.UI;

public class StackFSMBehaviour : MonoBehaviour
{
    [SerializeField]
    private AntData _antData;

    public Transform LeafTransform;
    public Transform HomeTransform;
    public string CurrentStateName;


    public AntContext AntContext;
   
    private void OnEnable()
    {
        _antData = Instantiate(_antData);
        _antData.Inventory = new List<string>();
        AntContext = new AntContext(new StackFindLeafState(), _antData);
        
    }

    [Range(1,3)]
    public float speed = 3;
    public virtual void Update()
    {
        Debug.DrawLine(transform.position, transform.position + _antData.Velocity);

        var mousepos = Input.mousePosition;
        mousepos.z = 10;//this is aggrivating af....

        //update the data
        _antData.CursorPosition = Camera.main.ScreenToWorldPoint(mousepos);
        _antData.AntPosition = transform.position;
        _antData.HomePosition = HomeTransform.position;
        _antData.LeafPosition = LeafTransform.position;


        transform.position += _antData.Velocity * Time.deltaTime * speed;

        AntContext.Update(this);

        CurrentStateName = AntContext.CurrentState.ToString();
    }
}