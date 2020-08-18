using System;

public class TaskModel
{
    public Guid id {get; set;}
    public FamilyMember member { get; set; }
    public string text { get; set; }
    public bool isDone { get; set; }

    public Guid selectedMember{ get; set; }

    protected virtual void OnClickCallback(TaskModel e)
    {
        EventHandler<TaskModel> handler = ClickCallback;
        if (handler != null)
        {
            handler(this, e);
        }
    }
    public event EventHandler<TaskModel> ClickCallback;
    public void InvokClickCallback(TaskModel e)
    {


        OnClickCallback(e);
    }

    public event EventHandler<TaskModel> ClickCallbackDelete;


    protected virtual void OnClickCallbackDelete(TaskModel e)
    {
        EventHandler<TaskModel> handler = ClickCallbackDelete;
        if (handler != null)
        {
            handler(this, e);
        }
    }
    public void InvokClickCallbackDelete(TaskModel e)
    {
        OnClickCallbackDelete(e);
    }


}
