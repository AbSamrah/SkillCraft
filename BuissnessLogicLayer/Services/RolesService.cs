using AutoMapper;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersManagement.BuissnessLogicLayer.Models;

namespace UsersManagement.BuissnessLogicLayer.Services
{
    public class RolesService
    {
        private IRoleRepository _roleRepository;
        private IMapper _mapper;
        public RolesService(IRoleRepository roleRepository, IMapper mapper)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        public async Task<List<RoleDto>> GetAll()
        {
            var roles = await _roleRepository.GetAllAsync();
            return _mapper.Map<List<RoleDto>>(roles);
        }
    }
}
