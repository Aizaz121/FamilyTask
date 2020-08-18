
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using WebClient.Abstractions;

namespace WebClient.Pages
{
    public class TasksBase : ComponentBase
    {
        protected List<FamilyMember> members = new List<FamilyMember>();
        protected MenuItem[] leftMenuItem;
        protected FamilyMember getmember = new FamilyMember();
        protected bool showCreator;
        protected bool isLoaded;
        protected bool showUpdator;
        protected bool showLister;
        protected TaskModel[] tasksToShow;
        protected List<TaskModel> allTasks = new List<TaskModel>();
        protected string selectedMemberId = "";
        protected string subject = "";
        

        [Inject]
        public IMemberDataService MemberDataService { get; set; }

        [Inject]
        public ITasksDataService TasksDataService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await MemberDataService.GetAllMembers();

                if (result != null && result.Payload != null && result.Payload.Any())
                {
                    foreach (var item in result.Payload)
                    {
                        members.Add(new FamilyMember()
                        {
                            avtar = item.Avatar,
                            email = item.Email,
                            firstname = item.FirstName,
                            lastname = item.LastName,
                            role = item.Roles,
                            id = item.Id
                        });
                        selectedMemberId = members?.First()?.id.ToString();
                    }
                }


                var allTasksResult = await TasksDataService.GetAllTasks();
                if (allTasksResult != null && allTasksResult.Payload != null && allTasksResult.Payload.Any())
                {
                    foreach (var item in allTasksResult.Payload)
                    {
                        allTasks.Add(new TaskModel()
                        {
                            text = item.Subject,
                            isDone = item.IsComplete,
                            member = members.Where(x => x.id == item.AssignedToId).FirstOrDefault(),
                            id = item.Id
                        });
                        allTasks[allTasks.FindIndex(ind => ind.id == item.Id)].ClickCallback += onTaskItemCompleted;
                        allTasks[allTasks.FindIndex(ind => ind.id == item.Id)].ClickCallbackDelete += onTaskDelete;
                    }
                }

                leftMenuItem = new MenuItem[members.Count + 1];
                leftMenuItem[0] = new MenuItem
                {
                    label = "All Tasks",
                    referenceId = Guid.Empty,
                    isActive = true
                };
                leftMenuItem[0].ClickCallback += showAllTasks;
                for (int i = 1; i < members.Count + 1; i++)
                {
                    leftMenuItem[i] = new MenuItem
                    {
                        iconColor = members[i - 1].avtar,
                        label = members[i - 1].firstname,
                        referenceId = members[i - 1].id,
                        isActive = false
                    };
                    leftMenuItem[i].ClickCallback += onItemClick;
                }
                showAllTasks(null, leftMenuItem[0]);
                isLoaded = true;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        protected void onAddItem()
        {
            showLister = false;
            showCreator = true;
            makeMenuItemActive(null);
            StateHasChanged();
        }
        protected  async Task onAddNewTask(TaskModel taskModel)
        {
            try
            {
                var response = TasksDataService.Create(new Domain.Commands.CreateTasksCommand
                {
                    AssignedToId = Guid.Parse(taskModel.selectedMember.ToString()),
                    IsComplete = false,
                    Subject = taskModel.text
                });


                members = new List<FamilyMember>();
                allTasks = new List<TaskModel>();
                tasksToShow = new TaskModel[] { };
                await OnInitializedAsync();
                subject = "";
                StateHasChanged();

            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public List<FamilyMember> getMembers()
        {
            return members;
    }
        protected void onTaskItemCompleted(object sender, TaskModel e)
        {
            var updateResponse = TasksDataService.Update(new Domain.Commands.UpdateTasksCommand {
            Id = e.id,
            Subject =    e.text  ,     
            IsComplete= e.isDone,
            AssignedToId= e.member.id
            });
        }

        protected async void onTaskDelete(object sender, TaskModel e)
        {
            var updateResponse = TasksDataService.Delete(e.id);
            members = new List<FamilyMember>();
            allTasks = new List<TaskModel>();
            tasksToShow = null;
            await OnInitializedAsync();
        }
        
        protected void onItemClick(object sender, object e)
        {
            Guid val = (Guid)e.GetType().GetProperty("referenceId").GetValue(e);
            makeMenuItemActive(e);
            if (allTasks != null && allTasks.Count > 0)
            {
                tasksToShow = allTasks.Where(item =>
                {
                    if (item.member != null)
                    {
                        return item.member.id == val;
                    }
                    else
                    {
                        return false;
                    }
                }).ToArray();
               
            }
            showLister = true;
            showCreator = false;
            StateHasChanged();
        }
        protected void showAllTasks(object sender, object e)
        {
            tasksToShow = allTasks?.ToArray();
            showLister = true;
            showCreator = false;
            makeMenuItemActive(e);
            StateHasChanged();
        }

        protected void makeMenuItemActive(object e)
        {
            foreach (var item in leftMenuItem)
            {
                item.isActive = false;
            }
            if (e != null)
            {
                e.GetType().GetProperty("isActive").SetValue(e, true);
            }
        }

        protected async Task onMemberAdd(FamilyMember familyMember)
        {
            var result = await MemberDataService.Create(new Domain.Commands.CreateMemberCommand()
            {
                Avatar = familyMember.avtar,
                FirstName = familyMember.firstname,
                LastName = familyMember.lastname,
                Email = familyMember.email,
                Roles = familyMember.role
            });

            if (result != null && result.Payload != null && result.Payload.Id != Guid.Empty)
            {
                members = new List<FamilyMember>();
                allTasks = new List<TaskModel>();
                tasksToShow = new TaskModel[] { };
                await OnInitializedAsync();
                StateHasChanged();
            }

        }
    }
}
