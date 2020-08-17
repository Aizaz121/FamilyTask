using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebClient.Abstractions;

namespace WebClient.Pages
{
    public class MembersBase : ComponentBase
    {
        protected List<FamilyMember> members = new List<FamilyMember>();
        protected List<MenuItem> leftMenuItem = new List<MenuItem>();
        protected FamilyMember mm = new FamilyMember();
        protected bool showCreator;
        protected bool isLoaded;
        protected bool showUpdator;

        [Inject]
        public IMemberDataService MemberDataService { get; set; }

        protected override async Task OnInitializedAsync()
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
                }
            }

            for (int i = 0; i < members.Count; i++)
            {
                leftMenuItem.Add(new MenuItem
                {
                    iconColor = members[i].avtar,
                    label = members[i].firstname,
                    referenceId = members[i].id
                });
            }
            showCreator = true;
            showUpdator = false;
            isLoaded = true;
        }

        protected void onAddItem()
        {
            showCreator = true;
            showUpdator = false;
            StateHasChanged();
        }
        protected async Task xx(Guid refId)
        {
            showCreator = false;
            showUpdator = true;

            //TODO Add GetBy Id Call
            var result = await MemberDataService.GetAllMembers();
            var mm2 = result.Payload.ToList().Where(x => x.Id == refId).FirstOrDefault();
            var mmXX = new FamilyMember()
            {
                avtar = mm2.Avatar,
                email = mm2.Email,
                firstname = mm2.FirstName,
                lastname = mm2.LastName,
                role = mm2.Roles,
                id = mm2.Id
            };
            mm = mmXX;
            StateHasChanged();
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
                members.Add(new FamilyMember()
                {
                    avtar = result.Payload.Avatar,
                    email = result.Payload.Email,
                    firstname = result.Payload.FirstName,
                    lastname = result.Payload.LastName,
                    role = result.Payload.Roles,
                    id = result.Payload.Id
                });

                leftMenuItem.Add(new MenuItem
                {
                    iconColor = result.Payload.Avatar,
                    label = result.Payload.FirstName,
                    referenceId = result.Payload.Id
                });


                showCreator = false;
                showUpdator = false;
                StateHasChanged();
            }

        }

        protected async Task onMemberUpdate(FamilyMember familyMember)
        {
            var tt = "tes";

            var result = await MemberDataService.Update(new Domain.Commands.UpdateMemberCommand()
            {
                Id = familyMember.id,
                Avatar = familyMember.avtar,
                FirstName = familyMember.firstname,
                LastName = familyMember.lastname,
                Email = familyMember.email,
                Roles = familyMember.role
            });

            if (result != null && result.Succeed)
            {
               
                members = new List<FamilyMember>();
                leftMenuItem =new List<MenuItem>();
                await OnInitializedAsync();
                showCreator = false;
                showUpdator = false;
                isLoaded = true;
                StateHasChanged();
            }
          

        }

        protected async Task onMemberDelete(Guid familyMember)
        {
            var tt = "tes";

            var result = await MemberDataService.Delete(familyMember);

            if (result != null && result.Succeed)
            {

                members = new List<FamilyMember>();
                leftMenuItem = new List<MenuItem>();
                await OnInitializedAsync();
                showCreator = false;
                showUpdator = false;
                isLoaded = true;
                StateHasChanged();
            }


        }

    }
}
