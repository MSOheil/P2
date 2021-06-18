using Microsoft.AspNetCore.Mvc;
using RShop.Data;
using RShop.Data.Repositories;
using RShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RShop.Components
{
    public class ProductGroupComponent:ViewComponent
    {
        private IGroupRepository _groupRepository;
        public ProductGroupComponent(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

       public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("/Views/Component/ProductGroupComponent.cshtml", _groupRepository.GetGroupForShow());
        }


    }
}
