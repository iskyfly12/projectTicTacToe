using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public PeaceType peaceType { get; private set; }
    public int row { get; private set; }
    public int col { get; private set; }
    public bool isEmpty { get { return peaceType == PeaceType.None; } }

    private GameObject peace;

    private event Action<Grid> onGridClick;
    private event Action<Grid> onGridSelect;

    public void Init(int row, int col, Action<Grid> onGridClickAction, Action<Grid> onGridSelectAction)
    {
        this.row = row;
        this.col = col;

        name = string.Format("Grid [{0}][{1}]", row, col);

        onGridClick = onGridClickAction;
        onGridSelect = onGridSelectAction;

        Clear();
    }

    public void SetPeace(PeaceType peaceType)
    {
        this.peaceType = peaceType;

        switch (peaceType)
        {
            case PeaceType.None:
                if (peace != null)
                    peace.SetActive(false);
                peace = null;
                break;
            case PeaceType.Cross:
                peace = ObjectPoolerSystem.SpawFromPool("CrossPeace");
                peace.transform.position = new Vector3(transform.position.x, peace.transform.position.y, transform.position.z);
                break;
            case PeaceType.Nought:
                peace = ObjectPoolerSystem.SpawFromPool("NoughtPeace");
                peace.transform.position = new Vector3(transform.position.x, peace.transform.position.y, transform.position.z);
                break;
        }
    }

    public void SimulatePeace(PeaceType peaceType)
    {
        this.peaceType = peaceType;
    }

    public void Clear() => SetPeace(PeaceType.None);

    public bool HasPeace(PeaceType peaceType) { return this.peaceType == peaceType; }

    public void MakeMove() => onGridClick(this);

    void OnMouseEnter()
    {
        if (onGridSelect == null)
            return;

        onGridSelect(this);
    }

    void OnMouseDown()
    {
        if (onGridClick == null)
            return;

        onGridClick(this);
    }
}
