using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Data;
using sportify.DAL.Repositories.Contracts;

namespace sportify.BLL.Services
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDTO> GetUserById(string userId)
        {
            var user = await _unitOfWork.UserRepository.GetUserById(userId);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<List<UserDTO>> GetUsersByIds(List<string> userIds)
        {
            var users = await _unitOfWork.UserRepository.GetUsersByIds(userIds);
            return _mapper.Map<List<UserDTO>>(users);
        }
    }
}
