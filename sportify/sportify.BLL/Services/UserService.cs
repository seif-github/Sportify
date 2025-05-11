using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Repositories.Contracts;

namespace sportify.BLL.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepo, IMapper mapper)
        {
            this._userRepo = userRepo;
            this._mapper = mapper;
        }

        public async Task<List<UserDTO>> GetUsersByIds(List<string> userIds)
        {
            return _mapper.Map<List<UserDTO>>(await _userRepo.GetUsersByIds(userIds));
        }
    }
}
