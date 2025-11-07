using AutoMapper;
using UserStudentMgmt.Application.DTOs.Users;
using UserStudentMgmt.Application.Interfaces;
using UserStudentMgmt.Domain.Interfaces;

namespace UserStudentMgmt.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepo, IMapper mapper)
    {
        _userRepo = userRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        return _mapper.Map<UserDto>(user);
    }
    

    public async Task UpdateAsync(int id, UserRequestDto dto)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException("User not found");
        
        _mapper.Map(dto, user);
        await _userRepo.UpdateAsync(user);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        await _userRepo.DeleteAsync(user);
    }
}