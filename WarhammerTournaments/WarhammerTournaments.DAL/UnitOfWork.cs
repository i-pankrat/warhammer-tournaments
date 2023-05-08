using Microsoft.AspNetCore.Http;
using WarhammerTournaments.DAL.Data;
using WarhammerTournaments.DAL.Entity;
using WarhammerTournaments.DAL.Interface;
using WarhammerTournaments.DAL.Repository;

namespace WarhammerTournaments.DAL;

public sealed class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ApplicationDbContext _context;
    private TournamentRepository _tournamentRepository;
    private IEntityRepository<Application> _applicationRepository;
    private IUserRepository _userRepository;

    public UnitOfWork(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public TournamentRepository TournamentRepository
    {
        get
        {
            if (_tournamentRepository == null)
            {
                _tournamentRepository = new TournamentRepository(_context);
            }

            return _tournamentRepository;
        }
    }

    public IUserRepository UserRepository
    {
        get
        {
            if (_userRepository == null)
            {
                _userRepository = new UserRepository(_context, _httpContextAccessor);
            }

            return _userRepository;
        }
    }

    public IEntityRepository<Application> ApplicationRepository
    {
        get
        {
            if (_applicationRepository == null)
            {
                _applicationRepository = new EntityRepository<Application>(_context);
            }

            return _applicationRepository;
        }
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }


    private bool _disposed;

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}