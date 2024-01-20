using AutoMapper;
using BE.DataAccess;
using BE.Model;

namespace BE.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetList();
        Task<User> GetById(long id);
        Task<User> GetByName(string name);
        Task Create(UserCreateRequest model);
        Task Update(long id, UserUpdateRequest model);
    }

    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetList()
        {
            return await _userRepository.GetList();
        }

        public async Task<User> GetById(long id)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return user;
        }

        public async Task<User> GetByName(string name)
        {
            var user = await _userRepository.GetByName(name);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return user;
        }

        public async Task Create(UserCreateRequest model)
        {
            // map model to new user object
            var user = _mapper.Map<User>(model);

            // save user
            await _userRepository.Add(user);
        }

        public async Task Update(long id, UserUpdateRequest model)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            // copy model props to user
            _mapper.Map(model, user);

            // save user
            await _userRepository.Update(user);
        }
    }

}
